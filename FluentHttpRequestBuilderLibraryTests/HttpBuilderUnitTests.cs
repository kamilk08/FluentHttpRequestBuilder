using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentHttpRequestBuilderLibrary;
using Newtonsoft.Json;
using Xunit;

namespace FluentHttpRequestBuilderLibraryTests
{
    [Collection("Sequential")]
    public class HttpRequestBuilderUnitTests
    {
        private readonly HttpRequestBuilder _builder;
        private readonly Fixture _fixture;

        public HttpRequestBuilderUnitTests()
        {
            _builder = new HttpRequestBuilder(new FakeStreamProvider());
            _fixture = new Fixture();
        }
        
        [Fact]
        public void WithMethod_WhenCalled_ShouldReturnMessageWithDesiredHttpMethodType()
        {
            var type = HttpMethod.Post;

            var request = _builder.InitializeRequest()
                .WithMethod(type)
                .GetRequest();

            request.Method.Should().Be(type);
        }

        [Fact]
        public void WithUri_WhenCalled_ShouldReturnMessageWithDesiredUriContent()
        {
            var uriContent = "http://www.google.com";

            var request = _builder.InitializeRequest()
                .WithUri(uriContent)
                .GetRequest();

            request.RequestUri.OriginalString.Should().Be(uriContent);
        }

        [Fact]
        public void WithContent_WhenCalled_ShouldSetDesiredTypeOfContentInCreatedRequest()
        {
            var bytes = Enumerable.Repeat<byte>(1, 4096).ToArray();

            var content = new ByteArrayContent(bytes);

            var request = _builder.InitializeRequest()
                .WithContent(content)
                .GetRequest();

            request.Content.Should().Be(content);
        }

        [Fact]
        public async Task WithStringContent_WhenCalled_ShouldSetDesiredStringContentInCreatedRequest()
        {
            var data = new TestObject(Guid.NewGuid(), _fixture.Create<int>(), _fixture.Create<List<char>>());

            var request = _builder.InitializeRequest()
                .WithStringContent(data)
                .GetRequest();

            var stream = await request.Content.ReadAsStreamAsync();
            using (var sr = new StreamReader(stream))
            {
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                var content = await sr.ReadToEndAsync();
                var deserialized = JsonConvert.DeserializeObject<TestObject>(content);

                request.Content.Should().BeOfType<StringContent>();
                deserialized.Should().BeEquivalentTo(data);
            }
        }

        [Fact]
        public void AddHeader_WhenCalled_ShouldAddDesiredHeader()
        {
            var request = _builder.InitializeRequest()
                .AddHeader("accept", "application/json")
                .AddHeader("connection", "keep-alive")
                .GetRequest();

            request.Headers.Accept.Should().NotBeNull();
            request.Headers.Accept.Count.Should().Be(1);
            request.Headers.Connection.Should().NotBeNull();
            request.Headers.Connection.Count.Should().Be(1);
        }

        [Fact]
        public void AddBearerToken_WhenCalled_ShouldAddBearerAuthenticationToken()
        {
            var rawToken =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            var request = _builder.InitializeRequest()
                .AddBearerToken(rawToken)
                .GetRequest();
            
            request.Headers.Authorization.Should().NotBeNull();
            request.Headers.Authorization.Scheme.Should().Be("Bearer");
            request.Headers.Authorization.Parameter.Should().Be(rawToken);
        }

        [Fact]
        public async Task WithFormUrlEncodedContent_WhenCalled_ShouldCreateRequestWithContentOfTypeUrlEncoded()
        {
            var firstKey = "key";
            var firstValue = "value";
            var secondKey = "key2";
            var secondValue = "value2";

            var request = _builder.InitializeRequest()
                .WithFormUrlEncodedContent()
                .AddFormKeyValue(firstKey, firstValue)
                .AddFormKeyValue(secondKey, secondValue)
                .PassFormValuesAsHttpContent()
                .GetRequest();

            Dictionary<string, string> dict;

            request.Content.Should().BeOfType<FormUrlEncodedContent>();
            var content = request.Content as FormUrlEncodedContent;

            var stream = await content.ReadAsStreamAsync();
            using (stream)
            {
                using (var sr = new StreamReader(stream))
                {
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    var end = await sr.ReadToEndAsync();
                    var values = end.Split(new char[] {'&'}).ToArray();

                    dict = values.ToDictionary(k => k.Split(new char[] {'='}).First(),
                        v => v.Split(new char[] {'='}).Last());
                }
            }

            dict[firstKey].Should().Be(firstValue);
            dict[secondKey].Should().Be(secondValue);
        }

        [Fact]
        public void WithStreamContent_WhenCalled_ShouldCreateRequestWithStreamContent()
        {

            var request = _builder.InitializeRequest()
                .WithStreamContent()
                .GetFileStream(_fixture.Create<string>())
                .PassStreamAsHttpContent()
                .GetRequest();

            request.Content.Should().BeOfType<StreamContent>();
        }

