using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.Orders.Request;
using API.V1.Features.Orders.Response;
using FunctionalTests.Base;
using Xunit;

namespace FunctionalTests
{
    [Collection(nameof(TestFixture))]
    public class OrderTests
    {
        private readonly TestFixture _testFixture;

        public OrderTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact(Skip = "Currently a bug with updating")]
        public async Task UpdateOrder_Should_Update_Order_With_New_Values()
        {
            // arrange
            var createdOrder = _testFixture.CreateTestOrder();
            _ = await _testFixture.ExecuteDbContextAsync(
                async context =>
                {
                    _ = await context.AddAsync(createdOrder);

                    return await context.SaveChangesAsync();
                });

            var httpClient = _testFixture.Factory.CreateClient();
            var updateOrderRequest = CreateOrderUpdateRequest(createdOrder.Id);

            // act
            var response = await httpClient.PutAsJsonAsync("api/v1/orders", updateOrderRequest, CancellationToken.None);

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var orderId = await response.Content.ReadFromJsonAsync<int>();

            var order = await httpClient.GetFromJsonAsync<OrderGetResult>(
                $"api/v1/orders/{orderId}",
                CancellationToken.None);

            Assert.NotNull(order);
            Assert.Equal(updateOrderRequest.OrderId, order.OrderId);
            Assert.Equal(updateOrderRequest.Address, order.Address);
            Assert.Equal(updateOrderRequest.Description, order.Description);
            Assert.Equal(updateOrderRequest.StartDate, order.StartDate);
            Assert.Equal(updateOrderRequest.EndDate, order.EndDate);
            Assert.Equal(updateOrderRequest.InvoiceDate, order.InvoiceDate);
            Assert.Equal(updateOrderRequest.OrderStatus, order.OrderStatus);
        }

        [Fact]
        public async Task UpdateOrder_Should_Return_NotFound_For_NonExisting_Id()
        {
            // arrange
            var httpClient = _testFixture.Factory.CreateClient();
            var updateOrderRequest = CreateOrderUpdateRequest();

            // act
            var response = await httpClient.PutAsJsonAsync("api/v1/orders", updateOrderRequest, CancellationToken.None);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private static OrderUpdateRequest CreateOrderUpdateRequest(int id = int.MaxValue)
        {
            return new()
            {
                OrderId = id,
                Address = Faker.Address.StreetAddress(),
                Description = Faker.Lorem.Sentence(),
                CustomerName = Faker.Company.Name(),
                CustomerPhoneNumber = Faker.Phone.Number(),
            };
        }

        [Fact]
        public async Task GetOrderById_Should_Return_Correct_Order_For_Id()
        {
            // arrange
            var createdOrder = _testFixture.CreateTestOrder();
            _ = await _testFixture.ExecuteDbContextAsync(
                async context =>
                {
                    _ = await context.AddAsync(createdOrder);

                    return await context.SaveChangesAsync();
                });

            // act
            var httpClient = _testFixture.Factory.CreateClient();
            var response = await httpClient.GetAsync($"api/v1/orders/{createdOrder.Id}");

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var orderResult = JsonSerializer.Deserialize<OrderGetResult>(content);

            Assert.NotNull(orderResult);
            Assert.Equal(createdOrder.Id, orderResult.OrderId);
            Assert.Equal(createdOrder.ObjectNumber, orderResult.ObjectNumber);
            Assert.Equal(createdOrder.Address, orderResult.Address);
            Assert.Equal(createdOrder.Description, orderResult.Description);
            Assert.Equal(createdOrder.StartDate, orderResult.StartDate);
            Assert.Equal(createdOrder.EndDate, orderResult.EndDate);
            Assert.Equal(createdOrder.InvoiceDate, orderResult.InvoiceDate);
            Assert.Equal(createdOrder.OrderStatus, orderResult.OrderStatus);
        }

        [Fact]
        public async Task GetOrderById_Should_Return_NotFound_For_Non_Existing_Id()
        {
            // arrange
            var httpClient = _testFixture.Factory.CreateClient();

            // act
            var response = await httpClient.GetAsync("api/v1/orders/999999");

            // assert
            Assert.True(!response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_Should_Create_A_New_Order()
        {
            // arrange
            var createdOrder = new OrderCreateRequest()
            {
                CustomerPhoneNumber = Faker.Phone.Number(),
                Address = Faker.Address.StreetAddress(),
                Description = Faker.Lorem.Sentence(),
                CustomerName = Faker.Company.Name(),
            };

            var httpClient = _testFixture.Factory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(createdOrder), Encoding.UTF8, "application/json");

            // act
            var response = await httpClient.PostAsync("api/v1/orders", httpContent);

            var id = JsonSerializer.Deserialize<int>(await response.Content.ReadAsStreamAsync());

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var getResponse = await httpClient.GetAsync($"api/v1/orders/{id}");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_Should_Return_BadRequest_When_ObjectNumber_Is_Missing()
        {
            // arrange
            var createdOrder = new OrderCreateRequest();
            var httpClient = _testFixture.Factory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(createdOrder), Encoding.UTF8, "application/json");

            // act
            var response = await httpClient.PostAsync("api/v1/orders", httpContent);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_Should_Delete_Order()
        {
            // arrange
            var createdOrder = _testFixture.CreateTestOrder();
            _ = await _testFixture.ExecuteDbContextAsync(
                async context =>
                {
                    _ = await context.AddAsync(createdOrder);

                    return await context.SaveChangesAsync();
                });

            var httpClient = _testFixture.Factory.CreateClient();

            // act
            var response = await httpClient.DeleteAsync($"api/v1/orders/{createdOrder.Id}", CancellationToken.None);

            // assert 
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(createdOrder.Id, content);
        }

        [Fact]
        public async Task DeleteOrder_Should_Return_Not_Found_For_Non_Existing_Id()
        {
            // arrange
            var httpClient = _testFixture.Factory.CreateClient();

            // act
            var response = await httpClient.DeleteAsync("api/v1/orders/9999999", CancellationToken.None);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ListAll_Should_Return_List_Of_All_Orders()
        {
            // arrange
            var createdOrder1 = _testFixture.CreateTestOrder();
            var createdOrder2 = _testFixture.CreateTestOrder();
            _ = await _testFixture.ExecuteDbContextAsync(
                async context =>
                {
                    await context.AddRangeAsync(createdOrder1, createdOrder2);

                    return await context.SaveChangesAsync();
                });

            // act
            var httpClient = _testFixture.Factory.CreateClient();
            var response = await httpClient.GetAsync("api/v1/orders");

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var orderList = JsonSerializer.Deserialize<List<OrderGetResult>>(content);

            Assert.NotNull(orderList);
            Assert.NotEmpty(orderList);
        }

        [Fact]
        public async Task ListAll_Should_Return_List_With_Specified_Number_Of_Values()
        {
            // arrange
            var createdOrder1 = _testFixture.CreateTestOrder();
            var createdOrder2 = _testFixture.CreateTestOrder();
            _ = await _testFixture.ExecuteDbContextAsync(
                async context =>
                {
                    await context.AddRangeAsync(createdOrder1, createdOrder2);

                    return await context.SaveChangesAsync();
                });

            // act
            var httpClient = _testFixture.Factory.CreateClient();
            var response = await httpClient.GetAsync("api/v1/orders?perPage=1&page=1");

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var orderList = JsonSerializer.Deserialize<List<OrderGetResult>>(content);

            Assert.NotNull(orderList);
            Assert.Single(orderList);
        }
    }
}