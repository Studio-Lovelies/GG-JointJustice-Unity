using System;
using System.Linq;
using Credits.Renderables;
using NUnit.Framework;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private static void StyledStringEquals(StyledString a, StyledString b)
    {
        Assert.AreEqual(a.text, b.text);
        Assert.AreEqual(b.style.heading, b.style.heading);
    }
    private static void ImageEquals(Image a, Image b)
    {
        Assert.AreEqual(a.image, b.image);
    }

    [Test]
    public void CanParseHeadlines()
    {
        StyledStringEquals(new StyledString("Headline 1") { style = new StyledString.Style() { heading = 1 } }, (StyledString)Credits.Generator.GenerateFromMarkdown("# Headline 1").First());
        StyledStringEquals(new StyledString("Headline 1") { style = new StyledString.Style() { heading = 1 } }, (StyledString)Credits.Generator.GenerateFromMarkdown("#Headline 1").First());
    }

    [Test]
    public void LineBreakCreatesTwoMatchingItems()
    {
        var generatedItems = Credits.Generator.GenerateFromMarkdown("# Headline Part 1 <br /> Headline Part 2").Select(entry => (StyledString)entry).ToArray();
        StyledStringEquals(new StyledString("Headline Part 1") { style = new StyledString.Style() { heading = 1 } }, generatedItems[0]);
        StyledStringEquals(new StyledString("Headline Part 2") { style = new StyledString.Style() { heading = 1 } }, generatedItems[1]);
    }

    [Test]
    public void CanParseRegularText()
    {
        StyledStringEquals(new StyledString("Cool text #1") { style = new StyledString.Style() { heading = 0 } }, (StyledString)Credits.Generator.GenerateFromMarkdown("Cool text #1").First());
        StyledStringEquals(new StyledString("Cool text") { style = new StyledString.Style() { heading = 0 } }, (StyledString)Credits.Generator.GenerateFromMarkdown("Cool text").First());
        StyledStringEquals(new StyledString("") { style = new StyledString.Style() { heading = 0 } }, (StyledString)Credits.Generator.GenerateFromMarkdown("").First());
    }

    [Test]
    public void CanParseImage()
    {
        ImageEquals(new Image("TestSprite"), (Image)Credits.Generator.GenerateFromMarkdown("![image text](TestSprite)").First());
        StyledStringEquals(new StyledString("Not an image: ![image text](path)") { style = new StyledString.Style() { heading = 0 } }, (StyledString)Credits.Generator.GenerateFromMarkdown("Not an image: ![image text](path)").First());
    }


    [Test]
    public void ThrowsIfImageNotFound()
    {
        const string notFoundFilePath = "NotFound";
        var exception = Assert.Throws<NullReferenceException>(() => {
            Credits.Generator.GenerateFromMarkdown($"![image text]({notFoundFilePath})");
        });
        StringAssert.Contains(notFoundFilePath, exception.Message);
    }

}
