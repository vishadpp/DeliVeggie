using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DeliVeggie.MicroService.BusinessLogic;
using DeliVeggie.MicroService.ConnectionSettings;
using DeliVeggie.MicroService.DBContext.Implementation;
using DeliVeggie.MicroService.DBContext.Interface;
using DeliVeggie.MicroService.Model;
using EasyNetQ;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace DeliVeggie.MicroService.Test.Tests
{
    [TestClass]
    public class DeliVeggieLogicTest
    {
        private Mock<IOptions<MongoSettings>> _mockOptions;
        private Mock<IMongoDatabase> _mockDB;
        private Mock<IMongoClient> _mockClient;
        Mock<IMongoDBContext> _mockMongoContext;
        //DeliVeggieLogic _deliVeggieLogic;
        Mock<IMongoCollection<Product>> _mockProductCollection;
        Mock<IBus> _mockBus;

        public DeliVeggieLogicTest()
        {
            _mockOptions = new Mock<IOptions<MongoSettings>>();
            _mockDB = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();
            _mockProductCollection = new Mock<IMongoCollection<Product>>();
            _mockMongoContext = new Mock<IMongoDBContext>();
            _mockBus = new Mock<IBus>();
        }

        [TestMethod]
        public void MongoBookDBContext_Constructor_Success()
        {
            var settings = new MongoSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "DeliVeggie"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _mockClient.Setup(c => c
            .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
                .Returns(_mockDB.Object);

            //Act 
            var context = new MongoDBContext(_mockOptions.Object?.Value);

            //Assert 
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void MongoBookDBContext_GetCollection_Failure()
        {

            //Arrange
            var settings = new MongoSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "DeliVeggie"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);
            _mockClient.Setup(c => c
            .GetDatabase(_mockOptions.Object.Value.DatabaseName, null))
                .Returns(_mockDB.Object);

            //Act 
            var context = new MongoDBContext(_mockOptions.Object?.Value);
            var myCollection = context.GetCollection<Product>("");

            //Assert 
            Assert.IsNull(myCollection);

        }

        [TestMethod]
        public void MongoBookDBContext_GetCollection_Success()
        {
            //Arrange
            var settings = new MongoSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "DeliVeggie"
            };

            _mockOptions.Setup(s => s.Value).Returns(settings);

            _mockClient.Setup(c => c.GetDatabase(_mockOptions.Object.Value.DatabaseName, null)).Returns(_mockDB.Object);

            //Act 
            var context = new MongoDBContext(_mockOptions.Object?.Value);
            var myCollection = context.GetCollection<Product>("Products");

            //Assert 
            Assert.IsNotNull(myCollection);
        }

        [TestMethod]
        public async Task GetProductTest_Success()
        {
            var mockList = new List<Common.Dto.ProductDto>();
            mockList.Add(new Common.Dto.ProductDto
            {
                Id = "61502e6982a49a63f78ea68c",
                Name = "Uncle Ben's Rice",
                EntryDate = "2021-09-26",
                Price = 16.86
            });

            mockList.Add(new Common.Dto.ProductDto
            {
                Id = "61502e7a82a49a63f78ea691",
                Name = "A pile of potatoes",
                EntryDate = "2021-09-26",
                Price = 20.5
            });

            DeliVeggieLogic deliVeggieLogic = new DeliVeggieLogic(_mockMongoContext.Object, _mockBus.Object);
            var result = deliVeggieLogic.GetProduct(new Common.Dto.ProductDto());
            
            //Assert.AreEqual(2, result.Result.Count);
        }

        [TestMethod]
        public async Task GetProductTest_Failure()
        {
            var mockList = new List<Common.Dto.ProductDto>();
            mockList.Add(new Common.Dto.ProductDto
            {
                Id = "61502e6982a49a63f78ea68c",
                Name = "Uncle Ben's Rice",
                EntryDate = "2021-09-26",
                Price = 16.86
            });

            mockList.Add(new Common.Dto.ProductDto
            {
                Id = "61502e7a82a49a63f78ea691",
                Name = "A pile of potatoes",
                EntryDate = "2021-09-26",
                Price = 20.5
            });

            DeliVeggieLogic deliVeggieLogic = new DeliVeggieLogic(_mockMongoContext.Object, _mockBus.Object);
            var result = deliVeggieLogic.GetProduct(new Common.Dto.ProductDto());
            
            Assert.AreNotEqual(mockList, result);
        }

        [TestMethod]
        public void GetAllProductsTest_Success()
        {
            DeliVeggieLogic deliVeggieLogic = new DeliVeggieLogic(_mockMongoContext.Object, _mockBus.Object);
            var result = deliVeggieLogic.GetProduct(new Common.Dto.ProductDto());
        }

        [TestMethod]
        public void GetAllProductsTest_Failure()
        {
            var _productMockList = new List<Product>();
            DeliVeggieLogic deliVeggieLogic = new DeliVeggieLogic(_mockMongoContext.Object, _mockBus.Object);
            var result = deliVeggieLogic.GetProduct(new Common.Dto.ProductDto());
            Assert.AreNotEqual(_productMockList, result);
        }

        [TestMethod]
        public void GetProductDetailsTest_Success()
        {
            var id = "61502e6982a49a63f78ea68c";
            var objectId = new ObjectId(id);

            var _productMock = new Product
            {
                Id = new ObjectId("61502e6982a49a63f78ea68c"),
                Name = "Uncle Ben's Rice",
                EntryDate = Convert.ToDateTime("2021-09-26"),
                Price = 16.86
            };

            DeliVeggieLogic deliVeggieLogic = new DeliVeggieLogic(_mockMongoContext.Object, _mockBus.Object);
            var result = deliVeggieLogic.GetProductDetails(new Common.Dto.ProductDto { Id = "61502e6982a49a63f78ea68c" });

            //foreach (var product in result)
            //{
            //    Assert.IsNotNull(product);
            //    Assert.AreSame(product.Id, _productMock.Id);
            //    Assert.AreSame(product.Name, _productMock.Name);
            //    break;
            //}

        }

        [TestMethod]
        public void GetProductDetailsTest_Failure()
        {
            //var id = "61502e6982a49a63f78ea68c";
            //var objectId = new ObjectId(id);

            var _productMock = new Product
            {
                Id = new ObjectId("61502e6982a49a63f78ea687"),
                Name = "Test",
                EntryDate = Convert.ToDateTime("2021-09-26"),
                Price = 6.86
            };

            DeliVeggieLogic deliVeggieLogic = new DeliVeggieLogic(_mockMongoContext.Object, _mockBus.Object);
            var result = deliVeggieLogic.GetProductDetails(new Common.Dto.ProductDto { Id = "61502e6982a49a63f78ea68c" });

            Assert.AreNotEqual(_productMock, result);
        }
    }
}
