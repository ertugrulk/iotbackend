using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IoTBackend.Infrastructure.Services
{
    public interface IStorageService
    {
        Task<IEnumerable<string>> ListFolderAsync(string path, bool foldersOnly = false);
        Task<Stream> ReadDocumentStreamAsync(string path);
    }
}