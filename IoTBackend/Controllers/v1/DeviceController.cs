using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IoTBackend.Application.Queries;
using IoTBackend.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoTBackend.API.Controllers.v1
{
    [Route("/api/v1/devices")]
    public class DeviceController : BaseController
    {
        public DeviceController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get measurements of a device by date.
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="date">Required field to filter measurements by date. Format: YYYY-MM-DD</param>
        /// <param name="sensorType">Optional field to filter measurements by sensor type, i.e. temperature</param>
        /// <returns>List of measurements</returns>
        /// <response code="404">Specified device does not exist</response>
        [ProducesResponseType(typeof(DeviceMeasurementViewModel[]),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("{deviceName}/measurements")]
        public async Task<IActionResult> GetDeviceMeasurementsByDate(string deviceName, [FromQuery, Required] DateTime date, [FromQuery] string sensorType)
        {
            return Ok(await _mediator.Send(new GetDeviceMeasurementsQuery(deviceName, date, sensorType)));
        }
    }
}
