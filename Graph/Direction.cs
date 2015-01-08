using Utilities;

namespace Graph
{
    public class Direction : TypesafeEnumPattern
    {
        public static Direction North = new Direction("north");
        public static Direction South = new Direction("south");
        public static Direction East = new Direction("east");
        public static Direction West = new Direction("west");
        public static Direction Northeast = new Direction("northeast");
        public static Direction Southeast = new Direction("southeast");
        public static Direction Northwest = new Direction("northwest");
        public static Direction Southwest = new Direction("southwest");

        protected Direction(string value) : base(value)
        {
        }
    }
}
