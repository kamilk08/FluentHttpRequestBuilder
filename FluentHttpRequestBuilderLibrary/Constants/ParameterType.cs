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

        public static ParameterType ByteArray = new ParameterType(1, "ByteArray");
        public static ParameterType Stream = new ParameterType(2, "Stream");
        public static ParameterType String = new ParameterType(3, "String");
    }
}