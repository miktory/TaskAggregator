using TaskAggregator.Tests.Integration.Factories;
using Xunit;

namespace TaskAggregator.Tests.Integration;

public static class Fixtures
{
    public const string IntegrationTestsCollectionName = "Integration";

    [CollectionDefinition(Fixtures.IntegrationTestsCollectionName)]
    public sealed class IntegrationCollection : ICollectionFixture<ApiFactory>
    {
    }
}

