using ArchUnitNET.Domain;
using ArchUnitNET.Loader;

namespace TaskAggregator.Tests.Arch;

public static class Fixtures
{
    public static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            AssemblyAccessor.ApiAssembly,
            AssemblyAccessor.ApplicationAssembly,
            AssemblyAccessor.DomainAssembly,
            AssemblyAccessor.InfrastructureAssembly,
            AssemblyAccessor.SharedAssembly,
            AssemblyAccessor.TestsAssembly
            )
        .Build();
}
