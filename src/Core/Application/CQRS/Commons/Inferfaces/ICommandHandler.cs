using Application.Abstractions.Commons.Results;

namespace Application.CQRS.Commons.Interfaces
{
    public interface ICommandHandler<TCommand, TResult>
        where TResult : IBaseResult
    {
        Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}