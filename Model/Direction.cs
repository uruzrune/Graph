using Graph.Utilities;

namespace Graph.Model
{
    public class Direction : TypesafeEnumPattern
    {
        public static Direction North { get; } = new("north");
        public static Direction South { get; } = new("south");
        public static Direction East { get; } = new("east");
        public static Direction West { get; } = new("west");
        public static Direction Northeast { get; } = new("northeast");
        public static Direction Southeast { get; } = new("southeast");
        public static Direction Northwest { get; } = new("northwest");
        public static Direction Southwest { get; } = new("southwest");

        protected Direction(string value) : base(value)
        {
        }
    }
}
