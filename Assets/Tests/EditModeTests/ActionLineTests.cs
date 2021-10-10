using NUnit.Framework;

public class ActionLineTests
{
    [Test]
    public void FailureToParseFloatThrows()
    {
        var line = new ActionLine("this is not a number");
        Assert.Throws(typeof(UnableToParseException), () =>
        {
            line.NextFloat("radius of the sun");
        });
    }

    [Test]
    public void FailureToParseFloatLogsGoodError()
    {
        var line = new ActionLine("&rise:moon ");

        try
        {
            line.NextFloat("radius of the sun");
        }
        catch (UnableToParseException e)
        {
            Assert.AreEqual("Failed to parse radius of the sun: cannot parse `moon` as decimal value", e.Message);
        }
    }

    [Test]
    public void NotEnoughParametersThrows()
    {
        var line = new ActionLine("action:one,two,three");

        Assert.DoesNotThrow(() =>
        {
            line.NextString("first");
            line.NextString("second");
            line.NextString("third");
        });

        Assert.Throws(typeof(NotEnoughParametersException), () =>
        {
            line.NextString("fourth");
        });
    }

    [Test]
    public void CanParseEnumValues()
    {
        var line = new ActionLine("action:Left,Right,Middle ");

        var left = line.NextEnumValue<ItemDisplayPosition>("display pos");
        var right = line.NextEnumValue<ItemDisplayPosition>("display pos");
        var middle = line.NextEnumValue<ItemDisplayPosition>("display pos");

        Assert.AreEqual(ItemDisplayPosition.Left, left);
        Assert.AreEqual(ItemDisplayPosition.Right, right);
        Assert.AreEqual(ItemDisplayPosition.Middle, middle);
    }

    [Test]
    public void EnumParseValueThrows()
    {
        var line = new ActionLine("action:Roight ");


        Assert.Throws(typeof(UnableToParseException), () =>
         {
             line.NextEnumValue<ItemDisplayPosition>("display pos");
         });

    }

    [Test]
    public void EnumParseValueGivesGoodErrorMessage()
    {
        var line = new ActionLine("action:Roight ");

        try
        {
            line.NextEnumValue<ItemDisplayPosition>("display position");
        }
        catch (UnableToParseException e)
        {
            Assert.AreEqual("Failed to parse display position: cannot parse `Roight` as Left/Right/Middle", e.Message);
        }
    }
}
