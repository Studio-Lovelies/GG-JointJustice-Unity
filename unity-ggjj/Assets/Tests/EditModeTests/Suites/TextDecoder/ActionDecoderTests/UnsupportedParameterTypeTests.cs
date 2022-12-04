using NUnit.Framework;
using TextDecoder.Parser;
using UnityEngine;

namespace Tests.EditModeTests.Suites.TextDecoderTests.ActionDecoderTests
{
    public class UnsupportedParameterTypeTests
    {
        private class ActionDecoderExtension : ActionDecoderBase
        {
            protected void NOT_SUPPORTED_PARAMETER(object unsupported)
            {
                // intentionally void
            }
        }

        [Test]
        public void ThrowsMissingParserException()
        {
            const string LINE_TO_PARSE = "&NOT_SUPPORTED_PARAMETER:cantBeParsed";

            Debug.Log("Attempting to parse:\n" + LINE_TO_PARSE);
            Assert.Throws<MissingParserException>(() => {
                new ActionDecoderExtension().InvokeMatchingMethod(LINE_TO_PARSE);
            });
        }
    }
}