using Utilities;

namespace Graph
{
    public class HexOrientation : TypesafeEnumPattern
    {
        public static HexOrientation HorizontalOdd = new HexOrientation("horizontal-odd");
        public static HexOrientation HorizontalEven = new HexOrientation("horizontal-even");
        public static HexOrientation VerticalOdd = new HexOrientation("vertical-odd");
        public static HexOrientation VerticalEven = new HexOrientation("vertical-even");

        protected HexOrientation(string value) : base(value)
        {
        }
    }
}
