using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIZeroAnomaly.Models
{
    public class SensorRede
    {
        public ObjectId _id { get; set; }
        public int IdRede { get; set; }
        public int IdSensor { get; set; }
        public DateTime Data { get; set; }
    }
}