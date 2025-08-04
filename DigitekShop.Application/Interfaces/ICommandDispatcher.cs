using MediatR;

namespace DigitekShop.Application.Interfaces
{
    public interface ICommandDispatcher
    {
        Task<TResult> DispatchAsync<TResult>(MediatR.IRequest<TResult> command);
        Task DispatchAsync(MediatR.IRequest command);
    }
} 