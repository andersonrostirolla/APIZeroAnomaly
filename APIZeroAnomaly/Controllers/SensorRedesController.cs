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

        public List<SensorRede> getRede(int idRede)
        {
            dbmongo.criarConexaoDB();

            var id = idRede;

            var rede = dbmongo.getColunaRede()
                .Find(b => b.idRede == id)
                .SortBy(b => b.idSensor)
                .Limit(5)
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

        public void Post(int idRede, int idSensor)
        {
            dbmongo.criarConexaoDB();

            if ((!string.IsNullOrEmpty(Convert.ToString(idRede))) && (!string.IsNullOrEmpty(Convert.ToString(idSensor))))
            {
                SensorRede rede = new SensorRede();
                rede.idRede = idRede;
                rede.idSensor = idSensor;
                rede.data = DateTime.Now;

                dbmongo.getColunaRede().InsertOne(rede);
            }
        }

        public void Put(int idRede, int idSensor)
        {
            dbmongo.criarConexaoDB();

            var filter = Builders<SensorRede>.Filter.Eq("idRede", idRede);

            var update = Builders<SensorRede>.Update
                .Set("idSensor", idSensor)
                .Set("data", DateTime.Now);

            dbmongo.getColunaRede().UpdateOneAsync(filter, update);
        }
    }
}
