using System;
namespace Utilities
{
    public class TypesafeEnumPattern : IEquatable<TypesafeEnumPattern>
    {
        public string Value { get; protected set; }

        protected TypesafeEnumPattern(string value)
        {
            Value = value;
        }

        public bool Equals(TypesafeEnumPattern other)
        {
            return other != null && other.Value == Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
