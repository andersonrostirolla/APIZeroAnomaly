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
        public string idSensor { get; set; }
        public string valor { get; set; }
        public DateTime data { get; set; }
    }
}