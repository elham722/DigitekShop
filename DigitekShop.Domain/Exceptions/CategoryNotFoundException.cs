using System;

namespace DigitekShop.Domain.Exceptions
{
    public class CategoryNotFoundException : DomainException
    {
        public int CategoryId { get; }

        public CategoryNotFoundException(int categoryId) 
            : base($"Category with ID {categoryId} was not found.")
        {
            CategoryId = categoryId;
        }

        public CategoryNotFoundException(string categoryName) 
            : base($"Category with name '{categoryName}' was not found.")
        {
        }
    }
} 