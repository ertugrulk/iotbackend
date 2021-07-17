using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IoTBackend.Infrastructure.Services
{
    public interface IStorageService
    {
        Task<IEnumerable<string>> ListFolderAsync(string path);
        Task<string> ReadDocumentAsStringAsync(string path);
        Task ReadDocumentStreamAsync(string path, Stream stream);
    }
}