using MediatR;

namespace DigitekShop.Application.Interfaces
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TResult>(MediatR.IRequest<TResult> query);
    }
} 