﻿using API.V1.Features.Orders.Request;
using Xunit;

namespace UnitTests.ValidatorTests
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
            var result = sut.Validate(order);

            // assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OrderCreateValidator_Should_Fail_When_ObjectNumber_Null()
        {
            // arrange
            var sut = new OrderCreateRequest.OrderCreateRequestValidator();
            var order = new OrderCreateRequest();

            // act
            var result = sut.Validate(order);

            // assert
            Assert.False(result.IsValid);
        }
    }
}