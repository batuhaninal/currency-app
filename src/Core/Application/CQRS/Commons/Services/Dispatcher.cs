using Application.Abstractions.Commons.Results;
using Application.CQRS.Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Commons.Services
{
    public sealed class Dispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Dispatcher> _logger;

        public Dispatcher(IServiceProvider serviceProvider, ILogger<Dispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand
            where TResult : IBaseResult
        {
            _logger.LogInformation("Handling command {CommandType} at {Time}", typeof(TCommand).Name, DateTime.UtcNow);

            try
            {
                var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
                var pipelines = _serviceProvider.GetServices<IPipeline<TCommand, TResult>>().ToList();

                // Pipeline zinciri oluşturuluyor
                Func<Task<TResult>> pipelineChain = () => handler.Handle(command, cancellationToken);
                foreach (var pipeline in pipelines.AsEnumerable().Reverse()) // ters sirayla calismali
                {
                    var next = pipelineChain;
                    pipelineChain = () => pipeline.HandleAsync(command, cancellationToken, next);
                }

                var result = await pipelineChain();
                _logger.LogInformation("Handled command {CommandType} successfully", typeof(TCommand).Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling command {CommandType}", typeof(TCommand).Name);
                throw;
            }
        }

        public async Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : IQuery
        {
            _logger.LogInformation("Handling query {QueryType} at {Time}", typeof(TQuery).Name, DateTime.UtcNow);

            try
            {
                var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
                var pipelines = _serviceProvider.GetServices<IPipeline<TQuery, TResult>>().ToList();

                Func<Task<TResult>> pipelineChain = () => handler.Handle(query, cancellationToken);
                foreach (var pipeline in pipelines.AsEnumerable().Reverse())
                {
                    var next = pipelineChain;
                    pipelineChain = () => pipeline.HandleAsync(query, cancellationToken, next);
                }

                var result = await pipelineChain();
                _logger.LogInformation("Handled query {QueryType} successfully", typeof(TQuery).Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling query {QueryType}", typeof(TQuery).Name);
                throw;
            }
        }
    }
}