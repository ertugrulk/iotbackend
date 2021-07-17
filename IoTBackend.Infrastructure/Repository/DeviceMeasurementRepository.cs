using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTBackend.Application.Exceptions;
using IoTBackend.Application.Repositories;
using IoTBackend.Application.ViewModels;

namespace IoTBackend.Infrastructure.Repository
{
    public class DeviceMeasurementRepository : IDeviceMeasurementRepository
    {
        public Task<IEnumerable<DeviceMeasurementViewModel>> GetMeasurements(string deviceName, DateTime date)
        {
            // #TODO
            if(deviceName != "testdevice")
            {
                throw new DeviceNotFoundException(deviceName);
            }
            var tempResult = new List<DeviceMeasurementViewModel>() { new DeviceMeasurementViewModel("temperature", DateTime.Now, 5) };
            return Task.FromResult(tempResult.AsEnumerable());
        }

        public Task<IEnumerable<DeviceMeasurementViewModel>> GetMeasurements(string deviceName, DateTime date, string sensorType)
        {
            return GetMeasurements(deviceName, date);
        }
    }
}
