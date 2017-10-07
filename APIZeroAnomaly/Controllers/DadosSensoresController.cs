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
using System.Web.Http.Description;

namespace APIZeroAnomaly.Controllers
{
    public class DadosSensoresController : ApiController
    {
        private DBCon dbmongo = new DBCon();


        //Tras todos
        public List<DadosSensor> GetAllDadosSensores()
        {
            dbmongo.criarConexaoDB();

            var dados = dbmongo.getColunaDados().Find(_ => true).ToList();

            return dados;
        }

        [ResponseType(typeof(DadosSensor))]
        public IHttpActionResult GetDadosSensor(int Id)
        {
            dbmongo.criarConexaoDB();

            var dados = dbmongo.getColunaDados()
                .Find(b => b.IdSensor == Id)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

            if (dados == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dados);
            }
        }

        public void tratarAnomalia(int IdSensor)
        {
            List<Double> ListDados = new List<Double>();
            TrataAnomalia tAnomalia = new TrataAnomalia();
            List<String> classificacao = new List<String>(); 

            dbmongo.criarConexaoDB();

            //verificar se o sensor que estou passando para inserção existe
            var verifSensor = dbmongo.getColunaConfigSensor()
                .Find(b => b.IdSensor == IdSensor)
                .ToListAsync()
                .Result;
            
            //pois só tenho tratamento até 3 vizinhos
            if (verifSensor.Count > 0)
            {

                var sensores = dbmongo.getColunaDados()
                    .Find(b => b.IdSensor == IdSensor)
                    .SortBy(b => b.IdSensor)
                    .ToListAsync()
                    .Result;

                //consultar os valores definidos de min e max para o sensor passado
                var configSensor = dbmongo.getColunaConfigSensor()
                    .Find(b => b.IdSensor == IdSensor)
                    .SortBy(b => b.IdSensor)
                    .ToListAsync()
                    .Result;

                for (int i = 0; i < sensores.Count; i++)
                {
                    ListDados.Add(sensores[i].Valor);
                    tAnomalia.setClassificacao(sensores[i].Valor, configSensor[0].Min, configSensor[0].Max);
                    classificacao.Add(tAnomalia.getClassificacao());
                }

                tAnomalia.trataAnomalia(ListDados, configSensor[0].VizinhosPadrao, configSensor[0].Min, configSensor[0].Max, false);

                var filter = Builders<DadosSensor>.Filter.Eq("IdSensor", IdSensor);

                for (int j = 0; j < sensores.Count; j++)
                {
                    if (classificacao[j] == "1")
                    {
                        filter = Builders<DadosSensor>.Filter.Eq("Data", sensores[j].Data);

                        var update = Builders<DadosSensor>.Update
                            .Set("Valor", ListDados[j]);

                        dbmongo.getColunaDados().UpdateOneAsync(filter, update);
                    }
                }
            }
        }

        public void tratarAnomaliaRede(int Vizinhos, int IdSensor, bool TrataTempoReal)
        {
            List<Double> ListDados = new List<Double>();
            TrataAnomalia tAnomalia = new TrataAnomalia();
            List<String> classificacao = new List<String>();

            dbmongo.criarConexaoDB();

            //verificar se o sensor que estou passando para inserção existe
            var verifSensor = dbmongo.getColunaConfigSensor()
                .Find(b => b.IdSensor == IdSensor)
                .ToListAsync()
                .Result;

            //pois só tenho tratamento até 3 vizinhos
            if (Vizinhos < 3 & verifSensor.Count > 0)
            {
                //pegando os ultimos vizinhos[pode ser 3,2 ou 1] valores
                var sensores = dbmongo.getColunaDados()
                .Find(b => b.IdSensor == IdSensor)
                .SortBy(b => b.IdSensor)
                .ToListAsync()
                .Result;

                var sensorDados = dbmongo.getColunaDados()
                    .Find(b => b.IdSensor == IdSensor)
                    .Limit(Vizinhos + 1)
                    .Skip(sensores.Count() - (Vizinhos + 1))
                    .ToListAsync()
                    .Result;

                //consultar os valores definidos de min e max para o sensor passado
                var configSensor = dbmongo.getColunaConfigSensor()
                    .Find(b => b.IdSensor == IdSensor)
                    .SortBy(b => b.IdSensor)
                    .ToListAsync()
                    .Result;

                for (int i = 0; i < sensorDados.Count; i++)
                {
                    ListDados.Add(sensorDados[i].Valor);
                    tAnomalia.setClassificacao(sensorDados[i].Valor, configSensor[0].Min, configSensor[0].Max);
                    classificacao.Add(tAnomalia.getClassificacao());
                }

                tAnomalia.trataAnomalia(ListDados, Vizinhos, configSensor[0].Min, configSensor[0].Max, TrataTempoReal);

                var filter = Builders<DadosSensor>.Filter.Eq("IdSensor", IdSensor);

                for (int j = 0; j < sensorDados.Count; j++)
                {
                    if (classificacao[j] == "1")
                    {
                        filter = Builders<DadosSensor>.Filter.Eq("Data", sensorDados[j].Data);

                        var update = Builders<DadosSensor>.Update
                            .Set("Valor", ListDados[j]);

                        dbmongo.getColunaDados().UpdateOneAsync(filter, update);
                    }
                }
            }
        }

