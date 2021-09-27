using System;
using System.Collections.Generic;
using System.Text;

namespace DeliVeggie.MicroService.ConnectionSettings
{
    public interface IMongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
