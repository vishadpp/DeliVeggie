using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliVeggie.MicroService.Model
{
    public class Product
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DateTime EntryDate { get; set; }
        public Double Price { get; set; }
    }
}
