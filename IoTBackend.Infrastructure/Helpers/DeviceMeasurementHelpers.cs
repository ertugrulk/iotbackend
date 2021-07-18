using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IoTBackend.Application.Models;

namespace IoTBackend.Infrastructure.Helpers
{
    internal static class DeviceMeasurementHelpers
    {
        internal static async Task<IEnumerable<DeviceMeasurement>> ParseDeviceMeasurementFile(string sensorType, Stream stream)
        {
            var result = new List<DeviceMeasurement>();
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                try
                {
                    var line = await reader.ReadLineAsync();
                    if (line == null) continue;
                    var splitLine = line.Split(";");
                    var date = DateTime.Parse(splitLine[0]);
                    result.Add(new DeviceMeasurement(sensorType, date, splitLine[1]));
                }
                catch
                {
                    // #TODO: Logging
                }
                    
            }

            return result;
        }
    }
}