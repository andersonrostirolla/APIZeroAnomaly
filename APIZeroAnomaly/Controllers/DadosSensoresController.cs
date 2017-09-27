using APIZeroAnomaly.Models;
using APIZeroAnomaly.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace APIZeroAnomaly.Controllers
{
    public class DadosSensoresController : ApiController
    {
        //DBCon dbmongo = new DBCon();

        public List<DadosSensor> Get()
        {
            var dbmongo = new MongoClient();
            var db = dbmongo.GetDatabase("dadosSensor");
            var coll = db.GetCollection<DadosSensor>("dadosSensor");

            //var id = new ObjectId("59cada0dbf98224ccd040111");
            var id = "1";

            var sensores = coll
                .Find(b => b.idSensor == id)
                .SortBy(b => b.idSensor)
                .Limit(5)
                .ToListAsync()
                .Result;

            return sensores;
        }

        public void Post(string idSensor, string valor)
        {
            var dbmongo = new MongoClient();
            var db = dbmongo.GetDatabase("dadosSensor");
            var coll = db.GetCollection<DadosSensor>("dadosSensor");

            if ((!string.IsNullOrEmpty(idSensor)) && (!string.IsNullOrEmpty(valor)))
            {
                DadosSensor dados = new DadosSensor();
                dados.idSensor = idSensor;
                dados.valor = valor;
                dados.data = DateTime.Now;

                coll.InsertOne(dados);
            }
        }

        public void Put(string idSensor)
        {
            var dbmongo = new MongoClient();
            var db = dbmongo.GetDatabase("dadosSensor");
            var coll = db.GetCollection<DadosSensor>("dadosSensor");

            var filter = Builders<DadosSensor>.Filter.Eq("idSensor", idSensor);

            var update = Builders<DadosSensor>.Update
                .Set("valor", "15.2")
                .Set("data", DateTime.Now);

            coll.UpdateOneAsync(filter, update);
        }

    }
}
