using System;

namespace FluentHttpRequestBuilderLibrary.Constants
{
    public class MediaType : IComparable,IEquatable<MediaType>
    {
        public int Value { get; }

        public string Name { get; }

        public MediaType(int value, string name)
        {
            Value = value;
            Name = name;
        }

        internal static readonly MediaType ApplicationJson = new MediaType(1, "application/json");
        internal static readonly MediaType OctetStream = new MediaType(2, "application/octet-stream");

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MediaType) obj);
        }

        public bool Equals(MediaType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }
        
        public int CompareTo(object obj)
        {
            var mediaType = obj as MediaType;
            if (mediaType == null) return 0;

            return this.Value.CompareTo(mediaType.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }

        public static bool operator ==(MediaType firstObject, MediaType secondObject)
        {
            if (ReferenceEquals(firstObject, null) && ReferenceEquals(secondObject, null))
                return true;

            if (ReferenceEquals(firstObject, null) || ReferenceEquals(secondObject, null))
                return false;

            return firstObject.Equals(secondObject);
        }

        public static bool operator !=(MediaType firstObject, MediaType secondObject)
        {
            return !(firstObject == secondObject);
        }


    }
}