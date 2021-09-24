namespace FluentHttpRequestBuilderLibrary.Constants
{
    public class MediaType
    {
        public int Value { get; }
        
        public string Name { get; }

        public MediaType(int value, string name)
        {
            Value = value;
            Name = name;
        }
        
        internal static readonly MediaType ApplicationJson = new MediaType(1,"application/json");
        internal static readonly MediaType OctetStream = new MediaType(2,"application/octet-stream");
        
    }
}