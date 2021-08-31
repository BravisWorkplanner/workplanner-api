using API.V1.Features.Orders.Response;
using FunctionalTests.Base;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Orders
{
    [Collection(nameof(TestFixture))]
    public class GetTests
    {
        private readonly TestFixture _testFixture;

        public GetTests(TestFixture fixture) => _testFixture = fixture;

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
    }
}