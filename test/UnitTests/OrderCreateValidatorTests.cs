using API.V1.Features.Orders.Request;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTests
{
    public class OrderCreateValidatorTests
    {
        [Fact]
        public void OrderCreateValidator_Should_Pass_Validation_For_Correct_Object()
        {
            // arrange
            var sut = new OrderCreateRequest.OrderCreateRequestValidator();
            var order = new OrderCreateRequest
            {
                Address = "B123",
                Description = "B123",
                CustomerPhoneNumber = "B123",
                CustomerName = "B123",
            };

            // act
            var result = sut.TestValidate(order);

            // assert
            result.ShouldNotHaveValidationErrorFor(o => o.Address);
            result.ShouldNotHaveValidationErrorFor(o => o.Description);
            result.ShouldNotHaveValidationErrorFor(o => o.CustomerPhoneNumber);
            result.ShouldNotHaveValidationErrorFor(o => o.CustomerName);
        }

        [Fact]
        public void OrderCreateValidator_Should_Fail_When_ObjectNumber_Null()
        {
            // arrange
            var sut = new OrderCreateRequest.OrderCreateRequestValidator();
            var order = new OrderCreateRequest();

            // act
            var result = sut.TestValidate(order);

            // assert
            result.ShouldHaveValidationErrorFor(o => o.Address);
            result.ShouldHaveValidationErrorFor(o => o.Description);
            result.ShouldHaveValidationErrorFor(o => o.CustomerPhoneNumber);
            result.ShouldHaveValidationErrorFor(o => o.CustomerName);
        }
    }
}