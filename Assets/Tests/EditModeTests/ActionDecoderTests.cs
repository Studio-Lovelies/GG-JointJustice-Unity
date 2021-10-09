using NUnit.Framework;

namespace Tests.EditModeTests
{
    public class ActionDecoderTests
    {
        [Test]
        public void RunInvalidCommand()
        {
            var decoder = new ActionDecoder();

            Assert.Throws(typeof(UnknownCommandException), () =>
            {
                decoder.OnNewActionLine("spujb");
            });
        }
    }
}