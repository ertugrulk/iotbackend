using System;
using System.Globalization;

namespace IoTBackend.Application.Models
{
    public class DeviceMeasurement
    {
        public string SensorType { get; }
        public DateTime DateTime { get; }
        public object Value { get; } 

        public DeviceMeasurement(string sensorType, DateTime dateTime, string value)
        {
            SensorType = sensorType;
            DateTime = dateTime;
            if (decimal.TryParse(value, NumberStyles.Number, new CultureInfo("pl-PL"), out var decValue))
            {
                Value = decValue;
            }
            else
            {
                Value = value;
            }
        }
        
#pragma warning disable 8632
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (DeviceMeasurement) obj;
            return DateTime == other.DateTime && Value == other.Value && SensorType == other.SensorType;
        }
#pragma warning restore 8632


        public override int GetHashCode()
        {
            return HashCode.Combine(SensorType, DateTime, Value);
        }
    }
}