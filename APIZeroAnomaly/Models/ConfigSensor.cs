using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.Models
{
    public class ConfigSensor
    {
        public ObjectId _id { get; set; }
        public int IdSensor { get; set; }
        public string Descricao { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public string UnidadeMedida { get; set; }
        public string Metodo { get; set; }
        public string VizinhosPadrao { get; set; }
        public DateTime Data { get; set; }
    }
}