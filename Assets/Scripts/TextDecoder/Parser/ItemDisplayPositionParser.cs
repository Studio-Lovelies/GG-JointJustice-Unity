using System;

namespace TextDecoder.Parser
{
    public class ItemDisplayPositionParser : Parser<ItemDisplayPosition>
    {
        public override string Parse(string input, out ItemDisplayPosition output)
        {
            if (!Enum.TryParse(input, out output))
            {
                return $"Cannot convert '{input}' into an {typeof(ItemDisplayPosition)} (valid values include: '{string.Join(", ", Enum.GetValues(typeof(ItemDisplayPosition)))}')";
            }
            return null;
        }
    }
}