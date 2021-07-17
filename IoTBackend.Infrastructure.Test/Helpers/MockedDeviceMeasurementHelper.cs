using System;
using System.Collections.Generic;
using System.Linq;
using IoTBackend.Application.ViewModels;

namespace IoTBackend.Infrastructure.Test.Helpers
{
    internal static class MockedDeviceMeasurementHelper
    {
        // Could be moved a common assembly with application.test
        internal static IEnumerable<DeviceMeasurementViewModel> GenerateMeasurements(string sensorType, DateTime date, int amount)
        {
            var result = new List<DeviceMeasurementViewModel>();
            for (var i = 0; i < amount; i++)
            {
                result.Add(new DeviceMeasurementViewModel(sensorType, date, "testValue"));
            }

            return result;
        }
        
        internal static string MapMeasurementsToFile(IEnumerable<DeviceMeasurementViewModel> data)
        {
            return string.Join("\n", data.Select(d => $"{d.DateTime:s};{d.Value}"));
        }
    }
}