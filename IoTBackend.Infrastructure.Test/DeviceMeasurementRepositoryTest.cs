using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task DeviceMeasurementRepository_ReturnsEmptyListWhenNoFileExists()
        {
            var mockedStorageService = new Mock<IStorageService>();
            mockedStorageService.Setup(mss => mss.ListFolderAsync(It.IsAny<string>()))
                .ReturnsAsync(Enumerable.Empty<string>());
            var repository = new DeviceMeasurementRepository(mockedStorageService.Object);

            var results = await repository.GetMeasurementsAsync(deviceName: "testdevice", date: new DateTime(2021, 07, 17),
                sensorType: null);

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
            var mockedStorageService = new Mock<IStorageService>();
            mockedStorageService.Setup(mss => mss.ListFolderAsync(deviceName))
                .ReturnsAsync(new[] { sensorType });
            mockedStorageService.Setup(mss => mss.ListFolderAsync($"{deviceName}/{sensorType}"))
                .ReturnsAsync(new[] {fileName});
            mockedStorageService.Setup(mss => mss.ReadDocumentAsStringAsync($"{deviceName}/{sensorType}/{fileName}"))
                .ReturnsAsync(mockedFileContents);
            var repository = new DeviceMeasurementRepository(mockedStorageService.Object);

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
            var mockedStorageService = new Mock<IStorageService>();
            foreach (var sensorType in sensorTypes)
            {
                var measurements = MockedDeviceMeasurementHelper.GenerateMeasurements(sensorType, date, 2);
                var mockedFileContents = MockedDeviceMeasurementHelper.MapMeasurementsToFile(measurements);
                mockedStorageService.Setup(mss => mss.ListFolderAsync($"{deviceName}/{sensorType}"))
                    .ReturnsAsync(new[] {"bogus.csv", fileName, "bogus2.csv"});
                mockedStorageService.Setup(mss => mss.ReadDocumentAsStringAsync($"{deviceName}/{sensorType}/{fileName}"))
                    .ReturnsAsync(mockedFileContents);
            }
            mockedStorageService.Setup(mss => mss.ListFolderAsync(deviceName))
                .ReturnsAsync(sensorTypes);
            var repository = new DeviceMeasurementRepository(mockedStorageService.Object);

            var results = await repository.GetMeasurementsAsync(deviceName, date, null);
            
            foreach (var sensorType in sensorTypes)
            {
                results.Where(r => r.SensorType == sensorType).Should().HaveCount(measurementPerSensorType);
            }
        }

        [TestMethod]
        public void DeviceMeasurementRepository_ExtractsMeasurementsFromHistoricalZipWhenFileDoesNotExist()
        {
            // TODO: Create mocked forms of zip archive per measurement containing the CSV file for given date
        }
    }
}
