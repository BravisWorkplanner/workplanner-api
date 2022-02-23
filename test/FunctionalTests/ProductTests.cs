using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.V1.Features.Products.Request;
using API.V1.Features.Products.Response;
using API.V1.Features.Workers.Request;
using API.V1.Features.Workers.Response;
using Domain.Entities;
using FunctionalTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FunctionalTests
{
    [Collection(nameof(TestFixture))]
    public class ProductTests
    {
        private readonly TestFixture _testFixture;

        public ProductTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
    public async Task CreateProduct_Should_Create_New_Product()
    {
        // arrange
        var createdWorker = new ProductCreateRequest()
        {
            Type = Faker.Lorem.GetFirstWord(),
            Description = string.Join(' ', Faker.Lorem.Words(6)),
        };

        var httpClient = _testFixture.Factory.CreateClient();
        var httpContent = new StringContent(JsonSerializer.Serialize(createdWorker), Encoding.UTF8, "application/json");

        // act
        var response = await httpClient.PostAsync("api/v1/products", httpContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var id = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStreamAsync());

        Assert.NotEqual(default(int), id);

        var product = await _testFixture.ExecuteDbContextAsync(async db => await db.Products.FindAsync(id));

        Assert.NotNull(product);
        Assert.Equal(id, product.Id);
    }

    [Fact]
    public async Task DeleteProduct_Should_Delete_Product()
    {
        // arrange
        var product = await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var product = new Product
                {
                    Type = Faker.Lorem.GetFirstWord(),
                    Description = string.Join(' ', Faker.Lorem.Words(6)),
                };

                db.Entry(product).State = EntityState.Added;
                await db.SaveChangesAsync();

                return product;
            });

        var httpClient = _testFixture.Factory.CreateClient();

        // act
        var response = await httpClient.DeleteAsync($"api/v1/products/{product.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var deletedProduct = await _testFixture.ExecuteDbContextAsync(async db => await db.Products.FindAsync(product.Id));

        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteProduct_Should_Return_NotFound_For_Non_Existing_Product()
    {
        var httpClient = _testFixture.Factory.CreateClient();

        // act
        var response = await httpClient.DeleteAsync($"api/v1/products/{int.MaxValue}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_Should_Update_Product()
    {
        var product = await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var product = new Product
                {
                    Type = Faker.Lorem.GetFirstWord(),
                    Description = string.Join(' ', Faker.Lorem.Words(6)),
                };

                db.Entry(product).State = EntityState.Added;
                await db.SaveChangesAsync();

                return product;
            });

        var updateProductRequest = new ProductUpdateRequest
        {
            ProductId = product.Id,
            Type = Faker.Lorem.GetFirstWord(),
            Description = string.Join(' ', Faker.Lorem.Words(6)),
        };

        var httpClient = _testFixture.Factory.CreateClient();

        var data = new StringContent(JsonSerializer.Serialize(updateProductRequest), Encoding.UTF8, "application/json");

        // act
        var response = await httpClient.PutAsync("api/v1/products", data);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedProduct = await _testFixture.ExecuteDbContextAsync(async db => await db.Products.FindAsync(product.Id));

        Assert.NotNull(updatedProduct);
        Assert.Equal(updateProductRequest.ProductId, updatedProduct.Id);
        Assert.Equal(updateProductRequest.Description, updatedProduct.Description);
        Assert.Equal(updateProductRequest.Type, updatedProduct.Type);
    }

    [Fact]
    public async Task UpdateProduct_Should_Return_NotFound_For_Non_Existing_Product()
    {
        var httpClient = _testFixture.Factory.CreateClient();

        var updateProductRequest = new ProductUpdateRequest
        {
            ProductId = int.MaxValue,
            Type = Faker.Lorem.GetFirstWord(),
            Description = string.Join(' ', Faker.Lorem.Words(6)),
        };

        var data = new StringContent(JsonSerializer.Serialize(updateProductRequest), Encoding.UTF8, "application/json");

        // act
        var response = await httpClient.PutAsync("api/v1/products", data);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ListAll_Should_Return_List_Of_All_Products()
    {
        // arrange
        await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var product1 = new Product
                {
                    Type = Faker.Lorem.GetFirstWord(),
                    Description = string.Join(' ', Faker.Lorem.Words(6)),
                };

                var product2 = new Product
                {
                    Type = Faker.Lorem.GetFirstWord(),
                    Description = string.Join(' ', Faker.Lorem.Words(6)),
                };

                db.Products.AddRange(product1, product2);
                await db.SaveChangesAsync();
            });

        // act
        var httpClient = _testFixture.Factory.CreateClient();
        var response = await httpClient.GetAsync("api/v1/products");

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var workerList = JsonSerializer.Deserialize<List<WorkerListResult>>(content);

        Assert.NotNull(workerList);
        Assert.NotEmpty(workerList);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public async Task ListAll_Should_Return_List_Of_Specified_PageSize(int pageSize)
    {
        // arrange
        await _testFixture.ExecuteDbContextAsync(
            async db =>
            {
                var products = Enumerable.Range(0, 5).Select(_ => new Product
                {
                    Type = Faker.Lorem.GetFirstWord(),
                    Description = string.Join(' ', Faker.Lorem.Words(6)),
                });

                db.Products.AddRange(products);
                await db.SaveChangesAsync();
            });

        // act
        var httpClient = _testFixture.Factory.CreateClient();
        var response = await httpClient.GetAsync($"api/v1/products?perPage={pageSize}&page=1");

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var productList = JsonSerializer.Deserialize<List<ProductListResult>>(content);

        Assert.NotNull(productList);
        Assert.Equal(productList.Count, pageSize);
    }
    }
}
