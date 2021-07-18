using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoTBackend.Application.Models;

namespace IoTBackend.Application.Repositories
{
    public interface IDeviceMeasurementRepository
    {
        Task<IEnumerable<DeviceMeasurement>> GetMeasurementsAsync(string deviceName, DateTime date, string sensorType);
    }
}
