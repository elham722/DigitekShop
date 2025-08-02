using MediatR;

namespace DigitekShop.Application.Interfaces
{
    /// <summary>
    /// Base interface for all requests that return a result
    /// This is compatible with MediatR's IRequest<TResult>
    /// </summary>
    public interface IRequest<TResult>
    {
    }

    /// <summary>
    /// Base interface for all requests that don't return a result
    /// This is compatible with MediatR's IRequest
    /// </summary>
    public interface IRequest : IRequest<Unit>
    {
    }
} 