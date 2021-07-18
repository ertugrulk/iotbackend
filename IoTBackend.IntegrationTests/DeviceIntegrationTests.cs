using System;
using System.Threading.Tasks;
using System.Text.Json;
using FluentAssertions;
using IoTBackend.API;
using IoTBackend.Application.Mappers;
using IoTBackend.Infrastructure.Services;
using IoTBackend.Infrastructure.Test.Helpers;
using IoTBackend.IntegrationTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IoTBackend.IntegrationTests
{
    [TestClass]
    public class DeviceIntegrationTests
    {
        private static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
       
        [TestMethod]
        public async Task GetMeasurementsv1IntegrationTest()
        {
            // Arrange
            const string deviceName = "testdevice";
            const string sensorType = "temperature";
            var date = new DateTime(2021, 07, 18);
            var mockedStorageService = new MockedStorageService();
            var measurements = MockedDeviceMeasurementHelper.GenerateMeasurements(sensorType, date, 3);
            var fileContents = MockedDeviceMeasurementHelper.MapMeasurementsToFile(measurements);
            var filePath = $"{deviceName}/{sensorType}/{date:yyyy-MM-dd}.csv";
            mockedStorageService.UploadFile(filePath, fileContents);
            var expectedViewModels = DeviceMeasurementViewModelMapper.MapIn(measurements);
            var expectedJson = JsonSerializer.Serialize(expectedViewModels, JsonSerializerOptions);
            using var factory = new IoTBackendWebApplicationFactory<Startup>(services =>
            {
                services.SwapTransient<IStorageService>(provider => mockedStorageService);
            });
            var client = factory.CreateClient();
            
            
            var response = await client.GetAsync($"api/v1/devices/{deviceName}/measurements?date={date:yyyy-MM-dd}&sensorType={sensorType}");
            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().BeEquivalentTo(expectedJson);
        }
    }
}