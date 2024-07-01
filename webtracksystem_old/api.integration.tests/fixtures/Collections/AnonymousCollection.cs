using Xunit;

namespace Api.Integration.Tests
{
    [CollectionDefinition(nameof(AnonymousCollection))]
    public class AnonymousCollection : ICollectionFixture<CommonFixture>
    {
    }
}