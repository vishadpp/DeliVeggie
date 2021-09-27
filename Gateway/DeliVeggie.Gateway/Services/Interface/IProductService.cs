using DeliVeggie.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliVeggie.Gateway.Services.Interface
{
    public interface IProductService
    {
        List<ProductDto> GetAllProducts();
        ProductDto GetProductDetails(string id);
    }
}
