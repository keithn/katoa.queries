using System;
using System.Reflection;
using Xunit;

namespace Katoa.Queries.Tests
{
    public class QueryTests
    {
        [Fact]
        public void TestLoadingExampleQuery()
        {
            var example = Query.For("Example");
            Assert.Equal("example!\r\n", example);
        }

        [Fact]
        void TestLoadingQueryWithIgnoredSection()
        {
            var withIgnored = Query.For("WithIgnored");
            Assert.Equal("this part loads!\r\n", withIgnored);
        }

        [Fact]
        void TestThrowIfQueryDoesNotExist()
        {
            Assert.Throws(typeof(Exception), () => Query.For("IDoNotExist"));
        }
    }
}