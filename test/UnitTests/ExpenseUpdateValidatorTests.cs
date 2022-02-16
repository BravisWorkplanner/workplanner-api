using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.V1.Features.Expenses.Requests;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTests
{
    public class ExpenseUpdateValidatorTests
    {
        [Fact]
        public void ExpenseUpdateValidator_Should_Fail_When_OrderId_DefaultValue()
        {
            // arrange
            var sut = new OrderExpenseUpdateValidator();
            var expense = new ExpenseUpdateRequest
            {
                Price = double.MaxValue,
            };

            // act
            var result = sut.TestValidate(expense);

            // assert
            result.ShouldHaveValidationErrorFor(o => o.ProductId);
            result.ShouldHaveValidationErrorFor(o => o.Description);
            result.ShouldHaveValidationErrorFor(o => o.ExpenseId);
        }
    }
}