using System.Collections.Generic;

namespace FluentHttpRequestBuilderLibrary
{
    public class HttpRequestFormUrlCollection
    {
        private readonly Dictionary<string, string> _requestParams = new Dictionary<string, string>();
        internal readonly HttpRequestBuilder Builder;

        public IReadOnlyDictionary<string, string> RequestParams => _requestParams;

        public HttpRequestFormUrlCollection(HttpRequestBuilder builder)
        {
            Builder = builder;
        }

        internal HttpRequestFormUrlCollection AddParamWithValue(string key, string value)
        {
            _requestParams.Add(key, value);
            return this;
        }

        internal HttpRequestFormUrlCollection RemoveParam(string key)
        {
            _requestParams.Remove(key);
            return this;
        }

        public void ClearForm()
        {
            _requestParams.Clear();
        }
        
    }
}