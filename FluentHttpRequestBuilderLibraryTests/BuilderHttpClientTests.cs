using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentHttpRequestBuilderLibrary;
using FluentHttpRequestBuilderLibraryTests.Constatns;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace FluentHttpRequestBuilderLibraryTests
{
    [TestFixture]
    public class BuilderHttpClientTests
    {
        private Fixture _fixture;
        private HttpRequestBuilder _builder;

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
        
        [SetUp]
        public void SetUp()
        {
            _builder = new HttpRequestBuilder(new FakeStreamProvider());

            _fixture = new Fixture();
        }
    }
}