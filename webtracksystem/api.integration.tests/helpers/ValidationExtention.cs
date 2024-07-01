using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace Api.Integration.Tests.Extentions
{
    public static class ValidationDictionaryExtention
    {
        public static void ValidateMessage(this IDictionary<string, string[]> result, string property)
        {
            result.Should().ContainKey(property);
            result[property].Should().HaveCount(1);
            result[property].First().Should().NotBeNullOrWhiteSpace();
        }
    }
}