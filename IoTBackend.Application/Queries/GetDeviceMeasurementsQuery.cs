using System;
using System.Collections.Generic;
using IoTBackend.Application.ViewModels;
using MediatR;

namespace IoTBackend.Application.Queries
{
    public class GetDeviceMeasurementsQuery : IRequest<IEnumerable<DeviceMeasurementViewModel>>
    {
        public GetDeviceMeasurementsQuery(string deviceName, DateTime date, string sensorType)
        {
            DeviceName = deviceName;
            Date = date;
            SensorType = sensorType;
        }
        public string DeviceName { get; }
        public DateTime Date { get; }
        public string SensorType { get; }
    }
}
