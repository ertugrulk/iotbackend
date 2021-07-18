using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IoTBackend.Application.ViewModels;

namespace IoTBackend.Infrastructure.Helpers
{
    internal static class DeviceMeasurementHelpers
    {
        internal static async Task<IEnumerable<DeviceMeasurementViewModel>> ParseDeviceMeasurementFile(string sensorType, Stream stream)
        {
            var result = new List<DeviceMeasurementViewModel>();
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (line == null) continue;
                var splitLine = line.Split(";");
                var date = DateTime.Parse(splitLine[0]);
                result.Add(new DeviceMeasurementViewModel(sensorType, date, splitLine[1]));
            }

            return result;
        }
    }
}