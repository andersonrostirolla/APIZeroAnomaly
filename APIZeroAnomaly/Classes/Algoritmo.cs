﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.Classes
{
    public class Algoritmo
    {
        public string anomalia;

        public void setClassificacao(double valor, double min, double max)
        {
            if (valor > max || valor < min)
            {
                this.anomalia = "1";
            }
            else
            {
                this.anomalia = "0";
            }
        }

        public String getClassificacao()
        {
            return anomalia;
        }

        public List<Double> trataAnomalia(List<Double> listDados, int vizinhos, double min, double max)
        {
            List<String> classificacao = new List<String>();
            Vizinho vizinho = new Vizinho();

            vizinho.setVizinhos(vizinhos);
            vizinho.setMinMax(min, max);

            switch (vizinho.getVizinhos())
            {
                case 1:
                    vizinho.UmVizinho(listDados);

                    for (int i = 0; i < listDados.Count(); i++)
                    {
                        foreach (double row in listDados)
                        {
                            setClassificacao(row, vizinho.getMin(), vizinho.getMax());
                            classificacao.Add(getClassificacao());
                        }

                        if (classificacao.Contains("1"))
                        {
                            vizinho.UmVizinho(listDados);
                            classificacao.Clear();
                        }
                    }
                    break;
                case 2:
                    vizinho.DoisVizinhos(listDados);

                    for (int i = 0; i < listDados.Count(); i++)
                    {
                        foreach (double row in listDados)
                        {
                            setClassificacao(row, vizinho.getMin(), vizinho.getMax());
                            classificacao.Add(getClassificacao());
                        }

                        if (classificacao.Contains("1"))
                        {
                            vizinho.DoisVizinhos(listDados);
                            classificacao.Clear();
                        }
                    }
                    break;
                case 3:
                    vizinho.TresVizinhos(listDados);

                    for (int i = 0; i < listDados.Count(); i++)
                    {
                        foreach (double row in listDados)
                        {
                            setClassificacao(row, vizinho.getMin(), vizinho.getMax());
                            classificacao.Add(getClassificacao());
                        }

                        if (classificacao.Contains("1"))
                        {
                            vizinho.TresVizinhos(listDados);
                            classificacao.Clear();
                        }
                    }
                    break;
                default:
                    return listDados;

            }

            return listDados;

        }
    }
}