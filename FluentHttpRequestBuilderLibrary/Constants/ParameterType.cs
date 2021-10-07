using System;

namespace FluentHttpRequestBuilderLibrary.Constants
{
    public class ParameterType : IEquatable<ParameterType>
    {
        public int Value { get; }

        public string Name { get; }

        public ParameterType(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public static readonly ParameterType ByteArray = new ParameterType(1, "ByteArray");
        public static readonly ParameterType Stream = new ParameterType(2, "Stream");
        public static readonly ParameterType String = new ParameterType(3, "String");

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (this.GetType() != obj.GetType()) return false;

            return this.Equals((ParameterType) obj);
        }

        public bool Equals(ParameterType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }

        public static bool operator ==(ParameterType firstObject, ParameterType secondObject)
        {
            if (ReferenceEquals(firstObject, null) && ReferenceEquals(secondObject, null))
                return true;

            if (ReferenceEquals(firstObject, null) || ReferenceEquals(secondObject, null))
                return false;

            return firstObject.Equals(secondObject);
        }

        public static bool operator !=(ParameterType firstObject, ParameterType secondObject)
        {
            return !(firstObject == secondObject);
        }
    }
}