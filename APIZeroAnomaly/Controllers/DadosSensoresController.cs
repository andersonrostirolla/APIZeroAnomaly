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

        public List<DadosSensor> listarDados(int IdSensor)
        {
            dbmongo.criarConexaoDB();

            var id = IdSensor;

            var sensores = dbmongo.getColuna()
                .Find(b => b.IdSensor == id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            return sensores;
        }

        //get All
        public List<DadosSensor> Get()
        {
            dbmongo.criarConexaoDB();

            var sensores = dbmongo.getColuna().Find(_ => true).ToList();

            return sensores;
        }

        public void tratarAnomalia(int Vizinhos, int IdSensor)
        {
            List<Double> listDados = new List<Double>();
            Algoritmo alg = new Algoritmo();
            List<String> classificacao = new List<String>(); 
            
            var id = IdSensor;

            dbmongo.criarConexaoDB();

            var sensores = dbmongo.getColuna()
                .Find(b => b.IdSensor == id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            //consultar os valores definidos de min e max para o sensor passado
            var configSensor = dbmongo.getColunaConfigSensor()
                .Find(b => b.IdSensor == id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            for (int i = 0; i < sensores.Count; i++)
            {
                listDados.Add(sensores[i].Valor);
                alg.setClassificacao(sensores[i].Valor, configSensor[0].Min, configSensor[0].Max);
                classificacao.Add(alg.getClassificacao());
            }

            alg.trataAnomalia(listDados,Vizinhos, configSensor[0].Min, configSensor[0].Max, false);

            var filter = Builders<DadosSensor>.Filter.Eq("IdSensor", id);

            for (int j = 0; j < sensores.Count; j++)
            {
                if (classificacao[j] == "1")
                {
                    filter = Builders<DadosSensor>.Filter.Eq("Data", sensores[j].Data);

                    var update = Builders<DadosSensor>.Update
                        .Set("Valor", listDados[j]);

                    dbmongo.getColuna().UpdateOneAsync(filter, update);
                }
            }
        }

        public void tratarAnomaliaRede(int Vizinhos, int IdSensor, bool TrataTempoReal)
        {
            List<Double> listDados = new List<Double>();
            Algoritmo alg = new Algoritmo();
            List<String> classificacao = new List<String>();

            var id = IdSensor;

            dbmongo.criarConexaoDB();

            //pegando os ultimos vizinhos[pode ser 3,2 ou 1] valores
            var sensores = dbmongo.getColuna()
                .Find(b => b.IdSensor == id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            var sensorDados = dbmongo.getColuna()
                .Find(b => b.IdSensor == id)
                .Limit(Vizinhos+1)
                .Skip(sensores.Count()-(Vizinhos+1))
                .ToListAsync()
                .Result;

            //consultar os valores definidos de min e max para o sensor passado
            var configSensor = dbmongo.getColunaConfigSensor()
                .Find(b => b.IdSensor == id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            for (int i = 0; i < sensorDados.Count; i++)
            {
                listDados.Add(sensorDados[i].Valor);
                alg.setClassificacao(sensorDados[i].Valor, configSensor[0].Min, configSensor[0].Max);
                classificacao.Add(alg.getClassificacao());
            }

            alg.trataAnomalia(listDados, Vizinhos, configSensor[0].Min, configSensor[0].Max, TrataTempoReal);

            var filter = Builders<DadosSensor>.Filter.Eq("IdSensor", id);

            for (int j = 0; j < sensorDados.Count; j++)
            {
                if (classificacao[j] == "1")
                {
                    filter = Builders<DadosSensor>.Filter.Eq("Data", sensorDados[j].Data);

                    var update = Builders<DadosSensor>.Update
                        .Set("Valor", listDados[j]);

                    dbmongo.getColuna().UpdateOneAsync(filter, update);
                }
            }
        }

        //se redeSensor for true, então procurar nos sensores da rede se existe algum valor sem anomalia.
        public void Post(int IdSensor, double Valor, bool TrataTempoReal, int VizinhosPadrao)
        {
            dbmongo.criarConexaoDB();
            Algoritmo alg = new Algoritmo();
            int cont = 0;
            double valorOriginal = 0.0;
            //verifica se é anomalia
            alg.setClassificacao(valor, min, max);
            //caso for anomalia, trata imediatamente com valores dos outros sensores da mesma rede
            if (redeSensor && alg.getClassificacao() == "1")
            {
                var id = idSensor;

                //aqui eu pego a rede do sensor
                var rede = dbmongo.getColunaRede()
                    .Find(b => b.idSensor == id)
                    .SortBy(b => b.idSensor)
                    .ToListAsync()
                    .Result;

                //aqui eu pego todos os sensores na mesma rede
                var rede2 = dbmongo.getColunaRede()
                    .Find(b => b.idRede == rede[0].idRede)
                    .SortBy(b => b.idRede)
                    .ToListAsync()
                    .Result;

                for (int i = 0; i < rede2.Count; i++)
                {
                    //aqui confiro se o sensor que estou olhando não é o mesmo que estou passando
                    if (idSensor != rede2[i].idSensor)
                    {
                        //aqui eu percorro a lista de sensores olhando os ultimos dados para verificar se é ou não anomalia, se não for eu salvo este dado no lugar em tempo real;
                        var dados = dbmongo.getColuna()
                        .Find(b => b.idSensor == rede2[i].idSensor)
                        .SortBy(b => b.idSensor)
                        .ToListAsync()
                        .Result;

                        //pega a classificação do ultimo valor
                        //se ele for anomalia testa outro sensor da rede, se nao for anomalia, substitui o valor.

                        //caso nenhum valor encontrado nos sensores possa ser trocado, chama o método trataAnomaliaRede que pega os ultimos valores e trata.
                        if (dados.Count > 0)
                        {
                            alg.setClassificacao(dados[dados.Count() - 1].valor, min, max);
                            if (alg.getClassificacao() == "0")
                            {
                                valorOriginal = valor;
                                valor = dados[dados.Count() - 1].valor;
                                cont = 1;
                                break;
                            }
                        }
                    }
                }
            } else
            {
                cont = 1;
            }

            if ((!string.IsNullOrEmpty(Convert.ToString(idSensor))) && (!string.IsNullOrEmpty(Convert.ToString(valor))))
            {
                DadosSensor dados = new DadosSensor();
                dados.idSensor = idSensor;
                dados.valor = valor;
                if (valorOriginal == 0.0)
                    dados.valorOriginal = valor;
                else
                    dados.valorOriginal = valor;
                dados.data = DateTime.Now;

                dbmongo.getColuna().InsertOne(dados);
            }

            if (cont == 0)
            {
                tratarAnomaliaRede(vizinhos, min, max, idSensor, redeSensor);
            }
        }

        public void Put(int idSensor, double valor)
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
