using APIZeroAnomaly.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.DB
{
    public class DBCon
    {
        private IMongoDatabase db;
        protected IMongoCollection<DadosSensor> coll;
        protected IMongoCollection<SensorRede> collRede;

        public void criarConexaoDB()
        {
            MongoClient dbmongo = new MongoClient();
            db = dbmongo.GetDatabase("dadosSensor");
            coll = db.GetCollection<DadosSensor>("dadosSensor");
            collRede = db.GetCollection<SensorRede>("sensorRede");
        }

        public IMongoCollection<DadosSensor> getColuna()
        {
            return coll;
        }

        public IMongoCollection<SensorRede> getColunaRede()
        {
            return collRede;
        }
    }
    
}