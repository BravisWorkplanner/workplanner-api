using API.V1.Features.Orders.Response;
using FunctionalTests.Base;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Orders
{
    [Collection(nameof(TestFixture))]
    public class ListTests
    {
        private readonly TestFixture _testFixture;

        public ListTests(TestFixture testFixture) => _testFixture = testFixture;

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