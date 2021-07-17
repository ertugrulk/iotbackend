using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using IoTBackend.Application.Exceptions;
using IoTBackend.Application.Repositories;
using IoTBackend.Application.ViewModels;
using IoTBackend.Infrastructure.Helpers;
using IoTBackend.Infrastructure.Services;

namespace IoTBackend.Infrastructure.Repository
{
    public class DeviceMeasurementRepository : IDeviceMeasurementRepository
    {
        private readonly IStorageService _storageService;
        private const string HistoricalZipFile = "historical.zip";
        public DeviceMeasurementRepository(IStorageService storageService)
        {
            _storageService = storageService;
        }
        private async Task<IEnumerable<DeviceMeasurementViewModel>> GetMeasurementsFromHistoricalData(string deviceName, string folder, string expectedFile)
        {
            await using var zipStream = new MemoryStream();
            await _storageService.ReadDocumentStreamAsync($"{deviceName}/{folder}/{HistoricalZipFile}",
                zipStream);
            using var archive = new ZipArchive(zipStream);
            var expectedEntry = archive.Entries.FirstOrDefault(e => e.Name == expectedFile);
            if (expectedEntry == null)
            {
                return Enumerable.Empty<DeviceMeasurementViewModel>();
            }
            
            using var sr = new StreamReader(expectedEntry.Open());
            var contents = await sr.ReadToEndAsync();
            return DeviceMeasurementHelpers.ParseDeviceMeasurementFile(folder, contents);

        }
        public async Task<IEnumerable<DeviceMeasurementViewModel>> GetMeasurementsAsync(string deviceName, DateTime date, string sensorType)
        {
            if (string.IsNullOrEmpty(deviceName))
            {
                throw new ArgumentNullException(nameof(deviceName));
            }
            IEnumerable<string> folders;
            try
            {
                folders = await _storageService.ListFolderAsync(deviceName);
            }
            catch (DirectoryNotFoundException)
            {
                throw new DeviceNotFoundException(deviceName);
            }

            // Filter sensors if needed
            if (!string.IsNullOrEmpty(sensorType))
            {
                folders = folders.Where(f => f == sensorType).ToList();
            }

            var results = new List<DeviceMeasurementViewModel>();
            foreach (var folder in folders)
            {
                var path = $"{deviceName}/{folder}";
                var files = await _storageService.ListFolderAsync(path);
                var expectedFile = $"{date:yyyy-MM-dd}.csv";
                if (files.Contains(expectedFile))
                {
                    var contents = await _storageService.ReadDocumentAsStringAsync($"{path}/{expectedFile}");
                    results.AddRange(DeviceMeasurementHelpers.ParseDeviceMeasurementFile(folder, contents));
                }
                else // Extract contents from archival zip file
                {
                    
                    if (!files.Contains(HistoricalZipFile)) continue;
                    results.AddRange(await GetMeasurementsFromHistoricalData(deviceName, folder, expectedFile));
                }
            }

            return results;

        }
    }
}