        //se TrataTempoReal for true, então procurar nos sensores da rede se existe algum Valor sem anomalia.
        public void Post(int IdSensor, double Valor, bool TrataTempoReal)
        {
            dbmongo.criarConexaoDB();
            TrataAnomalia tAnomalia = new TrataAnomalia();
            int Cont = 0;
            double ValorOriginal = 0.0;

            //verificar se o sensor que estou passando para inserção existe
            var verifSensor = dbmongo.getColunaConfigSensor()
                .Find(b => b.IdSensor == IdSensor)
                .ToListAsync()
                .Result;

            if (verifSensor.Count > 0)
            {
                //consultar os valores definidos de min e max para o sensor passado
                var configSensor = dbmongo.getColunaConfigSensor()
                    .Find(b => b.IdSensor == IdSensor)
                    .SortBy(b => b.IdSensor)
                    .ToListAsync()
                    .Result;

                //verifica se é anomalia
                tAnomalia.setClassificacao(Valor, configSensor[0].Min, configSensor[0].Max);
                //caso for anomalia, trata imediatamente com valores dos outros sensores da mesma rede
                if (TrataTempoReal && tAnomalia.getClassificacao() == "1")
                {
                    //aqui eu pego a rede do sensor
                    var rede = dbmongo.getColunaRede()
                        .Find(b => b.IdSensor == IdSensor)
                        .SortBy(b => b.IdSensor)
                        .ToListAsync()
                        .Result;

                    //aqui eu pego todos os sensores na mesma rede
                    var rede2 = dbmongo.getColunaRede()
                        .Find(b => b.IdRede == rede[0].IdRede)
                        .SortBy(b => b.IdRede)
                        .ToListAsync()
                        .Result;

                    for (int i = 0; i < rede2.Count; i++)
                    {
                        //aqui confiro se o sensor que estou olhando não é o mesmo que estou passando
                        if (IdSensor != rede2[i].IdSensor)
                        {
                            //aqui eu percorro a lista de sensores olhando os ultimos dados para verificar se é ou não anomalia, se não for eu salvo este dado no lugar em tempo real;
                            var dados = dbmongo.getColunaDados()
                            .Find(b => b.IdSensor == rede2[i].IdSensor)
                            .SortBy(b => b.IdSensor)
                            .ToListAsync()
                            .Result;

                            //pega a classificação do ultimo Valor
                            //se ele for anomalia testa outro sensor da rede, se nao for anomalia, substitui o Valor.

                            //caso nenhum Valor encontrado nos sensores possa ser trocado, chama o método trataAnomaliaRede que pega os ultimos valores e trata.
                            if (dados.Count > 0)
                            {
                                tAnomalia.setClassificacao(dados[dados.Count() - 1].Valor, configSensor[0].Min, configSensor[0].Max);
                                if (tAnomalia.getClassificacao() == "0")
                                {
                                    ValorOriginal = Valor;
                                    Valor = dados[dados.Count() - 1].Valor;
                                    Cont = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Cont = 1;
                }

                if ((!string.IsNullOrEmpty(Convert.ToString(IdSensor))) && (!string.IsNullOrEmpty(Convert.ToString(Valor))))
                {
                    DadosSensor dados = new DadosSensor();
                    dados.IdSensor = IdSensor;
                    dados.Valor = Valor;
                    if (ValorOriginal == 0.0)
                        dados.ValorOriginal = Valor;
                    else
                        dados.ValorOriginal = Valor;
                    dados.Data = DateTime.Now;

                    dbmongo.getColunaDados().InsertOne(dados);
                }

                //aqui eu pego o numero de vizinhos padrão cadastrado para o sensor passado
                var configSensorVizinhos = dbmongo.getColunaConfigSensor()
                    .Find(b => b.IdSensor == IdSensor)
                    .SortBy(b => b.IdSensor)
                    .ToListAsync()
                    .Result;

                if (Cont == 0)
                {
                    tratarAnomaliaRede(configSensorVizinhos[0].VizinhosPadrao, IdSensor, TrataTempoReal);
                }
            }
        }

        //deletar um registro errado
        public void Delete(int IdSensor, double Valor)
        {
            dbmongo.criarConexaoDB();

            var filter = Builders<DadosSensor>.Filter.Eq("IdSensor", IdSensor) & Builders<DadosSensor>.Filter.Eq("Valor", Valor);

            dbmongo.getColunaDados().DeleteOne(filter);
        }

        //não existe método atualizar para esta classe, pois se o dado for inserido errado é só deletar ou chamar o trata anomalia.

    }
}
