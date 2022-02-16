using API.V1.Features.Expenses.Requests;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTests
{
    public class ExpenseCreateValidatorTests
    {
        [Fact]
        public void ExpenseCreateValidator_Should_Fail_When_OrderId_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest() { WorkerId = 1, Price = double.MaxValue };

            // act
            var result = sut.TestValidate(order);

            // assert
            result.ShouldHaveValidationErrorFor(o => o.Description);
            result.ShouldHaveValidationErrorFor(o => o.ProductId);
            result.ShouldHaveValidationErrorFor(o => o.OrderId);

            result.ShouldNotHaveValidationErrorFor(o => o.WorkerId);
            result.ShouldNotHaveValidationErrorFor(o => o.Price);
        }

        [Fact]
        public void ExpenseCreateValidator_Should_Fail_When_WorkerId_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest() { OrderId = 1, Price = double.MaxValue };

            // act
            var result = sut.TestValidate(order);

            // assert
            result.ShouldHaveValidationErrorFor(o => o.Description);
            result.ShouldHaveValidationErrorFor(o => o.ProductId);
            result.ShouldHaveValidationErrorFor(o => o.WorkerId);

            result.ShouldNotHaveValidationErrorFor(o => o.OrderId);
            result.ShouldNotHaveValidationErrorFor(o => o.Price);
        }

        [Fact]
        public void ExpenseCreateValidator_Should_Fail_When_Price_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest() { OrderId = 1, WorkerId = 2 };

            // act
            var result = sut.TestValidate(order);

            // assert
            result.ShouldHaveValidationErrorFor(o => o.Description);
            result.ShouldHaveValidationErrorFor(o => o.ProductId);
            result.ShouldHaveValidationErrorFor(o => o.Price);

            result.ShouldNotHaveValidationErrorFor(o => o.OrderId);
            result.ShouldNotHaveValidationErrorFor(o => o.WorkerId);
        }

        [Fact]
        public void ExpenseCreateValidator_Should_Pass_When_WorkerId_OrderId_Price_Provided()
        {
            // arrange
            var sut = new OrderExpenseCreateValidator();
            var order = new OrderExpenseCreateRequest
            {
                OrderId = 1,
                WorkerId = 2,
                Price = double.MaxValue,
                ProductId = 3,
                Description = "Hello",
            };

            // act
            var result = sut.TestValidate(order);

            // assert
            result.ShouldNotHaveValidationErrorFor(o => o.Description);
            result.ShouldNotHaveValidationErrorFor(o => o.ProductId);
            result.ShouldNotHaveValidationErrorFor(o => o.Price);
            result.ShouldNotHaveValidationErrorFor(o => o.OrderId);
            result.ShouldNotHaveValidationErrorFor(o => o.WorkerId);
        }
    }
}