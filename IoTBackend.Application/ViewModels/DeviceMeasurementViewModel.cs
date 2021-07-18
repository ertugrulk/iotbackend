using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global

namespace IoTBackend.Application.ViewModels
{
    public class DeviceMeasurementViewModel
    {
        public DateTime DateTime { get; }
        public Dictionary<string, object> Sensors { get; }

        public DeviceMeasurementViewModel(DateTime dateTime, Dictionary<string, object> sensors)
        {
            DateTime = dateTime;
            Sensors = sensors;
        }

#pragma warning disable 8632
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (DeviceMeasurementViewModel) obj;
            if (other.DateTime != this.DateTime) return false;
            if ((Sensors == null && other.Sensors != null) || (Sensors != null && other.Sensors == null)) return false;
            if (Sensors == null) return true;
            if (Sensors.Count() != other.Sensors!.Count()) return false;
            var sensorList = Sensors.ToList();
            var otherSensorList = other.Sensors.ToList();
            for (var i = 0; i < sensorList.Count; i++)
            {
                var (sourceKey, sourceValue) = sensorList[i];
                var (destKey, destValue) = otherSensorList[i];
                if (sourceKey != destKey || sourceValue != destValue) return false;
            }

            return true;
        }
#pragma warning restore 8632

        public override int GetHashCode()
        {
            return HashCode.Combine(DateTime, Sensors);
        }
    }
}