using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IoTBackend.Application.Queries;
using IoTBackend.Application.Repositories;
using IoTBackend.Application.Validators;
using IoTBackend.Application.ViewModels;
using MediatR;

namespace IoTBackend.Application.Handlers
{
    public class GetDeviceMeasurementsQueryHandler : IRequestHandler<GetDeviceMeasurementsQuery, IEnumerable<DeviceMeasurementViewModel>>
    {
        private readonly IDeviceMeasurementRepository _repository;
        public GetDeviceMeasurementsQueryHandler(IDeviceMeasurementRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<DeviceMeasurementViewModel>> Handle(GetDeviceMeasurementsQuery request, CancellationToken cancellationToken)
        {
            // Could be moved to a base class or handled by a middleware
            var validator = new GetDeviceMeasurementsQueryValidator();
            validator.ValidateAndThrow(request);
            var measurements = string.IsNullOrEmpty(request.SensorType) ?
                await _repository.GetMeasurements(request.DeviceName, request.Date) :
                await _repository.GetMeasurements(request.DeviceName, request.Date, request.SensorType);
            return measurements;            
        }
    }
}
