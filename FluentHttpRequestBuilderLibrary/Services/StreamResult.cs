using System;
using System.IO;
using System.Linq;

namespace FluentHttpRequestBuilderLibrary.Services
{
    public class StreamResult : IDisposable
    {
        public Stream Stream { get; internal set; }
        
        public string FileName { get; internal set; }
        
        public string FilePath { get; internal set; }

        public string FileExtension { get; internal set; }
        
        public StreamResult(Stream stream, string fileName, string filePath)
        {
            Stream = stream;
            FileName = fileName;
            FilePath = filePath;
            FileExtension = filePath.Split(new char[] {'.'}).LastOrDefault();
        }

        public void Dispose()
        {
            Stream?.Dispose();
        }
    }
}