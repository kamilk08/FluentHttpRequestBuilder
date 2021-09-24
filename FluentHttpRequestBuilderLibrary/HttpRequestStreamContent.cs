using System;
using System.IO;
using System.Net.Http;
using FluentHttpRequestBuilderLibrary.Services;

namespace FluentHttpRequestBuilderLibrary
{
    public class HttpRequestStreamContent : IDisposable
    {
        private IStreamProvider _streamProvider;

        internal readonly HttpRequestBuilder Builder;
        
        public StreamContent Value { get; internal set; }

        public string FileName { get; internal set; }
        
        public HttpRequestStreamContent(IStreamProvider streamProvider,HttpRequestBuilder builder)
        {
            Builder = builder;
            _streamProvider = streamProvider;
        }

        public void Dispose()
        {
            this.Value?.Dispose();
        }
    }
}