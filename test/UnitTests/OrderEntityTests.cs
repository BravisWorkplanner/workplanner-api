using Domain.Entities;
using Xunit;

namespace UnitTests
{
    public class OrderEntityTests
    {
        [Theory]
        [InlineData(1, "B-001")]
        [InlineData(11, "B-011")]
        [InlineData(111, "B-111")]
        public void Order_ObjectId_Should_Contain_Id_With_Correct_Format(int id, string expected)
        {
            // arrange
            var order = new Order
            {
                Id = id,
            };

            // assert
            Assert.Equal(expected, order.ObjectNumber);
        }
    }
}
