using System;
using System.Collections.Generic;
using FluentAssertions;
using IoTBackend.Application.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IoTBackend.Application.Test.ViewModels
{
    [TestClass]
    public class DeviceMeasurementViewModelTests
    {
        [TestMethod]
        public void DeviceMeasurementViewModel_EqualsReturnsTrueForSameValues()
        {
            var expectedDateTime = new DateTime(2021, 07, 18);
            var expectedSensors = new Dictionary<string, object>{ {"test", "1"} };
            var leftVm = new DeviceMeasurementViewModel(expectedDateTime, expectedSensors);
            var rightVm = new DeviceMeasurementViewModel(expectedDateTime, expectedSensors);

            leftVm.Should().BeEquivalentTo(rightVm);
        }
        
        [TestMethod]
        public void DeviceMeasurementViewModel_EqualsReturnsFalseForDifferentSensorValues()
        {
            var leftDateTime = new DateTime(2021, 07, 18);
            var leftSensors = new Dictionary<string, object>{ {"test", "1"} };
            var rightDateTime = new DateTime(2021, 07, 18);
            var rightSensors = new Dictionary<string, object>{ {"test", "2"} };
            var leftVm = new DeviceMeasurementViewModel(leftDateTime, leftSensors);
            var rightVm = new DeviceMeasurementViewModel(rightDateTime, rightSensors);

            leftVm.Should().NotBeEquivalentTo(rightVm);
        }
        
        [TestMethod]
        public void DeviceMeasurementViewModel_Equals_WorksWithNullSensors_OK()
        {
            var expectedDateTime = new DateTime(2021, 07, 18);
            var sensors = new Dictionary<string, object>{ {"test", "1"} };
            var vmWithSensors = new DeviceMeasurementViewModel(expectedDateTime, sensors);
            var vmWithoutSensors = new DeviceMeasurementViewModel(expectedDateTime, null);

            vmWithSensors.Should().NotBeEquivalentTo(vmWithoutSensors);
            vmWithoutSensors.Should().NotBeEquivalentTo(vmWithSensors);
        }
        
        [TestMethod]
        public void DeviceMeasurementViewModel_Init_OK()
        {
            var expectedDateTime = new DateTime(2021, 07, 18);
            var expectedSensors = new Dictionary<string, object>{ {"test", "1"} };
            
            var vm = new DeviceMeasurementViewModel(expectedDateTime, expectedSensors);

            vm.DateTime.Should().BeSameDateAs(expectedDateTime);
            vm.Sensors.Should().BeSameAs(expectedSensors);
        }
    }
}