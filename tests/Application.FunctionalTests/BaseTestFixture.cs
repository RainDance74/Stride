using static Stride.Application.FunctionalTests.Testing;

namespace Stride.Application.FunctionalTests;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp() => await ResetState();
}
