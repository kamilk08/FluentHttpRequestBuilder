using System;
using System.IO;
using System.Threading.Tasks;

namespace FluentHttpRequestBuilderLibrary.Services
{
    public class FileStreamProvider : IStreamProvider
    {
        public Task<StreamResult> GetFileStreamAsync(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path), "File path cannot be null or empty");

            if(!File.Exists(path)) throw new InvalidOperationException("Cannot locate certain file.");

            var streamResult = GetStreamInternal(path);

            return Task.FromResult(streamResult);
        }

        public StreamResult GetFileStream(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path), "File path cannot be null or empty");

            if(!File.Exists(path)) throw new InvalidOperationException("Cannot locate certain file.");
            
            return GetStreamInternal(path);
        }
        
        private StreamResult GetStreamInternal(string path)
        {
            var fileName = Path.GetFileName(path);

            var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            var streamResult = new StreamResult(stream, fileName, path);
            return streamResult;
        }
    }
}