using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Credits.Renderables;

namespace Credits
{
    public class Generator
    {
        public static IEnumerable<IRenderable> GenerateFromMarkdown(string creditsMarkdown)
        {
            var paragraphs = creditsMarkdown.Split('\n').Select(paragraph => paragraph.Trim());
            var parsedParagraphs = new List<IRenderable>();
            foreach (var paragraph in paragraphs)
            {
                var lines = paragraph.Split(new[] { "<br />" }, System.StringSplitOptions.None);
                if (lines.Length > 1)
                {
                    var firstItem = new StyledString(lines.First());
                    var remainingItems = lines.Skip(1).Select(line => new StyledString(line)
                    {
                        style = firstItem.style
                    });
                    parsedParagraphs.AddRange(remainingItems.Prepend(firstItem));
                    continue;
                }

                parsedParagraphs.Add(GenerateRenderable(paragraph));
            }

            return parsedParagraphs;
        }

        private static IRenderable GenerateRenderable(string line)
        {
            const string imageRegexChar = "^!\\[.*\\]\\((.*)\\)$";
            var imageRegex = new Regex(imageRegexChar);
            var attempt = imageRegex.Match(line);
            if (attempt.Success)
            {
                return new Image(attempt.Groups[1].Captures[0].Value);
            }

            return new StyledString(line);
        }
    }
}
