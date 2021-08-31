using FunctionalTests.Base;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.Orders
{
    [Collection(nameof(TestFixture))]
    public class DeleteTests
    {
        private readonly TestFixture _testFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public DeleteTests(TestFixture testFixture, ITestOutputHelper testOutputHelper)
        {
            _testFixture = testFixture;
            _testOutputHelper = testOutputHelper;
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

            var error = await response.Content.ReadAsStringAsync();
            _testOutputHelper.WriteLine(error);

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
    }
}