using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliVeggie.MicroService.DBContext.Interface
{
    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
