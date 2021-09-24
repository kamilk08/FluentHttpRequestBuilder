namespace FluentHttpRequestBuilderLibrary.Constants
{
    public class ParameterType
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
    }
}