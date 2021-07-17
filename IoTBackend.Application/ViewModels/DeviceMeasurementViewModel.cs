using System;
namespace IoTBackend.Application.ViewModels
{
    public class DeviceMeasurementViewModel
    {
        public string SensorType { get; }
        public DateTime DateTime { get; }
        public object Value { get; } // to be checked if it's decimal 

        public DeviceMeasurementViewModel(string sensorType, DateTime dateTime, object value)
        {
            SensorType = sensorType;
            DateTime = dateTime;
            Value = value;
        }
    }
}
