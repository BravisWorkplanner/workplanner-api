using API.V1.Features.Orders.Request;
using Xunit;

namespace UnitTests.ValidatorTests
{
    public class OrderUpdateValidatorTests
    {
        [Fact]
        public void OrderUpdateValidator_Should_Fail_When_Id_DefaultValue()
        {
            // arrange
            var sut = new OrderUpdateValidator();
            var order = new OrderUpdateRequest();

            // act
            var result = sut.Validate(order);

            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderUpdateValidator_Should_Pass_When_Id_IsProvided()
        {
            // arrange
            var sut = new OrderUpdateValidator();
            var order = new OrderUpdateRequest
            {
                OrderId = 5,
                Description = "Hello",
                Address = "Hello",
                CustomerName = "Customer 1",
                CustomerPhoneNumber = "0700123123",
            };

            // act
            var result = sut.Validate(order);

            // assert
            Assert.True(result.IsValid);
        }
    }
}