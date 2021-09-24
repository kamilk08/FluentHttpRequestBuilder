using System.Net.Http;
using FluentHttpRequestBuilderLibrary.Constants;

namespace FluentHttpRequestBuilderLibrary.Parameters
{
    public class ByteArrayParameter : IRequestParameter
    {
        public string Key { get; }
        
        public byte[] Value { get; }

        public ParameterType ParameterType => ParameterType.ByteArray;
        
        public ByteArrayParameter(string key,byte[] value)
        {
            Key = key;
            Value = value;
        }
        
        public HttpContent ToHttpContent()
        {
            return new ByteArrayContent(Value);
        }


    }
}