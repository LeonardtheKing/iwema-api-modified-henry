using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IWema.Application.Common.Behaviours
{
    public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<RequestValidationBehaviour<TRequest, TResponse>> _logger;

        public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<RequestValidationBehaviour<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                var response = await next();
                _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
                return response;
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.Where(v => v.Errors.Count > 0).SelectMany(v => v.Errors).ToList();
            if (failures.Count > 0)
            {
                var errorMessages = string.Join("\n", failures.Select(v => v.ErrorMessage));
                _logger.LogWarning("Validation failures for {RequestName}: {Failures}", typeof(TRequest).Name, errorMessages);
                return (TResponse)Activator.CreateInstance(typeof(TResponse), errorMessages, false);
            }

            var result = await next();
            _logger.LogInformation("Successfully handled {RequestName}", typeof(TRequest).Name);
            return result;
        }
    }
}


