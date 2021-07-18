using System;
using System.Collections.Generic;
using System.Threading;
using IoTBackend.Application.Queries;
using IoTBackend.Application.Handlers;
using IoTBackend.Application.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using IoTBackend.Application.Mappers;
using IoTBackend.Application.Models;

namespace IoTBackend.Application.Test
{
    [TestClass]
    public class GetDeviceMeasurementsQueryHandlerTests
    {
        private static IEnumerable<DeviceMeasurement> GenerateMeasurements(DateTime date, int amount)
        {
            var result = new List<DeviceMeasurement>();
            for (var i = 0; i < amount; i++)
            {
                result.Add(new DeviceMeasurement("testsensor", date.AddMinutes(i), "0"));
            }
            return result;
        }

        [TestMethod]
        public async Task GetDeviceMeasurementsQueryHandler_ReturnMeasurementsByDeviceNameAndDate_OK()
        {
            const string expectedDeviceName = "testdevice";
            var expectedDate = new DateTime(2021, 07, 17);
            var mockedMeasurements = GenerateMeasurements(expectedDate, 10);
            var expectedRecords = DeviceMeasurementViewModelMapper.MapIn(mockedMeasurements);
            var mockedRepo = new Mock<IDeviceMeasurementRepository>();
            mockedRepo.Setup(r => r.GetMeasurementsAsync(expectedDeviceName, expectedDate, null)).ReturnsAsync(mockedMeasurements);
            var handler = new GetDeviceMeasurementsQueryHandler(mockedRepo.Object);
            var token = new CancellationToken();
            var query = new GetDeviceMeasurementsQuery(expectedDeviceName, expectedDate, null);

            var results = await handler.Handle(query, token);

            mockedRepo.Verify(mr => mr.GetMeasurementsAsync(expectedDeviceName, expectedDate, null));
            results.Should().NotBeNull().And.Equal(expectedRecords);
        }

        [TestMethod]
        public async Task GetDeviceMeasurementsQueryHandler_ReturnMeasurementsByDeviceNameDateAndSensorType_OK()
        {
            const string expectedDeviceName = "testdevice";
            var expectedDate = new DateTime(2021, 07, 17);
            var mockedMeasurements = GenerateMeasurements(expectedDate, 10);
            var expectedRecords = DeviceMeasurementViewModelMapper.MapIn(mockedMeasurements);
            const string expectedSensorType = "temperature";
            var mockedRepo = new Mock<IDeviceMeasurementRepository>();
            mockedRepo.Setup(r => r.GetMeasurementsAsync(expectedDeviceName, expectedDate, expectedSensorType)).ReturnsAsync(mockedMeasurements);
            var handler = new GetDeviceMeasurementsQueryHandler(mockedRepo.Object);
            var token = new CancellationToken();
            var query = new GetDeviceMeasurementsQuery(expectedDeviceName, expectedDate, expectedSensorType);

            var results = await handler.Handle(query, token);

            mockedRepo.Verify(mr => mr.GetMeasurementsAsync(expectedDeviceName, expectedDate, expectedSensorType));
            results.Should().NotBeNull().And.Equal(expectedRecords);
        }

        [TestMethod]
        public void GetDeviceMeasurementsQueryHandler_ThrowsExceptionWhenDeviceNameIsInvalid()
        {
            var handler = new GetDeviceMeasurementsQueryHandler(null);
            var token = new CancellationToken();

            var queryWithNullDeviceName = new GetDeviceMeasurementsQuery(deviceName: null, date: new DateTime(2021, 7, 17), sensorType: null);
            var queryWithEmptyDeviceName = new GetDeviceMeasurementsQuery(deviceName: "", date: new DateTime(2021, 7, 17), sensorType: null);

            FluentActions.Invoking(() => handler.Handle(queryWithNullDeviceName, token)).Should().ThrowAsync<ValidationException>();
            FluentActions.Invoking(() => handler.Handle(queryWithEmptyDeviceName, token)).Should().ThrowAsync<ValidationException>();
        }

        [TestMethod]
        public void GetDeviceMeasurementsQueryHandler_ThrowsExceptionWhenDateIsInvalid()
        {
            var handler = new GetDeviceMeasurementsQueryHandler(null);
            var token = new CancellationToken();

            var queryWithNoDate = new GetDeviceMeasurementsQuery(deviceName: "testDevice", date: DateTime.MinValue, sensorType: null);
            var queryWithFutureDate = new GetDeviceMeasurementsQuery(deviceName: "testDevice", date: DateTime.Now.AddDays(2), sensorType: null);

            FluentActions.Invoking(() => handler.Handle(queryWithNoDate, token)).Should().ThrowAsync<ValidationException>();
            FluentActions.Invoking(() => handler.Handle(queryWithFutureDate, token)).Should().ThrowAsync<ValidationException>();
        }
    }
}
