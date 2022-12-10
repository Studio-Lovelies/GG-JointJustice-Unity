using NUnit.Framework;

namespace Tests.EditModeTests.Suites
{
    public class AssetNameTests
    {
        [Test]
        public void ConvertAssetNameTests()
        {
            Assert.AreEqual("ThingWithSnakeCase",new NarrativeScriptAssetName("thing_with_snake_case").ToString());
            Assert.AreEqual("ThingWithSpaces",new NarrativeScriptAssetName("thing with spaces").ToString());
            Assert.AreEqual("ThingWithSpacesAndUnderscores",new NarrativeScriptAssetName("thing with_spaces and_underscores").ToString());
            Assert.AreEqual("ThingWithSpacesAndUnderscoresAndSomeCapitals",new NarrativeScriptAssetName("thing with_spaces and_underscores and Some Capitals").ToString());
        }
    }
}