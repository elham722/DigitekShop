using DigitekShop.Application.Responses;

namespace DigitekShop.Application.Interfaces
{
    /// <summary>
    /// Handler for Commands that return a result
    /// </summary>
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Handler for Commands that don't return a result
    /// </summary>
    public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, CommandResponse> where TCommand : ICommand
    {
    }

    /// <summary>
    /// Base interface for Commands that return a result
    /// </summary>
    public interface ICommand<TResult> : IRequest<TResult>
    {
    }

    /// <summary>
    /// Base interface for Commands that don't return a result
    /// </summary>
    public interface ICommand : ICommand<CommandResponse>
    {
    }
} 