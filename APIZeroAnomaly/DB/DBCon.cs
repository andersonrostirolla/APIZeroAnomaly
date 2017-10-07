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
        private IMongoDatabase DB;
        protected IMongoCollection<DadosSensor> CollDados;
        protected IMongoCollection<SensorRede> CollRede;
        protected IMongoCollection<ConfigSensor> CollConfigSensor;

        public void criarConexaoDB()
        {
            MongoClient dbmongo = new MongoClient();
            DB = dbmongo.GetDatabase("dados");
            CollDados = DB.GetCollection<DadosSensor>("dadosSensor");
            CollRede = DB.GetCollection<SensorRede>("sensorRede");
            CollConfigSensor = DB.GetCollection<ConfigSensor>("configSensor");
        }

        public IMongoCollection<DadosSensor> getColunaDados()
        {
            return CollDados;
        }

        public IMongoCollection<SensorRede> getColunaRede()
        {
            return CollRede;
        }

        public IMongoCollection<ConfigSensor> getColunaConfigSensor()
        {
            return CollConfigSensor;
        }
    }
    
}