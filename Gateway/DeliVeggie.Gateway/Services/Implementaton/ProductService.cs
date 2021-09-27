using DeliVeggie.Common.Dto;
using DeliVeggie.Gateway.Services.Interface;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliVeggie.Gateway.Services.Implementaton
{
    public class ProductService : IProductService
    {
        private readonly IBus _bus;

        public ProductService(IBus bus)
        {
            _bus = bus;
        }

        public List<ProductDto> GetAllProducts()
        {
            var result = new List<ProductDto>();
            result = _bus.Request<ProductDto, List<ProductDto>>(new ProductDto());
            return result;
        }

        public ProductDto GetProductDetails(string id)
        {
            var result = new List<ProductDto>();
            result = _bus.Request<ProductDto, List<ProductDto>>(new ProductDto { Id = id});
            return result.FirstOrDefault();
        }
    }
}
