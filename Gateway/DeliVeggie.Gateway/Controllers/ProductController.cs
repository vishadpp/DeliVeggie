using DeliVeggie.Common.Dto;
using DeliVeggie.Gateway.Services.Interface;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliVeggie.Gateway.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IServiceProvider _service;
        private IProductService _productService => _service.GetService(typeof(IProductService)) as IProductService;
        public ProductController(IServiceProvider service)
        {
            _service = service;
        }

        [HttpGet("GetAllProducts")]
        public List<ProductDto> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }

        [HttpGet("GetProductDetails")]
        public ProductDto GetProductDetails(string id)
        {
            return _productService.GetProductDetails(id);
        }

    }
}
