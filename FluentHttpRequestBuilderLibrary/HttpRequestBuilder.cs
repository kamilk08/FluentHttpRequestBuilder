using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using FluentHttpRequestBuilderLibrary.Services;
using Newtonsoft.Json;

namespace FluentHttpRequestBuilderLibrary
{
    public class HttpRequestBuilder
    {
        internal HttpRequestMessage Request;
        
        internal readonly IStreamProvider StreamProvider;
        internal HttpRequestFormUrlCollection FormUrlCollection;
        internal HttpRequestStreamContent StreamContent;
        internal HttpRequestMultiPartFormData MultiPartFormData;

        public HttpRequestBuilder()
        {
            StreamProvider = new FileStreamProvider();
        }

        public HttpRequestBuilder(IStreamProvider streamProvider)
        {
            StreamProvider = streamProvider ?? throw new ArgumentNullException(nameof(streamProvider), "Stream provider cannot be null.");
        }

        public HttpRequestBuilder InitializeRequest()
        {
            Request = new HttpRequestMessage();
            FormUrlCollection = new HttpRequestFormUrlCollection(this);
            StreamContent = new HttpRequestStreamContent(StreamProvider, this);
            MultiPartFormData = new HttpRequestMultiPartFormData(this, FormUrlCollection);

            return this;
        }

        public HttpRequestBuilder WithMethod(HttpMethod method)
        {
            Request.Method = method;

            return this;
        }

        public HttpRequestBuilder WithUri(string uri)
        {
            Request.RequestUri = new Uri(uri);
            return this;
        }

        public HttpRequestBuilder WithContent(HttpContent content)
        {
            Request.Content = content;

            return this;
        }

        public HttpRequestBuilder WithStringContent<T>(T value)
        {
            Request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            return this;
        }

        public HttpRequestBuilder AddHeader(string key, string value)
        {
            Request.Headers.Add(key, value);

            return this;
        }

        public HttpRequestBuilder AddMultipleHeaders(List<KeyValuePair<string, string>> values)
        {
            foreach (var pair in values)
            {
                Request.Headers.Add(pair.Key, pair.Value);
            }

            return this;
        }

        public HttpRequestMessage GetRequest()
        {
            var createdRequest = this.Request;
            this.FormUrlCollection.ClearForm();
            this.MultiPartFormData.ClearParams();
            this.Request = null;
            return createdRequest;
        }
    }
}