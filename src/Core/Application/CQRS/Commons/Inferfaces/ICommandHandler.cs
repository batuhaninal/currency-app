namespace Application.CQRS.Commons.Interfaces
{
    public interface ICommandHandler<TCommand, TResult>
    {
        Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}