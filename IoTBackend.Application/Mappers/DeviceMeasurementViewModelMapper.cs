using System.Collections.Generic;
using System.Linq;
using IoTBackend.Application.Models;
using IoTBackend.Application.ViewModels;

namespace IoTBackend.Application.Mappers
{
    public static class DeviceMeasurementViewModelMapper
    {
        public static IEnumerable<DeviceMeasurementViewModel> MapIn(IEnumerable<DeviceMeasurement> deviceMeasurements)
        {
            var results = from measurement in deviceMeasurements
                group measurement by measurement.DateTime
                into mg
                select new DeviceMeasurementViewModel(mg.Key,
                    mg.Select(item => new KeyValuePair<string, object>(item.SensorType, item.Value))
                        .ToDictionary(item => item.Key, item => item.Value));
            return results;
        }
    }
}