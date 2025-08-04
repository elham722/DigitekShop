using DigitekShop.Application.Interfaces;
using MediatR;

namespace DigitekShop.Application.Services
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly MediatR.IMediator _mediator;

        public CommandDispatcher(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResult> DispatchAsync<TResult>(MediatR.IRequest<TResult> command)
        {
            return await _mediator.Send(command);
        }

        public async Task DispatchAsync(MediatR.IRequest command)
        {
            await _mediator.Send(command);
        }
    }
} 