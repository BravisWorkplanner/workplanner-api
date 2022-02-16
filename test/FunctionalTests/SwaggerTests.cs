using System.Net;
using System.Threading.Tasks;
using FunctionalTests.Base;
using Xunit;

namespace FunctionalTests
{
    [Collection(nameof(TestFixture))]
    public class SwaggerTests
    {
        private readonly TestFixture _testFixture;
        public SwaggerTests(TestFixture fixture) => _testFixture = fixture;

        [Fact]
        public async Task RenderSwaggerUI()
        {
            // arrange
            var client = _testFixture.Factory.CreateClient();

            // act
            var response = await client.GetAsync("/swagger");

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RenderSwaggerJson()
        {
            // arrange
            var client = _testFixture.Factory.CreateClient();

            // act
            var response = await client.GetAsync("/swagger/v1/swagger.json");

            // assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}