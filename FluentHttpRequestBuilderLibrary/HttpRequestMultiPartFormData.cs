using System.Collections.Generic;
using System.Net.Http;
using FluentHttpRequestBuilderLibrary.Parameters;

namespace FluentHttpRequestBuilderLibrary
{
    public class HttpRequestMultiPartFormData
    {
        private readonly List<IRequestParameter> _requestParameters;

        private MultipartFormDataContent _formData;

        internal readonly HttpRequestBuilder Builder;
        internal readonly HttpRequestFormUrlCollection UrlCollection;

        public HttpRequestMultiPartFormData(HttpRequestBuilder builder, HttpRequestFormUrlCollection urlCollection)
        {
            Builder = builder;
            UrlCollection = urlCollection;
            _formData = new MultipartFormDataContent();
            _requestParameters = new List<IRequestParameter>();
        }

        public HttpRequestMultiPartFormData AddStringParam(string key, string value)
        {
            _requestParameters.Add(new StringParameter(key, value));

            return this;
        }

        public HttpRequestMultiPartFormData AddFileParam(string path)
        {
            var stream = this.Builder.StreamProvider.GetFileStream(path);
            
            _requestParameters.Add(new StreamParameter(stream));

            return this;
        }

        public HttpRequestMultiPartFormData AddByteArrayParam(string key, byte[] bytes)
        {
            _requestParameters.Add(new ByteArrayParameter(key, bytes));

            return this;
        }

        public void ClearParams()
        {
            this._requestParameters.Clear();
            this.UrlCollection.ClearForm();
        }

        public IReadOnlyList<IRequestParameter> FormParams() => this._requestParameters;
    }
}