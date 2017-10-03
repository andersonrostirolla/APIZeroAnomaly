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
        public int idSensor { get; set; }
        public double valor { get; set; }
        public double valorOriginal { get; set; }
        public DateTime data { get; set; }
    }
}