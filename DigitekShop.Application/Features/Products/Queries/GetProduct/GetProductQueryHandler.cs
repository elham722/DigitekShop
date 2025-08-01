using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Profiles;
using DigitekShop.Application.Responses;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.Exceptions;
using AutoMapper;

namespace DigitekShop.Application.Features.Products.Queries.GetProduct
{
    public class GetProductQueryHandler : IQueryHandler<GetProductQuery, SuccessResponse<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<ProductDto>> HandleAsync(GetProductQuery query, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(query.Id);
            if (product == null)
                throw new ProductNotFoundException(query.Id);

            var productDto = _mapper.Map<ProductDto>(product);
            return ResponseFactory.CreateSuccess(productDto, "Product retrieved successfully");
        }
    }
} 