using System;
using API.V1.Features.Orders.Request;
using API.V1.Features.Orders.Response;
using FunctionalTests.Base;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Enums;
using Xunit;

namespace FunctionalTests.Orders
{
    [Collection(nameof(TestFixture))]
    public class CreateTests
    {
        private readonly TestFixture _testFixture;

        public CreateTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
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
    }
}