using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentHttpRequestBuilderLibrary.Constants;
using FluentHttpRequestBuilderLibrary.Parameters;

namespace FluentHttpRequestBuilderLibrary
{
    public static class HttpRequestBuilderExtensions
    {
        public static HttpRequestBuilder AddBearerToken(this HttpRequestBuilder builder,string token)
        {
            builder.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return builder;
        }
        
        public static HttpRequestFormUrlCollection WithFormUrlEncodedContent(this HttpRequestBuilder builder)
        {
            return builder.FormUrlCollection;
        }

        public static HttpRequestFormUrlCollection AddFormKeyValue(this HttpRequestFormUrlCollection collection, string key,
            string value)
        {
            collection.AddParamWithValue(key, value);

            return collection.Builder.FormUrlCollection;
        }

        public static HttpRequestBuilder PassFormValuesAsHttpContent(
            this HttpRequestFormUrlCollection formUrlCollection)
        {
            var builder =
                formUrlCollection.Builder.WithContent(new FormUrlEncodedContent(formUrlCollection.RequestParams));

            return builder;
        }

        public static HttpRequestStreamContent WithStreamContent(this HttpRequestBuilder builder)
        {
            return builder.StreamContent;
        }

        public static HttpRequestStreamContent GetFileStream(this HttpRequestStreamContent streamContent, string path)
        {
            var streamResult = streamContent.Builder.StreamProvider.GetFileStream(path);
            
            streamContent.Value = new StreamContent(streamResult.Stream);

            return streamContent;
        }

        public static HttpRequestStreamContent SetContentType(this HttpRequestStreamContent streamContent,
            string contentType = null)
        {
            streamContent.Value.Headers.ContentType =
                new MediaTypeHeaderValue(contentType ?? "application/octet-stream");

            return streamContent;
        }

        public static HttpRequestStreamContent SetContentDisposition(this HttpRequestStreamContent streamContent,
            string type = null)
        {
            streamContent.Value.Headers.ContentDisposition = new ContentDispositionHeaderValue(type ?? "attachment")
            {
                FileName = streamContent.FileName
            };

            return streamContent;
        }

        public static HttpRequestBuilder PassStreamAsHttpContent(this HttpRequestStreamContent streamContent)
        {
            if (streamContent.Value.Headers.ContentType == null)
                streamContent.SetContentType();

            if (streamContent.Value.Headers.ContentDisposition == null)
                streamContent.SetContentDisposition();

            streamContent.Builder.WithContent(streamContent.Value);

            return streamContent.Builder;
        }

        public static HttpRequestMultiPartFormData WithMultiPartFormData(this HttpRequestBuilder builder)
        {
            return builder.MultiPartFormData;
        }

        public static HttpRequestMultiPartFormData AddUrlEncodedParam(this HttpRequestMultiPartFormData formData,
            string key, string value)
        {
            formData.UrlCollection.AddParamWithValue(key, value);
            
            return formData;
        }

        public static HttpRequestBuilder PassAsMultiPartDataContent(this HttpRequestMultiPartFormData formData)
        {
            var content = new MultipartFormDataContent();

            var @params = formData.FormParams();
            var fileParams = @params.Where(p => p.ParameterType == ParameterType.Stream).Cast<StreamParameter>()
                .ToList();

            var restOfParams = @params.Where(p => p.ParameterType != ParameterType.Stream).ToList();

            foreach (var param in fileParams)
                content.Add(param.ToHttpContent(), param.Key, param.StreamResult.FileName);

            foreach (var param in restOfParams)
                content.Add(param.ToHttpContent(), param.Key);
            
            content.Add(new FormUrlEncodedContent(formData.UrlCollection.RequestParams));
            
            var builder = formData.Builder.WithContent(content);

            return builder;
        }
    }
}