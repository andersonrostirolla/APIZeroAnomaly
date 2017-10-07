using APIZeroAnomaly.DB;
using APIZeroAnomaly.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace APIZeroAnomaly.Controllers
{
    public class SensorRedesController : ApiController
    {
        private DBCon dbmongo = new DBCon();

        //Tras todos
        public List<SensorRede> GetAllSensorRedes()
        {
            dbmongo.criarConexaoDB();

            var sensor = dbmongo.getColunaRede().Find(_ => true).ToList();

            return sensor;
        }

        [ResponseType(typeof(SensorRede))]
        public IHttpActionResult GetSensorRede(int Id)
        {
            dbmongo.criarConexaoDB();

            var sensor = dbmongo.getColunaRede()
                .Find(b => b.IdRede == Id)
                .SortBy(b => b.IdRede)
                .ToListAsync()
                .Result;

            if (sensor == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(sensor);
            }
        }

        public void Post(int IdRede, int IdSensor)
        {
            dbmongo.criarConexaoDB();

            if ((!string.IsNullOrEmpty(Convert.ToString(IdRede))) && (!string.IsNullOrEmpty(Convert.ToString(IdSensor))))
            {
                var redeSensor = dbmongo.getColunaRede()
                .Find(b => b.IdSensor == IdSensor) 
                .ToListAsync()
                .Result;

                var sensorCadastrado = dbmongo.getColunaConfigSensor()
                    .Find(b => b.IdSensor == IdSensor)
                    .ToListAsync()
                    .Result;

                //se a IdSensor não existir em alguma rede insere
                if (redeSensor.Count == 0 && sensorCadastrado.Count > 0)
                {
                    SensorRede rede = new SensorRede();
                    rede.IdRede = IdRede;
                    rede.IdSensor = IdSensor;
                    rede.Data = DateTime.Now;

                    dbmongo.getColunaRede().InsertOne(rede);
                }
            }
        }

        public void Delete(int IdRede)
        {
            dbmongo.criarConexaoDB();

            var filter = Builders<SensorRede>.Filter.Eq("IdRede", IdRede);

            dbmongo.getColunaRede().DeleteMany(filter);
        }

        //delete do sensor na rede
        public void DeleteSensor(int IdRede, int IdSensor)
        {
            dbmongo.criarConexaoDB();

            var filter = Builders<SensorRede>.Filter.Eq("IdSensor", IdSensor) & Builders<SensorRede>.Filter.Eq("IdRede", IdRede);

            dbmongo.getColunaRede().DeleteOne(filter);
        }

        //atualiza o sensor trocando-o de rede.
        public void Put(int IdRede, int IdSensor)
        {
            dbmongo.criarConexaoDB();

            var sensor = dbmongo.getColunaRede()
                .Find(b => b.IdRede == IdRede)
                .ToListAsync()
                .Result;

            //Se entrar quer dizer que deu boa, encontrou no banco a rede e 
            if (sensor[0].IdRede == IdRede)
            {
                //pegar qual a rede atual do sensor
                var sensor1 = dbmongo.getColunaRede()
                    .Find(b => b.IdSensor == IdSensor)
                    .ToListAsync()
                    .Result;

                var filter = Builders<SensorRede>.Filter.Eq("IdRede", sensor1[0].IdRede) & Builders<SensorRede>.Filter.Eq("IdSensor", IdSensor);

                dbmongo.getColunaRede().DeleteOne(filter);

                SensorRede rede = new SensorRede();
                rede.IdRede = IdRede;
                rede.IdSensor = IdSensor;
                rede.Data = DateTime.Now;

                dbmongo.getColunaRede().InsertOne(rede);
            }
        }
    }
}
