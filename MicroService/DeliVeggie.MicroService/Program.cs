using DeliVeggie.Common.Dto;
using DeliVeggie.MicroService.ConnectionSettings;
using DeliVeggie.MicroService.DBContext.Implementation;
using DeliVeggie.MicroService.DBContext.Interface;
using DeliVeggie.MicroService.BusinessLogic;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace DeliVeggie.MicroService
{
    public class Program
    {
        private readonly DeliVeggieLogic _deliVeggie;
        public static IConfigurationRoot Configuration;
        private static IConfiguration _iconfiguration;
        private readonly IBus _bus;

        public Program(DeliVeggieLogic deliVeggie, IBus bus)
        {
            _deliVeggie = deliVeggie;
            _bus = bus;
        }
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }

        public void Run()
        {
            _bus.Respond<ProductDto, List<ProductDto>>(request => _deliVeggie.GetProduct(request).Result);
            Console.WriteLine("Service started");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {

            var builder = new ConfigurationBuilder()
           .SetBasePath(Path.Combine(AppContext.BaseDirectory))
           .AddJsonFile("appsettings.json", optional: true);
            _iconfiguration = builder.Build();


            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<DeliVeggieLogic>();
                    services.AddScoped<IMongoDBContext, MongoDBContext>();

                    services.Configure<MongoSettings>(
                        _iconfiguration.GetSection(nameof(MongoSettings)));

                    services.AddSingleton<IMongoSettings>(sp =>
                    sp.GetRequiredService<IOptions<MongoSettings>>().Value);

                    RabbitMQSettings rabbitMQSettings = new RabbitMQSettings();
                    _iconfiguration.GetSection("RabbitMQSettings").Bind(rabbitMQSettings);

                    var bus = RabbitHutch.CreateBus(rabbitMQSettings.ConnectionString);
                    services.AddSingleton(bus);
                });
        }
        
    }
}
