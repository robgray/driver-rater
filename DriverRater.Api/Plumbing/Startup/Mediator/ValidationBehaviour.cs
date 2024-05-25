namespace DriverRater.Api.Plumbing.Startup.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public class ValidationBehaviour<TRequest, TResponse>(IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(requestType);

        var scope = serviceProvider.CreateScope();
        if (scope.ServiceProvider.GetService(validatorType) is not IValidator validator)
        {
            return await next();
        }

        var result = await validator.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken);
        if (result.IsValid)
        {
            return await next();
        }

        throw new ValidationException(result.Errors);
    }
}