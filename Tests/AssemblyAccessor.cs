using System.Reflection;

namespace TaskAggregator.Tests;

public static class AssemblyAccessor
{
    public static Assembly ApiAssembly => typeof(TaskAggregator.API.ApiAssemblyMarker).Assembly;
    public static Assembly DomainAssembly => typeof(TaskAggregator.Domain.DomainAssemblyMarker).Assembly;
    public static Assembly TestsAssembly => typeof(TaskAggregator.Tests.TestsAssemblyMarker).Assembly;
    public static Assembly InfrastructureAssembly => typeof(TaskAggregator.Infrastructure.InfrastructureAssemblyMarker).Assembly;
    public static Assembly ApplicationAssembly => typeof(TaskAggregator.Application.ApplicationAssemblyMarker).Assembly;
    public static Assembly SharedAssembly => typeof(TaskAggregator.Shared.SharedAssemblyMarker).Assembly;
}