        [Fact]
        public void WithMultiPartFormData_WhenCalled_ShouldCreateRequestWithMultiPartFormData()
        {
            
            var request = _builder.InitializeRequest()
                .WithMultiPartFormData()
                .AddFileParam(_fixture.Create<string>())
                .AddStringParam("key", "value")
                .AddByteArrayParam("bytes", Enumerable.Repeat<byte>(1, 4096).ToArray())
                .AddUrlEncodedParam("url", "urlvalue")
                .PassAsMultiPartDataContent()
                .GetRequest();

            request.Content.Should().BeOfType<MultipartFormDataContent>();
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(200)]
        public void WithStringContent_WhenCalledMultipleTimes_ShouldCreateMultipleRequests(int count)
        {
            var lst = new List<HttpRequestMessage>();

            for (int i = 0; i < count; i++)
            {
                var request = _builder.InitializeRequest()
                    .AddHeader("accept", "application/json")
                    .WithMethod(HttpMethod.Post)
                    .WithUri("http://www.google.com")
                    .WithStringContent(new TestObject(_fixture.Create<Guid>(), _fixture.Create<int>(),
                        _fixture.Create<List<Char>>()))
                    .GetRequest();
                
                lst.Add(request);
            }

            lst.Count.Should().Be(count);
            lst.Select(s => s.Method).All(s=>s==HttpMethod.Post).Should().BeTrue();
            lst.Select(s => s.Content).Should().AllBeAssignableTo<StringContent>();
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(200)]
        public void WithFormUrlEncodedContent_WhenCalledMultipleTimes_ShouldCreateMultipleRequests(int count)
        {
            var lst = new List<HttpRequestMessage>();

            for (int i = 0; i < count; i++)
            {
                var request = _builder.InitializeRequest()
                    .AddHeader("connection","keep-alive")
                    .WithMethod(HttpMethod.Put)
                    .WithUri("http://www.google.com")
                    .WithFormUrlEncodedContent()
                    .AddFormKeyValue(_fixture.Create<string>(),_fixture.Create<string>())
                    .AddFormKeyValue(_fixture.Create<string>(),_fixture.Create<string>())
                    .PassFormValuesAsHttpContent()
                    .GetRequest();
                
                lst.Add(request);
            }

            lst.Count.Should().Be(count);
            lst.Select(s => s.Method).All(s=>s==HttpMethod.Put).Should().BeTrue();
            lst.Select(s => s.Content).Should().AllBeAssignableTo<FormUrlEncodedContent>();
        }
        
        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(200)]
        public void WithStreamContent_WhenCalledMultipleTimes_ShouldCreateMultipleRequests(int count)
        {
            var lst = new List<HttpRequestMessage>();

            for (int i = 0; i < count; i++)
            {
                var request = _builder.InitializeRequest()
                    .AddHeader("connection","keep-alive")
                    .WithMethod(HttpMethod.Post)
                    .WithUri("http://www.google.com")
                    .WithStreamContent()
                    .GetFileStream(_fixture.Create<string>())
                    .PassStreamAsHttpContent()
                    .GetRequest();
                
                lst.Add(request);
            }

            lst.Count.Should().Be(count);
            lst.Select(s => s.Method).All(s=>s==HttpMethod.Post).Should().BeTrue();
            lst.Select(s => s.Content).Should().AllBeAssignableTo<StreamContent>();
        }
        
        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(200)]
        public void WithMultiPartData_WhenCalledMultipleTimes_ShouldCreateMultipleRequests(int count)
        {
            var lst = new List<HttpRequestMessage>();

            var temp = AppDomain.CurrentDomain.BaseDirectory;

            var path = Path.Combine(temp, "E7iaevAVIAMM5MC.jpg");
            
            for (int i = 0; i < count; i++)
            {
                var request = _builder.InitializeRequest()
                    .AddHeader("connection","keep-alive")
                    .WithMethod(HttpMethod.Post)
                    .WithUri("http://www.google.com")
                    .WithMultiPartFormData()
                    .AddFileParam(path)
                    .AddStringParam("string_param","val")
                    .AddByteArrayParam("byte_array_param",Enumerable.Repeat<byte>(9,1024).ToArray())
                    .AddUrlEncodedParam("urlencoded_param_one","value_one")
                    .AddUrlEncodedParam("urlencoded_param_two","value_two")
                    .PassAsMultiPartDataContent()
                    .GetRequest();
                
                lst.Add(request);
            }

            lst.Count.Should().Be(count);
            lst.Select(s => s.Method).All(s=>s==HttpMethod.Post).Should().BeTrue();
            lst.Select(s => s.Content).Should().AllBeAssignableTo<MultipartFormDataContent>();
        }
    }
}