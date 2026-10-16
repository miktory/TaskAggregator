using System.Reflection;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnitV3;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace TaskAggregator.Tests.Arch.Api;

public class ApiDependencyTests
{
    private static readonly Assembly[] _otherLayerWhitelistedAssemblies = [AssemblyAccessor.SharedAssembly, AssemblyAccessor.ApplicationAssembly];

    [Fact]
    public void Api_ShouldNotUse_NonWhitelistedLayers()
    {
        IArchRule rule = Types()
            .That().ResideInAssembly(typeof(TaskAggregator.API.ApiAssemblyMarker).Assembly)
            .And().DoNotHaveFullName(typeof(TaskAggregator.API.Extensions.DependencyInjectionExtensions).FullName)
            .Should().NotDependOnAny(
                Types().That().DoNotResideInAssembly(typeof(TaskAggregator.API.ApiAssemblyMarker).Assembly, _otherLayerWhitelistedAssemblies));

        rule.Check(Fixtures.Architecture);

    }
}
