using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IoTBackend.Application.Exceptions;
using IoTBackend.Infrastructure.Helpers;
using IoTBackend.Infrastructure.Repository;
using IoTBackend.Infrastructure.Services;
using IoTBackend.Infrastructure.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IoTBackend.Infrastructure.Test
{
    [TestClass]
    public class DeviceMeasurementRepositoryTest
    {
        [TestMethod]
        public async Task DeviceMeasurementRepository_ReturnsEmptyListWhenDateDoesNotExist()
        {
            var deviceName = "testdevice";
            var sensorType = "temperature";
            var existingFileDate = new DateTime(2021, 07, 17);
            var existingFileName = $"{existingFileDate:yyyy-MM-dd}.csv";
            var existingFilePath = $"{deviceName}/{sensorType}/{existingFileName}";
            var requestedDate = new DateTime(2021, 07, 18);
            var mockedStorageService = new MockedStorageService();
            mockedStorageService.UploadFile(existingFilePath, "random;csv;contents");
            var repository = new DeviceMeasurementRepository(mockedStorageService);

            var results = await repository.GetMeasurementsAsync(deviceName, requestedDate, sensorType);

            results.Should().BeEmpty();
        }
        
        [TestMethod]
        public async Task DeviceMeasurementRepository_ReturnsMeasurementsWhenOneSensorAndOneFileExists()
        {
            const string deviceName = "testdevice";
            const string sensorType = "temperature";
            var date = new DateTime(2021, 07, 17);
            var fileName = $"{date:yyyy-MM-dd}.csv";
            var measurements = MockedDeviceMeasurementHelper.GenerateMeasurements(sensorType, date, 1);
            var mockedFileContents = MockedDeviceMeasurementHelper.MapMeasurementsToFile(measurements);
            var mockedStorageService = new MockedStorageService();
            mockedStorageService.UploadFile($"{deviceName}/{sensorType}/{fileName}", mockedFileContents);
            var repository = new DeviceMeasurementRepository(mockedStorageService);

            var results = await repository.GetMeasurementsAsync(deviceName, date, sensorType);
            results.Should().Equal(measurements);
        }
        
        [TestMethod]
        public async Task DeviceMeasurementRepository_ReturnsMeasurementsFromMultipleSensorsForGivenDate()
        {
            const string deviceName = "testdevice";
            var sensorTypes = new[] {"temperature", "humidity", "rainfall"};
            const int measurementPerSensorType = 2;
            var date = new DateTime(2021, 07, 17);
            var fileName = $"{date:yyyy-MM-dd}.csv";
            var mockedStorageService = new MockedStorageService();
            foreach (var sensorType in sensorTypes)
            {
                var measurements = MockedDeviceMeasurementHelper.GenerateMeasurements(sensorType, date, 2);
                var mockedFileContents = MockedDeviceMeasurementHelper.MapMeasurementsToFile(measurements);
                mockedStorageService.UploadFile($"{deviceName}/{sensorType}/bogus.csv", mockedFileContents);
                mockedStorageService.UploadFile($"{deviceName}/{sensorType}/{fileName}", mockedFileContents);
            }
            var repository = new DeviceMeasurementRepository(mockedStorageService);

            var results = await repository.GetMeasurementsAsync(deviceName, date, null);
            
            foreach (var sensorType in sensorTypes)
            {
                results.Where(r => r.SensorType == sensorType).Should().HaveCount(measurementPerSensorType);
            }
        }

        [TestMethod]
        public void DeviceMeasurementRepository_ThrowsErrorWhenInvalidDeviceNameIsSent()
        {
            const string deviceName = "invalid-device";
            var date = new DateTime(2021, 07, 17);

            var mockedStorageService = new MockedStorageService();
            var repository = new DeviceMeasurementRepository(mockedStorageService);

            FluentActions
                .Invoking(() => repository.GetMeasurementsAsync(deviceName, date, null))
                .Should()
                .ThrowAsync<DeviceNotFoundException>();
        }
        [TestMethod]
        public void DeviceMeasurementRepository_ExtractsMeasurementsFromHistoricalZipWhenFileDoesNotExist()
        {
            // TODO: Create mocked forms of zip archive per measurement containing the CSV file for given date
        }
    }
}
