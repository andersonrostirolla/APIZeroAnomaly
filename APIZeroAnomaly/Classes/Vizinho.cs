using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.Classes
{
    public class Vizinho
    {
        public int vizinhos;
        private int cont = 0;
        private double max, min;

        public List<Double> UmVizinho(List<Double> listDados)
        {
            cont = 0;

            for (int i = 0; i <= listDados.Count() - getVizinhos(); i++)
            {
                if (i >= (listDados.Count() - getVizinhos()))
                {
                    for (int j = i; j < listDados.Count(); j++)
                    {
                        cont = listDados.Count() - j;

                        if ((listDados[j] >= getMin()) && (listDados[j] <= getMax()))
                            listDados[j] = listDados[j];
                        else
                        {
                            if (j == listDados.Count() - cont && listDados.Count() >= 1)
                                listDados[j] = listDados[j - 1];
                        }
                    }
                }
                else if (i >= getVizinhos())
                {
                    if ((listDados[i] >= getMin()) && (listDados[i] <= getMax()))
                        listDados[i] = listDados[i];
                    else
                        listDados[i] = ((listDados[i + 1] + listDados[i - 1]) / (getVizinhos() * 2));
                }
                else
                {
                    if ((listDados[i] >= getMin()) && (listDados[i] <= getMax()))
                        listDados[i] = listDados[i];
                    else
                    {
                        if (listDados.Count() > 2)
                        {
                            if (listDados[i + 1] >= getMin())
                                listDados[i] = listDados[i + 1];
                            else
                                listDados[i] = getMin();
                        }
                    }
                }
            }

            return listDados;
        }

        public List<Double> DoisVizinhos(List<Double> listDados)
        {
            cont = 0;

            for (int i = 0; i <= listDados.Count() - getVizinhos(); i++)
            {
                if (i >= (listDados.Count() - getVizinhos()))
                {
                    for (int j = i; j < listDados.Count(); j++)
                    {
                        cont = listDados.Count() - j;

                        if ((listDados[j] >= getMin()) && (listDados[j] <= getMax()))
                            listDados[j] = listDados[j];
                        else
                        {
                            if (j == listDados.Count() - cont && listDados.Count() >= 2)
                            {
                                if (j < listDados.Count() - 1)
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j + 1]) / ((getVizinhos() * 2) - 1));
                                else
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2]) / getVizinhos());
                            }
                        }
                    }
                }
                else if (i >= getVizinhos())
                {
                    if ((listDados[i] >= getMin()) && (listDados[i] <= getMax()))
                        listDados[i] = listDados[i];
                    else
                        listDados[i] = ((listDados[i - 2] + listDados[i - 1] + listDados[i + 1] + listDados[i + 2]) / (getVizinhos() * 2));
                }
                else
                {
                    if ((listDados[i] >= getMin()) && (listDados[i] <= getMax()))
                        listDados[i] = listDados[i];
                    else
                    {
                        if (listDados.Count() > 4)
                        {
                            if (i == 0)
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2]) / getVizinhos());
                            else if (i == 1)
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i - 1]) / ((getVizinhos() * 2) - 1));
                            else
                                listDados[i] = getMin();
                        }
                    }
                }
            }

            return listDados;
        }

        public List<Double> TresVizinhos(List<Double> listDados)
        {
            for (int i = 0; i <= listDados.Count() - getVizinhos(); i++)
            {
                if (i >= (listDados.Count() - getVizinhos()))
                {
                    for (int j = i; j < listDados.Count(); j++)
                    {
                        cont = listDados.Count() - j;
                        if ((listDados[j] >= getMin()) && (listDados[j] <= getMax()))
                            listDados[j] = listDados[j];
                        else
                        {
                            if (j == listDados.Count() - cont && listDados.Count() >= 3)
                            {
                                if (j < listDados.Count() - 2)
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3] + listDados[j + 2] + listDados[j + 1]) / ((getVizinhos() * 2) - 1));
                                else if (j < listDados.Count() - 1)
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3] + listDados[j + 1]) / ((getVizinhos() * 2) - 2));
                                else
                                    listDados[j] = ((listDados[j - 1] + listDados[j - 2] + listDados[j - 3]) / getVizinhos());
                            }
                        }
                    }
                }
                else if (i >= getVizinhos())
                {
                    if ((listDados[i] >= getMin()) && (listDados[i] <= getMax()))
                        listDados[i] = listDados[i];
                    else
                        listDados[i] = ((listDados[i - 3] + listDados[i - 2] + listDados[i - 1] + listDados[i + 1] + listDados[i + 2] + listDados[i + 3]) / (getVizinhos() * 2));
                }
                else
                {
                    if ((listDados[i] >= getMin()) && (listDados[i] <= getMax()))
                        listDados[i] = listDados[i];
                    else
                    {
                        if (listDados.Count() > 6)
                        {
                            if (i == 0)
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i + 3]) / getVizinhos());
                            else if (i == 1)
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i + 3] + listDados[i - 1]) / ((getVizinhos() * 2) - 2));
                            else if (i == 2)
                                listDados[i] = ((listDados[i + 1] + listDados[i + 2] + listDados[i + 3] + listDados[i - 1] + listDados[i - 2]) / ((getVizinhos() * 2) - 1));
                            else
                                listDados[i] = getMin();
                        }
                    }
                }
            }

            return listDados;

        }

        public void setVizinhos(int valor)
        {
            this.vizinhos = valor;
        }

        public int getVizinhos()
        {
            return this.vizinhos;
        }

        public void setMinMax(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public double getMax()
        {
            return this.max;
        }

        public double getMin()
        {
            return this.min;
        }
    }
}
