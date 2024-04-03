

using System.Net;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit.Abstractions;

namespace Api.Tests.IntegrationTests
{
    public class TagControllerTests
    {
        private readonly ITestOutputHelper _output;
        private readonly APIWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public TagControllerTests(ITestOutputHelper output)
        {
             _output = output;
            _factory = new APIWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetTags_DifferentPageNumbers_ReturnsDifferentTags()
        {
            // Arrange
            var url1 = "/api/tag?PageNumber=1&PageSize=10";
            var url2 = "/api/tag?PageNumber=2&PageSize=10";

            // Act
            var response1 = await _client.GetAsync(url1);
            var response2 = await _client.GetAsync(url2);

            var response1Content = await response1.Content.ReadAsStringAsync();
            var response2Content = await response2.Content.ReadAsStringAsync();

            _output.WriteLine(response1Content);
            _output.WriteLine(response2Content);

            // Assert
            response1Content.Should().NotBeNull();
            response2Content.Should().NotBeNull();
            response1Content.Should().NotBeEquivalentTo(response2Content);
        }

        [Fact]
        public async Task GetTags_DifferentSortOrders_ReturnsDifferentOrder()
        {
            // Arrange
            var url1 = "/api/tag?SortBy=Name&OrderBy=asc";
            var url2 = "/api/tag?SortBy=Name&OrderBy=desc";

            // Act
            var response1 = await _client.GetAsync(url1);
            var response2 = await _client.GetAsync(url2);

            var response1Content = await response1.Content.ReadAsStringAsync();
            var response2Content = await response2.Content.ReadAsStringAsync();

            // Assert
            response1Content.Should().NotBeNull();
            response2Content.Should().NotBeNull();
            response1Content.Should().NotBeEquivalentTo(response2Content);
        }

        [Fact]
        public async Task GetTags_LargePageSize_ReturnsMax50Tags()
        {
            // Arrange
            var url = "/api/tag?PageSize=1000";

            // Act
            var response = await _client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            int idCount = Regex.Matches(responseContent, "\"id\"").Count;

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            idCount.Should().BeGreaterThan(0);
            idCount.Should().Be(50);
        }
    }
}