using Microsoft.Extensions.DependencyInjection;
using DigitekShop.Application.Interfaces;

namespace DigitekShop.Application.Services
{
    /// <summary>
    /// Custom implementation of Mediator using DI Container
    /// This can be replaced with MediatR implementation later
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new InvalidOperationException($"No handler found for command {command.GetType().Name}");

            var method = handlerType.GetMethod("HandleAsync");
            var result = await (Task<TResult>)method!.Invoke(handler, new object[] { command, cancellationToken })!;

            return result;
        }

        public async Task Send(ICommand command, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new InvalidOperationException($"No handler found for command {command.GetType().Name}");

            var method = handlerType.GetMethod("HandleAsync");
            await (Task)method!.Invoke(handler, new object[] { command, cancellationToken })!;
        }

        public async Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new InvalidOperationException($"No handler found for query {query.GetType().Name}");

            var method = handlerType.GetMethod("HandleAsync");
            var result = await (Task<TResult>)method!.Invoke(handler, new object[] { query, cancellationToken })!;

            return result;
        }
    }
} 