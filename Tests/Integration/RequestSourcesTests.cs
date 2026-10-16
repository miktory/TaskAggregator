using System.Net;
using System.Net.Http.Json;
using TaskAggregator.API.Dtos.Requests;
using TaskAggregator.API.Dtos.Responses;
using TaskAggregator.Tests.Integration.Factories;
using Xunit;

namespace TaskAggregator.Tests.Integration;

[Collection(Fixtures.IntegrationTestsCollectionName)]
public class RequestSourcesTests
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _client;
    private readonly CancellationToken _ct;

    public RequestSourcesTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _ct = TestContext.Current.CancellationToken;
    }

    [Fact]
    public async Task CreateThenGetByIdFromLocation_WithValidData_ReturnsCreatedEntity()
    {
        var externalId = $"EXT-{Guid.NewGuid():N}";

        var create = await _client.PostAsJsonAsync(
            "/api/v1/request-sources",
            new CreateRequestSourceRequest(externalId), _ct);

        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        Assert.NotNull(create.Headers.Location);

        var id = long.Parse(create.Headers.Location!.Segments[^1]);

        var get = await _client.GetAsync($"/api/v1/request-sources/{id}", _ct);
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);

        var dto = await get.Content.ReadFromJsonAsync<RequestSourceResponse>(_ct);
        Assert.NotNull(dto);
        Assert.Equal(id, dto!.Id);
        Assert.Equal(externalId, dto.ExternalId);
    }

}
