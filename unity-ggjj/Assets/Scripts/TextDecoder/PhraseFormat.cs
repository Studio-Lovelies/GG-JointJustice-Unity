using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhraseFormatString
{
    private readonly string _input;
    public PhraseFormatString(string input)
    {
        _input = input;
    }

    public string Resolve(params string[] options)
    {
        // ReSharper disable once CoVariantArrayConversion - In-game options will only consist of strings
        return string.Format(_input, options);
    }
}

public class PhraseFormatOptions : IEnumerable<PhraseFormatOption>
{
    private readonly PhraseFormatOption[] _options;

    public PhraseFormatOptions(PhraseFormatOption[] options)
    {
        _options = options;
    }

    public IEnumerator<PhraseFormatOption> GetEnumerator()
    {
        return _options.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class PhraseFormatOption
{
    public string Text { get; }

    public int Points { get; }

    public PhraseFormatOption(string text, int points)
    {
        Text = text;
        Points = points;
    }
}
