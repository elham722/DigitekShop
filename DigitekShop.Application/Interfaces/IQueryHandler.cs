using MediatR;

namespace DigitekShop.Application.Interfaces
{
    /// <summary>
    /// Handler for Queries that return a result
    /// </summary>
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Base interface for Queries that return a result
    /// </summary>
    public interface IQuery<TResult> : MediatR.IRequest<TResult>
    {
    }
} 