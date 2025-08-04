using DigitekShop.Application.Interfaces;
using MediatR;

namespace DigitekShop.Application.Services
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly MediatR.IMediator _mediator;

        public QueryDispatcher(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResult> DispatchAsync<TResult>(MediatR.IRequest<TResult> query)
        {
            return await _mediator.Send(query);
        }
    }
} 