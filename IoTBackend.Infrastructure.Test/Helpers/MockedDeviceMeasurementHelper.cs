using System;
using System.Collections.Generic;
using System.Linq;
using IoTBackend.Application.Models;

namespace IoTBackend.Infrastructure.Test.Helpers
{
    public static class MockedDeviceMeasurementHelper
    {
        // Could be moved a common assembly with application.test
        public static IEnumerable<DeviceMeasurement> GenerateMeasurements(string sensorType, DateTime date, int amount)
        {
            var result = new List<DeviceMeasurement>();
            for (var i = 0; i < amount; i++)
            {
                result.Add(new DeviceMeasurement(sensorType, date.AddMinutes(i), "testValue"));
            }

            return result;
        }
        
        public static string MapMeasurementsToFile(IEnumerable<DeviceMeasurement> data)
        {
            return string.Join("\n", data.Select(d => $"{d.DateTime:s};{d.Value}"));
        }
    }
}