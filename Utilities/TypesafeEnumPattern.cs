namespace Graph.Utilities
{
    public class TypesafeEnumPattern : IEquatable<TypesafeEnumPattern>
    {
        public string Value { get; protected set; }

        protected TypesafeEnumPattern(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(TypesafeEnumPattern? other)
        {
            return other != null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TypesafeEnumPattern);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
