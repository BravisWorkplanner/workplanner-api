using API.V1.Features.Expenses.Requests;
using Xunit;

namespace UnitTests.ValidatorTests
{
    public class OrderExpenseCreateValidatorTests
    {
        [Fact]
        public void OrderExpenseCreateValidator_Should_Fail_When_OrderId_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest() {WorkerId = 1, Price = double.MaxValue};

            // act
            var result = sut.Validate(order);

            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderExpenseCreateValidator_Should_Fail_When_WorkerId_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest() {OrderId = 1, Price = double.MaxValue};

            // act
            var result = sut.Validate(order);

            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderExpenseCreateValidator_Should_Fail_When_Price_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest() {OrderId = 1, WorkerId = 2};

            // act
            var result = sut.Validate(order);

            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OrderExpenseCreateValidator_Should_Pass_When_WorkerId_OrderId_Price_Provided()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest() {OrderId = 1, WorkerId = 2, Price = double.MaxValue};

            // act
            var result = sut.Validate(order);

            // assert
            Assert.True(result.IsValid);
        }
    }

    public class OrderExpenseUpdateValidatorTests
    {
        [Fact]
        public void OrderExpenseUpdateValidator_Should_Fail_When_OrderId_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseUpdateValidator();
            var order = new OrderExpenseUpdateRequest
            {
                Price = double.MaxValue,
            };

            // act
            var result = sut.Validate(order);

            // assert
            Assert.False(result.IsValid);
        }
    }
}