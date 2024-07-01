using Xunit;

namespace Api.Integration.Tests
{
    [CollectionDefinition(nameof(ExistingUserCollection))]
    public class ExistingUserCollection : ICollectionFixture<ExistingUserFixture>
    {
    }
}