using System.Diagnostics;

namespace Credits.Renderables
{
    [DebuggerDisplay("[txt] h{style.heading}: '{text}'")]
    public class StyledString : IRenderable
    {
        public class Style
        {
            public int heading;
        }

        public StyledString(string rawString)
        {
            style = new Style();
            text = "";
            if (string.IsNullOrEmpty(rawString))
            {
                return;
            }

            var currentIndex = 0;
            while (rawString[currentIndex] == '#')
            {
                ++currentIndex;
            }
            style.heading = currentIndex;
            text = rawString.Substring(style.heading).Trim();
        }

        public string text;
        public Style style;
        public void Render()
        {
            throw new System.NotImplementedException();
        }
    }
}