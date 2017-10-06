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
    public class ConfigSensoresController : ApiController
    {
        private DBCon dbmongo = new DBCon();

        public List<ConfigSensor> getConfig(int IdSensor)
        {
            dbmongo.criarConexaoDB();

            var id = IdSensor;

            var config = dbmongo.getColunaConfigSensor()
                .Find(b => b.IdSensor == id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            return config;
        }

        public List<ConfigSensor> Get()
        {
            dbmongo.criarConexaoDB();

            var config = dbmongo.getColunaConfigSensor().Find(_ => true).ToList();

            return config;
        }

        public void Post(int IdSensor, string Descricao, double Min, double Max, string UnidadeMedida, string Metodo)
        {
            dbmongo.criarConexaoDB();

            if ((!string.IsNullOrEmpty(Convert.ToString(IdSensor))) && (!string.IsNullOrEmpty(Convert.ToString(IdSensor))))
            {
                ConfigSensor config = new ConfigSensor();
                config.IdSensor = IdSensor;
                config.Descricao = Descricao;
                config.Min = Min;
                config.Max = Max;
                config.UnidadeMedida = UnidadeMedida;
                config.Metodo = Metodo;
                config.Data = DateTime.Now;

                dbmongo.getColunaConfigSensor().InsertOne(config);
            }
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
