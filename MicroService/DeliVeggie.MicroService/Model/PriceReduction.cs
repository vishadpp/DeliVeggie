using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliVeggie.MicroService.Model
{
    public class PriceReduction
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int DayOfWeek { get; set; }
        public Double Reduction { get; set; }
    }
}
