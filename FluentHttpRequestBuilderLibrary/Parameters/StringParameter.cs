using System.Net.Http;
using System.Text;
using FluentHttpRequestBuilderLibrary.Constants;

namespace FluentHttpRequestBuilderLibrary.Parameters
{
    public class StringParameter : IRequestParameter
    {
        public string Key { get; }
        
        public string Value { get; }

        public Encoding Encoding { get; }

        public string MediaType { get; }

        public ParameterType ParameterType => ParameterType.String;
        
        public StringParameter(string key,string value)
        {
            Key = key;
            Value = value;
            Encoding = Encoding.UTF8;
            MediaType = MediaTypes.ApplicationJson;
        }

        public StringParameter(string key,string value, Encoding encoding = null, string mediaType = null)
        {
            Key = key;
            Value = value;
            Encoding = encoding ?? Encoding.UTF8;
            MediaType = mediaType ?? MediaTypes.ApplicationJson;
        }


        public HttpContent ToHttpContent()
        {
            return new StringContent(Value, Encoding, MediaType);
        }


    }
}