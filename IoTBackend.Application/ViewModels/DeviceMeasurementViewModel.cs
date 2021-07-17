using System;
namespace IoTBackend.Application.ViewModels
{
    public class DeviceMeasurementViewModel
    {
        public string SensorType { get; }
        public DateTime DateTime { get; }
        public string Value { get; } // to be checked if it's decimal 

        public DeviceMeasurementViewModel(string sensorType, DateTime dateTime, string value)
        {
            SensorType = sensorType;
            DateTime = dateTime;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (DeviceMeasurementViewModel) obj;
            return DateTime == other.DateTime && Value == other.Value && SensorType == other.SensorType;
        }
        

        public override int GetHashCode()
        {
            return HashCode.Combine(SensorType, DateTime, Value);
        }
    }
}
