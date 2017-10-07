using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.Classes
{
    public class Vizinho
    {
        public int Vizinhos;
        private int Cont = 0;
        private double Max, Min;

        public List<Double> UmVizinho(List<Double> ListDados)
        {
            Cont = 0;

            for (int i = 0; i <= ListDados.Count() - getVizinhos(); i++)
            {
                if (i >= (ListDados.Count() - getVizinhos()))
                {
                    for (int j = i; j < ListDados.Count(); j++)
                    {
                        Cont = ListDados.Count() - j;

                        if ((ListDados[j] >= getMin()) && (ListDados[j] <= getMax()))
                            ListDados[j] = ListDados[j];
                        else
                        {
                            if (j == ListDados.Count() - Cont && ListDados.Count() >= 1)
                                ListDados[j] = ListDados[j - 1];
                        }
                    }
                }
                else if (i >= getVizinhos())
                {
                    if ((ListDados[i] >= getMin()) && (ListDados[i] <= getMax()))
                        ListDados[i] = ListDados[i];
                    else
                        ListDados[i] = ((ListDados[i + 1] + ListDados[i - 1]) / (getVizinhos() * 2));
                }
                else
                {
                    if ((ListDados[i] >= getMin()) && (ListDados[i] <= getMax()))
                        ListDados[i] = ListDados[i];
                    else
                    {
                        if (ListDados.Count() > 2)
                        {
                            if (ListDados[i + 1] >= getMin())
                                ListDados[i] = ListDados[i + 1];
                            else
                                ListDados[i] = getMin();
                        }
                    }
                }
            }

            return ListDados;
        }

        public List<Double> UmVizinhoRede(List<Double> ListDados)
        {
            ListDados[1] = ListDados[0];
            
            return ListDados;
        }

        public List<Double> DoisVizinhos(List<Double> ListDados)
        {
            Cont = 0;

            for (int i = 0; i <= ListDados.Count() - getVizinhos(); i++)
            {
                if (i >= (ListDados.Count() - getVizinhos()))
                {
                    for (int j = i; j < ListDados.Count(); j++)
                    {
                        Cont = ListDados.Count() - j;

                        if ((ListDados[j] >= getMin()) && (ListDados[j] <= getMax()))
                            ListDados[j] = ListDados[j];
                        else
                        {
                            if (j == ListDados.Count() - Cont && ListDados.Count() >= 2)
                            {
                                if (j < ListDados.Count() - 1)
                                    ListDados[j] = ((ListDados[j - 1] + ListDados[j - 2] + ListDados[j + 1]) / ((getVizinhos() * 2) - 1));
                                else
                                    ListDados[j] = ((ListDados[j - 1] + ListDados[j - 2]) / getVizinhos());
                            }
                        }
                    }
                }
                else if (i >= getVizinhos())
                {
                    if ((ListDados[i] >= getMin()) && (ListDados[i] <= getMax()))
                        ListDados[i] = ListDados[i];
                    else
                        ListDados[i] = ((ListDados[i - 2] + ListDados[i - 1] + ListDados[i + 1] + ListDados[i + 2]) / (getVizinhos() * 2));
                }
                else
                {
                    if ((ListDados[i] >= getMin()) && (ListDados[i] <= getMax()))
                        ListDados[i] = ListDados[i];
                    else
                    {
                        if (ListDados.Count() > 4)
                        {
                            if (i == 0)
                                ListDados[i] = ((ListDados[i + 1] + ListDados[i + 2]) / getVizinhos());
                            else if (i == 1)
                                ListDados[i] = ((ListDados[i + 1] + ListDados[i + 2] + ListDados[i - 1]) / ((getVizinhos() * 2) - 1));
                            else
                                ListDados[i] = getMin();
                        }
                    }
                }
            }

            return ListDados;
        }

        public List<Double> DoisVizinhosRede(List<Double> ListDados)
        {
            ListDados[2] = ((ListDados[1] + ListDados[0]) / getVizinhos());

            return ListDados;
        }

        public List<Double> TresVizinhos(List<Double> ListDados)
        {
            for (int i = 0; i <= ListDados.Count() - getVizinhos(); i++)
            {
                if (i >= (ListDados.Count() - getVizinhos()))
                {
                    for (int j = i; j < ListDados.Count(); j++)
                    {
                        Cont = ListDados.Count() - j;
                        if ((ListDados[j] >= getMin()) && (ListDados[j] <= getMax()))
                            ListDados[j] = ListDados[j];
                        else
                        {
                            if (j == ListDados.Count() - Cont && ListDados.Count() >= 3)
                            {
                                if (j < ListDados.Count() - 2)
                                    ListDados[j] = ((ListDados[j - 1] + ListDados[j - 2] + ListDados[j - 3] + ListDados[j + 2] + ListDados[j + 1]) / ((getVizinhos() * 2) - 1));
                                else if (j < ListDados.Count() - 1)
                                    ListDados[j] = ((ListDados[j - 1] + ListDados[j - 2] + ListDados[j - 3] + ListDados[j + 1]) / ((getVizinhos() * 2) - 2));
                                else
                                    ListDados[j] = ((ListDados[j - 1] + ListDados[j - 2] + ListDados[j - 3]) / getVizinhos());
                            }
                        }
                    }
                }
                else if (i >= getVizinhos())
                {
                    if ((ListDados[i] >= getMin()) && (ListDados[i] <= getMax()))
                        ListDados[i] = ListDados[i];
                    else
                        ListDados[i] = ((ListDados[i - 3] + ListDados[i - 2] + ListDados[i - 1] + ListDados[i + 1] + ListDados[i + 2] + ListDados[i + 3]) / (getVizinhos() * 2));
                }
                else
                {
                    if ((ListDados[i] >= getMin()) && (ListDados[i] <= getMax()))
                        ListDados[i] = ListDados[i];
                    else
                    {
                        if (ListDados.Count() > 6)
                        {
                            if (i == 0)
                                ListDados[i] = ((ListDados[i + 1] + ListDados[i + 2] + ListDados[i + 3]) / getVizinhos());
                            else if (i == 1)
                                ListDados[i] = ((ListDados[i + 1] + ListDados[i + 2] + ListDados[i + 3] + ListDados[i - 1]) / ((getVizinhos() * 2) - 2));
                            else if (i == 2)
                                ListDados[i] = ((ListDados[i + 1] + ListDados[i + 2] + ListDados[i + 3] + ListDados[i - 1] + ListDados[i - 2]) / ((getVizinhos() * 2) - 1));
                            else
                                ListDados[i] = getMin();
                        }
                    }
                }
            }

            return ListDados;

        }

        public List<Double> TresVizinhosRede(List<Double> ListDados)
        {
            ListDados[3] = ((ListDados[2] + ListDados[1] + ListDados[0]) / getVizinhos());

            return ListDados;
        }

        public void setVizinhos(int valor)
        {
            this.Vizinhos = valor;
        }

        public int getVizinhos()
        {
            return this.Vizinhos;
        }

        public void setMinMax(double Min, double Max)
        {
            this.Min = Min;
            this.Max = Max;
        }

        public double getMax()
        {
            return this.Max;
        }

        public double getMin()
        {
            return this.Min;
        }
    }
}
