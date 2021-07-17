using System;
namespace IoTBackend.Application.Exceptions
{
    public class DeviceNotFoundException : Exception
    {
        public string DeviceName { get; }

        public DeviceNotFoundException(string deviceName)
        {
            DeviceName = deviceName;
        }
    }
}
