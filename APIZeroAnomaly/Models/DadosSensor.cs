using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.Models
{
    public class DadosSensor
    {
        public ObjectId _id { get; set; }
        public int IdSensor { get; set; }
        public double Valor { get; set; }
        public double ValorOriginal { get; set; }
        public DateTime Data { get; set; }
    }
}