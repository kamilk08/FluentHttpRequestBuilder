using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentHttpRequestBuilderLibrary;
using FluentHttpRequestBuilderLibraryTests.Constatns;
using RichardSzalay.MockHttp;
using Xunit;

namespace FluentHttpRequestBuilderLibraryTests
{
    [Collection("Sequential")]
    public class BuilderHttpClientTests
    {
        private readonly Fixture _fixture;
        private readonly HttpRequestBuilder _builder;

        public BuilderHttpClientTests()
        {
            _builder = new HttpRequestBuilder(new FakeStreamProvider());

            _fixture = new Fixture();
        }
        
        [Fact]
        public async Task GetRequest_WhenUserTriesToSendPostRequestWithJsonContent_ThenShouldGet200Response()
        {
            var request = _builder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(Extensions.PostUrl)
                .AddHeader("accept", "application/json")
                .WithStringContent(new TestObject(_fixture.Create<Guid>(), _fixture.Create<int>(),
                    _fixture.Create<List<char>>()))
                .GetRequest();
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When(Extensions.PostUrl)
                .Respond(req => new HttpResponseMessage(HttpStatusCode.OK));
            
            var client = new HttpClient(mockHttp);

            var response = await client.SendAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRequest_WhenUserTriesToSendPostRequestWithStreamContent_ThenShouldGet200Response()
        {
            var request = _builder.InitializeRequest()
                .WithUri(Extensions.PostUrl)
                .AddHeader("connection", "keep-alive")
                .WithStreamContent()
                .GetFileStream(_fixture.Create<string>())
                .PassStreamAsHttpContent()
                .GetRequest();

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(Extensions.PostUrl)
                .Respond(res => new HttpResponseMessage(HttpStatusCode.OK));
            
            var client = new HttpClient(mockHttp);

            var response = await client.SendAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task GetRequest_WhenUserTriesToSendPostRequestWithMultiPartFormData_ThenShouldGet200Response()
        {
            var request = _builder.InitializeRequest()
                .WithUri(Extensions.PostUrl)
                .AddHeader("connection", "keep-alive")
                .WithMultiPartFormData()
                .AddFileParam(_fixture.Create<string>())
                .AddStringParam(_fixture.Create<string>(), _fixture.Create<string>())
                .AddByteArrayParam(_fixture.Create<string>(), _fixture.Create<byte[]>())
                .AddUrlEncodedParam(_fixture.Create<string>(), _fixture.Create<string>())
                .PassAsMultiPartDataContent()
                .GetRequest();
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(Extensions.PostUrl)
                .Respond(res => new HttpResponseMessage(HttpStatusCode.OK));
            
            var client = new HttpClient(mockHttp);

            var response = await client.SendAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRequest_WhenUserTriesToSendPostRequestWithUrlEncodedForm_ThenShouldGet200Response()
        {
            var request = _builder
                .InitializeRequest()
                .WithUri(Extensions.PostUrl)
                .AddHeader("connection", "keep-alive")
                .WithFormUrlEncodedContent()
                .AddFormKeyValue(_fixture.Create<string>(), _fixture.Create<string>())
                .AddFormKeyValue(_fixture.Create<string>(), _fixture.Create<string>())
                .PassFormValuesAsHttpContent()
                .GetRequest();
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(Extensions.PostUrl)
                .Respond(res => new HttpResponseMessage(HttpStatusCode.OK));
            
            var client = new HttpClient(mockHttp);

            var response = await client.SendAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
    }
}