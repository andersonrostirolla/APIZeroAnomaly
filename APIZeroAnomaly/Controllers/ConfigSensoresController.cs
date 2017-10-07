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
    public class ConfigSensoresController : ApiController
    {
        private DBCon dbmongo = new DBCon();

        //Tras todos
        public List<ConfigSensor> GetAllConfigSensores()
        {
            dbmongo.criarConexaoDB();

            var config = dbmongo.getColunaConfigSensor().Find(_ => true).ToList();

            return config;
        }

        [ResponseType(typeof(ConfigSensor))]
        public IHttpActionResult GetConfigSensor(int Id)
        {
            dbmongo.criarConexaoDB();

            var config = dbmongo.getColunaConfigSensor()
                .Find(b => b.IdSensor == Id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            if (config == null)
            {
                return NotFound();
            } else
            {
                return Ok(config);
            }
        }

        public void Post(int IdSensor, string Descricao, double Min, double Max, string UnidadeMedida, string Metodo, int VizinhosPadrao)
        {
            dbmongo.criarConexaoDB();

            if ((!string.IsNullOrEmpty(Convert.ToString(IdSensor))) && (!string.IsNullOrEmpty(Convert.ToString(IdSensor))))
            {
                //verificar se já não existe um sensor com o numero apontado para um novo cadastro
                var sensor = dbmongo.getColunaConfigSensor()
                    .Find(b => b.IdSensor == IdSensor)
                    .ToListAsync()
                    .Result;

                if (sensor.Count == 0)
                {
                    ConfigSensor config = new ConfigSensor();
                    config.IdSensor = IdSensor;
                    config.Descricao = Descricao;
                    config.Min = Min;
                    config.Max = Max;
                    config.UnidadeMedida = UnidadeMedida;
                    config.Metodo = Metodo;
                    config.VizinhosPadrao = VizinhosPadrao;
                    config.Data = DateTime.Now;

                    dbmongo.getColunaConfigSensor().InsertOne(config);
                }
            }
        }

        public void Delete(int IdSensor)
        {
            dbmongo.criarConexaoDB();

            var filter = Builders<ConfigSensor>.Filter.Eq("IdSensor", IdSensor);

            dbmongo.getColunaConfigSensor().DeleteOne(filter);
        }

        public void Put(int IdSensor, double Min, double Max, string Metodo)
        {
            dbmongo.criarConexaoDB();

            var filter = Builders<ConfigSensor>.Filter.Eq("IdSensor", IdSensor);

            var update = Builders<ConfigSensor>.Update
                .Set("Min", Min)
                .Set("Max", Max)
                .Set("Metodo", Metodo)
                .Set("Data", DateTime.Now);

            dbmongo.getColunaConfigSensor().UpdateOneAsync(filter, update);
        }
    }
}
