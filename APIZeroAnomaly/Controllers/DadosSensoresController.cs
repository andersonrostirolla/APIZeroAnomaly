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
using APIZeroAnomaly.Classes;
using System.IO;

namespace APIZeroAnomaly.Controllers
{
    public class DadosSensoresController : ApiController
    {
        private DBCon dbmongo = new DBCon();

        //get 1 sensor
        public List<DadosSensor> Get(string idSensor)
        {
            dbmongo.criarConexaoDB();

            var id = idSensor;

            var sensores = dbmongo.getColuna()
                .Find(b => b.idSensor == id)
                .SortBy(b => b.idSensor)
                .Limit(5)
                .ToListAsync()
                .Result;

            return sensores;
        }

        //get All
        /*public List<DadosSensor> listarAll()
        {
            dbmongo.criarConexaoDB();

            var sensores = dbmongo.getColuna().Find(Builders<DadosSensor>.Filter.Empty).ToList();

            return sensores;
        }*/

        public void tratarAnomalia(int vizinhos, double min, double max, string idSensor)
        {
            List<Double> listDados = new List<Double>();
            Algoritmo alg = new Algoritmo();
            List<String> classificacao = new List<String>(); 
            
            var id = idSensor;

            dbmongo.criarConexaoDB();

            var sensores = dbmongo.getColuna()
                .Find(b => b.idSensor == id)
                .SortBy(b => b.idSensor)
                .Limit(5)
                .ToListAsync()
                .Result;

            for (int i = 0; i < sensores.Count; i++)
            {
                listDados.Add(Convert.ToDouble(sensores[i].valor));
                alg.setClassificacao(Convert.ToDouble(sensores[i].valor), min, max);
                classificacao.Add(alg.getClassificacao());
            }

            alg.trataAnomalia(listDados,vizinhos, min, max);

            var filter = Builders<DadosSensor>.Filter.Eq("idSensor", id);

            for (int j = 0; j < sensores.Count; j++)
            {
                if (classificacao[j] == "1")
                {
                    filter = Builders<DadosSensor>.Filter.Eq("data", sensores[j].data);

                    var update = Builders<DadosSensor>.Update
                        .Set("valor", listDados[j]);

                    dbmongo.getColuna().UpdateOneAsync(filter, update);
                }
            }
        }

        public void Post(string idSensor, double valor)
        {
            dbmongo.criarConexaoDB();

            if ((!string.IsNullOrEmpty(idSensor)) && (!string.IsNullOrEmpty(Convert.ToString(valor))))
            {
                DadosSensor dados = new DadosSensor();
                dados.idSensor = idSensor;
                dados.valor = valor;
                dados.data = DateTime.Now;

                dbmongo.getColuna().InsertOne(dados);
            }
        }

        public void Put(string idSensor, double valor)
        {
            dbmongo.criarConexaoDB();
            var filter = Builders<DadosSensor>.Filter.Eq("idSensor", idSensor);

            var update = Builders<DadosSensor>.Update
                .Set("valor", valor)
                .Set("data", DateTime.Now);

            dbmongo.getColuna().UpdateOneAsync(filter, update);
        }

    }
}
