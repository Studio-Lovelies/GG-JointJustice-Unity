using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TextDecoder.Parser
{
    public class PhraseFormatOptionsParser : Parser<PhraseFormatOptions>
    {
        private readonly Regex _optionGroupRegex = new Regex("(?'text'[^[]+)\\[(?'points'[^]]+)");
        public override string Parse(string input, out PhraseFormatOptions output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }
            
            var parseErrors = new List<string>();
            var optionString = input.Split("/");
            var options = new List<PhraseFormatOption>();
            foreach (var result in optionString)
            {
                var match = _optionGroupRegex.Match(result);
                if (!match.Success)
                {
                    parseErrors.Add($"[{result}]: Ensure the option has at least one character of text and a score inside square brakets");
                    continue;
                }
            
                if (!int.TryParse(match.Groups["points"].Value, out var points))
                {
                    parseErrors.Add($"[{result}]: Unable to parse points; make sure text between square brackets consists of only numbers, optionally prefixed by minus.");
                    continue;
                }

                options.Add(new PhraseFormatOption(match.Groups["text"].Value.Trim(), points));
            }

            if (parseErrors.Any())
            {
                output = null;
                return string.Join(" - ", parseErrors);
            }

            output = new PhraseFormatOptions(options.ToArray());
            return null;
        }
    }
    public class PhraseFormatStringParser : Parser<PhraseFormatString>
    {
        private readonly Regex _formatItemRegex = new("%(\\d+)%");
        public override string Parse(string input, out PhraseFormatString output)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                output = null;
                return $"String cannot be empty";
            }
            
            input = _formatItemRegex.Replace(input, "{$1}");
            Debug.Log(input);
            
            if (!input.Contains("{") || !input.Contains("}"))
            {
                output = null;
                return $"The placeholder text needs at least one location to substitute in";
            }

            output = new PhraseFormatString(input);
            return null;
        }
    }
}