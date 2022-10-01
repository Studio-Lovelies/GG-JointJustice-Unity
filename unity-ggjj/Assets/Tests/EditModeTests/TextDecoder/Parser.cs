using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TextDecoder.Parser;

namespace Tests.EditModeTests.TextDecoder
{
    public class Parser
    {
        private static IEnumerable<Type> AllAssetNameParser => typeof(Parser<>).Assembly.GetTypes().Where(type => type.BaseType is { IsGenericType: true } && type.BaseType.GetGenericTypeDefinition() == typeof(Parser<>));

        [Test]
        [TestCaseSource(nameof(AllAssetNameParser))]
        public void ThrowsIfStringIsEmpty(Type parser)
        {
            var parserConstructor = parser.GetConstructor(Type.EmptyTypes);
            var parserInstance = parserConstructor.Invoke(Array.Empty<object>());
            object[] parseMethodParameters = { "", null };
            string result = (string)parser.GetMethod("Parse")!.Invoke(parserInstance, parseMethodParameters);
            Assert.Greater(result.Length, 0);
        }

    }
}