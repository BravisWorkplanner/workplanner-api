using API.V1.Features.Orders.Request;
using API.V1.Features.Orders.Response;
using Domain.Enums;
using FunctionalTests.Base;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Orders
{
    [Collection(nameof(TestFixture))]
    public class UpdateTests
    {
        private readonly TestFixture _testFixture;

        public UpdateTests(TestFixture testFixture)
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
    }
}