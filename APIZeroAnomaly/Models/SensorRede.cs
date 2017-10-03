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
        public int idRede { get; set; }
        public int idSensor { get; set; }
        public DateTime data { get; set; }
    }
}