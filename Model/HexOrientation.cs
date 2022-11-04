using Graph.Utilities;

namespace Graph.Model
{
    public class HexOrientation : TypesafeEnumPattern
    {
        public static HexOrientation HorizontalOdd { get; } = new("horizontal-odd");
        public static HexOrientation HorizontalEven { get; } = new("horizontal-even");
        public static HexOrientation VerticalOdd { get; } = new("vertical-odd");
        public static HexOrientation VerticalEven { get; } = new("vertical-even");

        protected HexOrientation(string value) : base(value)
        {
        }
    }
}
