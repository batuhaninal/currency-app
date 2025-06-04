using Application.Abstractions.Commons.Results;
using Application.CQRS.Commons.Interfaces;
using Application.Models.DTOs.Commons.Results;
using FluentValidation;

namespace Application.CQRS.Commons.Behaviors
{
    public sealed class ValidationBehaviour<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : ICommand
        where TResponse : IBaseResult
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));
            var errors = failures
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .GroupBy(key=> key.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x=> x.ErrorMessage).ToArray()
                );

            if (errors.Any())
                return (TResponse)(IBaseResult)new ResultDto(400, false, null, "Validation Failed", errors);


            return await next();
        }
    }
}