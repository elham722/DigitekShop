using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Responses;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitekShop.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IOrderRepository _orderRepository; // اگر نیاز به چک سفارش فعال دارید

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResponse> HandleAsync(DeleteProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var product = await _unitOfWork.Products.GetByIdAsync(command.Id);
                if (product == null)
                    throw new ProductNotFoundException(command.Id);

                // TODO: اگر نیاز به چک سفارش فعال دارید، اینجا اضافه کنید
                // var hasActiveOrders = await _orderRepository.HasActiveOrdersForProductAsync(command.Id);
                // if (hasActiveOrders)
                //     throw new InvalidOperationException("Cannot delete product with active orders");

                await _unitOfWork.Products.DeleteAsync(command.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return ResponseFactory.CreateCommandSuccess("DeleteProduct", "Product deleted successfully");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
} 