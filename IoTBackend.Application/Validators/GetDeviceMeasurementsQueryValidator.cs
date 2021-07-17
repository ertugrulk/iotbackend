using System;
using FluentValidation;
using IoTBackend.Application.Queries;

namespace IoTBackend.Application.Validators
{
    public class GetDeviceMeasurementsQueryValidator : AbstractValidator<GetDeviceMeasurementsQuery>
    {
        public GetDeviceMeasurementsQueryValidator()
        {
            RuleFor(q => q.DeviceName)
                .NotNull()
                .NotEmpty();
            RuleFor(q => q.Date)
                .GreaterThan(DateTime.MinValue)
                .LessThanOrEqualTo(DateTime.Now);
        }
    }
}
