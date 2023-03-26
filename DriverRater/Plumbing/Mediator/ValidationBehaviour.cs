﻿namespace DriverRater.Plumbing.Mediator;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
{
    private readonly IServiceProvider _serviceProvider;
    public ValidationBehaviour(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(requestType);

        var scope = _serviceProvider.CreateScope();
        var validator = (IValidator)scope.ServiceProvider.GetService(validatorType);
        if (validator == null)
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