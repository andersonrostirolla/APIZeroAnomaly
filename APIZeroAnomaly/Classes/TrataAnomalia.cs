using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.Classes
{
    public class TrataAnomalia
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

        public List<Double> trataAnomalia(List<Double> listDados, int vizinhos, double min, double max, bool redeSensor)
        {
            List<String> classificacao = new List<String>();
            Vizinho vizinho = new Vizinho();

            vizinho.setVizinhos(vizinhos);
            vizinho.setMinMax(min, max);

            if (redeSensor)
            {
                switch (vizinho.getVizinhos())
                {
                    case 1:
                        vizinho.UmVizinhoRede(listDados);

                        for (int i = 0; i < listDados.Count(); i++)
                        {
                            foreach (double row in listDados)
                            {
                                setClassificacao(row, vizinho.getMin(), vizinho.getMax());
                                classificacao.Add(getClassificacao());
                            }

                            if (classificacao.Contains("1"))
                            {
                                vizinho.UmVizinhoRede(listDados);
                                classificacao.Clear();
                            }
                        }
                        break;
                    case 2:
                        vizinho.DoisVizinhosRede(listDados);

                        for (int i = 0; i < listDados.Count(); i++)
                        {
                            foreach (double row in listDados)
                            {
                                setClassificacao(row, vizinho.getMin(), vizinho.getMax());
                                classificacao.Add(getClassificacao());
                            }

                            if (classificacao.Contains("1"))
                            {
                                vizinho.DoisVizinhosRede(listDados);
                                classificacao.Clear();
                            }
                        }
                        break;
                    case 3:
                        vizinho.TresVizinhosRede(listDados);

                        for (int i = 0; i < listDados.Count(); i++)
                        {
                            foreach (double row in listDados)
                            {
                                setClassificacao(row, vizinho.getMin(), vizinho.getMax());
                                classificacao.Add(getClassificacao());
                            }

                            if (classificacao.Contains("1"))
                            {
                                vizinho.TresVizinhosRede(listDados);
                                classificacao.Clear();
                            }
                        }
                        break;
                    default:
                        return listDados;

                }
            }

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