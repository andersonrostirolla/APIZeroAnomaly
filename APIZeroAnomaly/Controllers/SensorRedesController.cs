using APIZeroAnomaly.DB;
using APIZeroAnomaly.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIZeroAnomaly.Controllers
{
    public class SensorRedesController : ApiController
    {
        private DBCon dbmongo = new DBCon();

        public List<SensorRede> getRede(int IdRede)
        {
            dbmongo.criarConexaoDB();

            var id = IdRede;

            var rede = dbmongo.getColunaRede()
                .Find(b => b.IdRede == id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            return rede;
        }

        public List<SensorRede> Get()
        {
            dbmongo.criarConexaoDB();

            var rede = dbmongo.getColunaRede().Find(_ => true).ToList();

            return rede;
        }

        public void Post(int IdRede, int IdSensor)
        {
            dbmongo.criarConexaoDB();

            if ((!string.IsNullOrEmpty(Convert.ToString(IdRede))) && (!string.IsNullOrEmpty(Convert.ToString(IdSensor))))
            {
                SensorRede rede = new SensorRede();
                rede.IdRede = IdRede;
                rede.IdSensor = IdSensor;
                rede.Data = DateTime.Now;

                dbmongo.getColunaRede().InsertOne(rede);
            }
        }

        public void Put(int IdRede, int IdSensor)
        {
            dbmongo.criarConexaoDB();

            var filter = Builders<SensorRede>.Filter.Eq("IdRede", IdRede);

            var update = Builders<SensorRede>.Update
                .Set("IdSensor", IdSensor)
                .Set("Data", DateTime.Now);

            dbmongo.getColunaRede().UpdateOneAsync(filter, update);
        }
    }
}
