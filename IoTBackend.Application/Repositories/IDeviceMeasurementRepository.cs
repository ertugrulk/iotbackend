using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoTBackend.Application.Models;
using IoTBackend.Application.ViewModels;

namespace IoTBackend.Application.Repositories
{
    public interface IDeviceMeasurementRepository
    {
        Task<IEnumerable<DeviceMeasurement>> GetMeasurementsAsync(string deviceName, DateTime date, string sensorType);
    }
}
