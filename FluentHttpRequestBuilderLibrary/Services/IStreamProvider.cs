using System.Threading.Tasks;

namespace FluentHttpRequestBuilderLibrary.Services
{
    public interface IStreamProvider
    {
        Task<StreamResult> GetFileStreamAsync(string path);
        StreamResult GetFileStream(string path);
    }
}