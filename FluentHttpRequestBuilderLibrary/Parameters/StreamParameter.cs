using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using FluentHttpRequestBuilderLibrary.Constants;
using FluentHttpRequestBuilderLibrary.Services;

namespace FluentHttpRequestBuilderLibrary.Parameters
{
    public class StreamParameter : IRequestParameter
    {
        public string Key { get; }

        public StreamResult StreamResult { get; }

        public ParameterType ParameterType => ParameterType.Stream;

        public StreamParameter(StreamResult result, string key = null)
        {
            StreamResult = result;
            Key = key ?? StreamResult.FileName;
        }

        public HttpContent ToHttpContent()
        {
            return new StreamContent(StreamResult.Stream);
        }
    }
}