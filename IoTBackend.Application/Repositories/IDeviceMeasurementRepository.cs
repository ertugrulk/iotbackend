using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoTBackend.Application.ViewModels;

namespace IoTBackend.Application.Repositories
{
    public interface IDeviceMeasurementRepository
    {
        Task<IEnumerable<DeviceMeasurementViewModel>> GetMeasurements(string deviceName, DateTime date);
        Task<IEnumerable<DeviceMeasurementViewModel>> GetMeasurements(string deviceName, DateTime date, string sensorType);
    }
}
