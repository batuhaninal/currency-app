namespace Application.CQRS.Commons
{
    public interface IPipeline<TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next);
    }
}