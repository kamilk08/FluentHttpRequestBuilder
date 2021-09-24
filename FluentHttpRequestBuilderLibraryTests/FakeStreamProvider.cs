using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentHttpRequestBuilderLibrary.Services;

namespace FluentHttpRequestBuilderLibraryTests
{
    public class FakeStreamProvider : IStreamProvider
    {
        public Task<StreamResult> GetFileStreamAsync(string path)
        {
           var streamResult= CreateStreamResultInternal(path);

           return Task.FromResult(streamResult);
        }

        private static StreamResult CreateStreamResultInternal(string path)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(path));

            var result = new StreamResult(stream, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            return result;
        }

        public StreamResult GetFileStream(string path)
        {
            return CreateStreamResultInternal(path);
        }
    }
}