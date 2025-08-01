namespace DigitekShop.Application.Interfaces
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommand<TResult>
    {
    }

    public interface ICommand : ICommand<Unit>
    {
    }

    public record Unit;
} 