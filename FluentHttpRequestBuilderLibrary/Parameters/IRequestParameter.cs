using System.Net.Http;
using FluentHttpRequestBuilderLibrary.Constants;

namespace FluentHttpRequestBuilderLibrary.Parameters
{
    public interface IRequestParameter
    {
        HttpContent ToHttpContent();
        
        string Key { get; }
        
        ParameterType ParameterType { get; }
    }
}