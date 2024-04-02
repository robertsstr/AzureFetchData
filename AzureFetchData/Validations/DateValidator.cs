using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;

namespace AzureFetchData.Validations
{
    public class DateValidator : AbstractValidator<HttpRequest>
    {
        public DateValidator()
        {
            RuleFor(req => req.Query["from"]).NotEmpty()
                .Must((request, from) => BeValidDateTime(from))
                .WithMessage("From date is not valid");
            RuleFor(req => req.Query["to"]).NotEmpty()
                .Must((request, to) => BeValidDateTime(to))
                .WithMessage("To date is not valid");
            RuleFor(req => req.Query).Custom((dates, context) =>
            {
                if (!IsValidDateRange(dates["from"], dates["to"]))
                {
                    context.AddFailure("From date must be lower than To date");
                }
            });
        }

        private static bool BeValidDateTime(string value)
        {
            return DateTime.TryParse(value, out _);
        }

        private static bool IsValidDateRange(string fromDate, string toDate)
        {
            if (!DateTime.TryParse(fromDate, out var parsedFromDate))
            {
                return false;
            }

            if (!DateTime.TryParse(toDate, out var parsedToDate))
            {
                return false;
            }

            return parsedFromDate < parsedToDate;
        }
    }
    
}
