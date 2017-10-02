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

        public void criarConexaoDB()
        {
            MongoClient dbmongo = new MongoClient();
            db = dbmongo.GetDatabase("dadosSensor");
            coll = db.GetCollection<DadosSensor>("dadosSensor");
        }

        public IMongoCollection<DadosSensor> getColuna()
        {
            return coll;
        }
    }
    
}