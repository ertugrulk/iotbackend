using System;
using FluentAssertions;
using IoTBackend.Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IoTBackend.Application.Test.Models
{
    [TestClass]
    public class DeviceMeasurementTests
    {

        [TestMethod]
        public void DeviceMeasurement_Init_OK()
        {
            const string expectedSensorType = "temperature";
            const string expectedValue = "5.2";
            var expectedDateTime = new DateTime(2021, 07, 18, 12, 27, 0);
            
            var dm = new DeviceMeasurement(expectedSensorType, expectedDateTime, expectedValue);

            dm.SensorType.Should().BeEquivalentTo(expectedSensorType);
            dm.DateTime.Should().BeSameDateAs(expectedDateTime);
            dm.Value.Should().BeEquivalentTo(expectedValue);
        }

        [TestMethod]
        public void DeviceMeasurement_Equals_OK()
        {
            const string sensorType = "temperature";
            const string value = "5";
            var dateTime = new DateTime(2021, 07, 18, 12, 27, 0);
            var leftDm = new DeviceMeasurement(sensorType, dateTime, value);
            var rightDm = new DeviceMeasurement(sensorType, dateTime, value);
            
            leftDm.Should().BeEquivalentTo(rightDm);
            rightDm.Should().BeEquivalentTo(leftDm);
        }
        
        [TestMethod]
        public void DeviceMeasurement_Equals_ReturnsFalseWhenValuesDiffer()
        {
            const string sensorType = "temperature";
            var dateTime = new DateTime(2021, 07, 18, 12, 27, 0);
            var leftDm = new DeviceMeasurement(sensorType, dateTime, "value");
            var rightDm = new DeviceMeasurement(sensorType, dateTime, "differentValue");
            
            leftDm.Should().NotBeEquivalentTo(rightDm);
        }
        
        [TestMethod]
        public void DeviceMeasurement_Equals_NullCheck_OK()
        {
            const string sensorType = "temperature";
            const string value = "5";
            var dateTime = new DateTime(2021, 07, 18, 12, 27, 0);
            var deviceMeasurement = new DeviceMeasurement(sensorType, dateTime, value);
            deviceMeasurement.Should().NotBeEquivalentTo((DeviceMeasurement) null);
        }
    }
}