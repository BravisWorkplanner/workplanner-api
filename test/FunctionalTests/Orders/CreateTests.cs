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
                ObjectNumber = $"B-{_testFixture.NextCourseNumber()}",
                Address = Faker.Address.StreetAddress(),
                Description = Faker.Lorem.Sentence(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(3),
                InvoiceDate = DateTime.UtcNow.AddDays(5),
                OrderStatus = OrderStatus.OnGoing,
            };
            var httpClient = _testFixture.Factory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(createdOrder), Encoding.UTF8, "application/json");

            // act
            var response = await httpClient.PostAsync("api/v1/orders", httpContent);

            // assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_Should_Provide_Location_Header_For_Getting_Created_Resource()
        {
            // arrange
            var createdOrder = new OrderCreateRequest()
            {
                ObjectNumber = $"B-{_testFixture.NextCourseNumber()}",
                Address = Faker.Address.StreetAddress(),
                Description = Faker.Lorem.Sentence(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(3),
                InvoiceDate = DateTime.UtcNow.AddDays(5),
                OrderStatus = OrderStatus.OnGoing,
            };
            var httpClient = _testFixture.Factory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(createdOrder), Encoding.UTF8, "application/json");

            // act
            var response = await httpClient.PostAsync("api/v1/orders", httpContent);

            // assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);

            var order = await httpClient.GetAsync(response.Headers.Location);
            var asd = await order.Content.ReadAsStringAsync();
            var getOrder = JsonSerializer.Deserialize<OrderGetResult>(asd);
            Assert.NotNull(getOrder);

            Assert.Equal(getOrder.Address, createdOrder.Address);
            Assert.Equal(getOrder.Description, createdOrder.Description);
            Assert.Equal(getOrder.ObjectNumber, createdOrder.ObjectNumber);
            Assert.Equal(getOrder.StartDate, createdOrder.StartDate);
            Assert.Equal(getOrder.EndDate, createdOrder.EndDate);
            Assert.Equal(getOrder.InvoiceDate, createdOrder.InvoiceDate);
            Assert.Equal(getOrder.OrderStatus, createdOrder.OrderStatus);
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