namespace DigitekShop.Application.Interfaces
{
    /// <summary>
    /// Mediator interface for sending Commands and Queries
    /// This can be implemented with MediatR or custom implementation
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Sends a command and returns the result
        /// </summary>
        Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a command that doesn't return a result
        /// </summary>
        Task Send(ICommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a query and returns the result
        /// </summary>
        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
} 