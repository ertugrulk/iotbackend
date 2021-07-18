using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoTBackend.Infrastructure.Services;

namespace IoTBackend.Infrastructure.Test.Helpers
{
    public class MockedStorageService : IStorageService
    {
        public MockedStorageService()
        {
            _uploadedFiles = new Dictionary<string, string>();
        }
        private readonly Dictionary<string, string> _uploadedFiles;
        
        public void UploadFile(string path, string contents)
        {
            _uploadedFiles.Add(path, contents);
        }
        
        public Task<IEnumerable<string>> ListFolderAsync(string path, bool foldersOnly = false)
        {
            var files = _uploadedFiles
                .Keys
                .Where(k => k.StartsWith(path))
                .Select(k => k[(path.Length + 1)..]);
            if (foldersOnly)
            {
                var folders = files
                    .Where(f => f.Contains('/'))
                    .Select(f => f[..f.IndexOf('/')])
                    .Distinct();
                return Task.FromResult(folders);
            }

            var filesInFolder = files.Where(f => !f.Contains('/'));
            return Task.FromResult(filesInFolder);
        }

        public Task<Stream> ReadDocumentStreamAsync(string path)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(_uploadedFiles[path]));
            return Task.FromResult(stream);
        }
    }
}