using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using IoTBackend.Application.ViewModels;

namespace IoTBackend.Infrastructure.Helpers
{
    internal static class DeviceMeasurementHelpers
    {
        internal static IEnumerable<DeviceMeasurementViewModel> ParseDeviceMeasurementFile(string sensorType, string contents)
        {
            var lines = contents.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
            return lines.Select(l =>
            {
                var splitLine = l.Split(";");
                var date = DateTime.Parse(splitLine[0]);
                return new DeviceMeasurementViewModel(sensorType, date, splitLine[1]);
            });
        }
    }
}