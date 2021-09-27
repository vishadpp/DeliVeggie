using DeliVeggie.Common.Dto;
using DeliVeggie.MicroService.Model;
using DeliVeggie.MicroService.DBContext.Interface;
using EasyNetQ;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeliVeggie.MicroService.BusinessLogic
{
    public class DeliVeggieLogic
    {
        private readonly IMongoDBContext _context;
        protected IMongoCollection<Product> _productCollection;
        protected IMongoCollection<PriceReduction> _priceReductionCollection;
        private readonly IBus _bus;

        public DeliVeggieLogic(IMongoDBContext context, IBus bus)
        {
            _context = context;
            _bus = bus;
            _productCollection = _context.GetCollection<Product>("Products");
            _priceReductionCollection = _context.GetCollection<PriceReduction>("PriceReductions");
        }

        public async Task<List<ProductDto>> GetProduct(ProductDto product)
        {
            var result = new List<ProductDto>();

            if (string.IsNullOrEmpty(product.Id))
                result = await this.GetAllProducts();
            else
                result = await this.GetProductDetails(product);

            return result;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            //Get all products
            var result = new List<ProductDto>();
            FilterDefinition<Product> filter = Builders<Product>.Filter.Empty;
            var productList = await _productCollection.FindAsync(filter).Result.ToListAsync();

            foreach (var item in productList)
            {
                result.Add(new ProductDto
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    EntryDate = item.EntryDate.ToLocalTime().ToString("dd MMMM, yyyy"),
                    Price = item.Price
                });
            }
            return result;
        }

        public async Task<List<ProductDto>> GetProductDetails(ProductDto product)
        {
            var result = new List<ProductDto>();

            if (product != null && ObjectId.TryParse(product.Id, out _))
            {
                //Get the product details based on the id
                FilterDefinition<Product> productFilter = Builders<Product>.Filter.Eq("_id", ObjectId.Parse(product.Id));
                var productResult = await _productCollection.FindAsync(productFilter).Result.FirstOrDefaultAsync();

                //Get the price reduction details based on the day of week
                int dayOfWeek = ((int)DateTime.Now.DayOfWeek) + 1;
                FilterDefinition<PriceReduction> priceReductionFilter = Builders<PriceReduction>.Filter.Eq(x => x.DayOfWeek, dayOfWeek);
                var priceReductionResult = await _priceReductionCollection.FindAsync(priceReductionFilter).Result.FirstOrDefaultAsync();

                //Price reduction logic
                if (productResult != null)
                {
                    result.Add(new ProductDto
                    {

                        Id = productResult.Id.ToString(),
                        Name = productResult.Name,
                        EntryDate = productResult.EntryDate.ToLocalTime().ToString("dd MMMM, yyyy"),
                        Price = priceReductionResult != null ? productResult.Price - (productResult.Price * priceReductionResult.Reduction) : productResult.Price
                    });
                }
            }

            return result;
        }

    }
}
