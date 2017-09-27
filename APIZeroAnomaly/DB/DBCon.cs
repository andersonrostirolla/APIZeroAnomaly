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

        public void criarConexaoDB()
        {
            var client = new MongoClient();
        }
    }
}