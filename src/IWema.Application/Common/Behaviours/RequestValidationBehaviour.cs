//using FluentValidation;
//using MediatR;

//namespace IWema.Application.Common.Behaviours;

//public class RequestValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
//    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
//{
//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        if (!validators.Any()) return await next();

//        var context = new ValidationContext<TRequest>(request);
//        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

//        var failures = validationResults.Where(v => v.Errors.Count > 0).SelectMany(v => v.Errors).ToList();
//        if (failures.Count > 0)
//        {
//            var errorMessages = string.Join(" \n", failures.Select(v => v.ErrorMessage));
//            //return (TResponse)Convert.ChangeType(new ServiceResponse(errorMessages, false), typeof(TResponse));
//              return (TResponse)Activator.CreateInstance(typeof(TResponse), errorMessages, false, null);
//        }


//        return await next();
//    }
//}


using FluentValidation;
using IWema.Application.Common.DTO;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IWema.Application.Common.Behaviours;

public class RequestValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<RequestValidationBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
 
        if (!validators.Any())
        {
            var response = await next();
            logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
            return response;
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults.Where(v => v.Errors.Count > 0).SelectMany(v => v.Errors).ToList();
        if (failures.Count > 0)
        {
            var errorMessages = string.Join(" \n", failures.Select(v => v.ErrorMessage));
            logger.LogWarning("Validation failures for {RequestName}: {Failures}", typeof(TRequest).Name, errorMessages);
           return (TResponse)Activator.CreateInstance(typeof(TResponse), errorMessages, false, null);
        }

        var result = await next();
        return result;
    }
}
