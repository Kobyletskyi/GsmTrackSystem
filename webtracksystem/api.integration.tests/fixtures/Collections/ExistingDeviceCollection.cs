using Xunit;

namespace Api.Integration.Tests
{
    [CollectionDefinition(nameof(ExistingDeviceCollection))]
    public class ExistingDeviceCollection : ICollectionFixture<ExistingDeviceFixture>
    {
    }
}