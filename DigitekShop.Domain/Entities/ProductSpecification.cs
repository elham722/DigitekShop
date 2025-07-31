using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities.Common;

namespace DigitekShop.Domain.Entities
{
    public class ProductSpecification : BaseEntity
    {
        public int ProductId { get; private set; }
        public string SpecificationName { get; private set; }
        public string SpecificationValue { get; private set; }
        public string Unit { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsImportant { get; private set; }

        // Navigation Properties
        public Product Product { get; private set; }

        // Constructor
        private ProductSpecification() { }

        public ProductSpecification(int productId, string specificationName, string specificationValue, 
            string unit = "", int displayOrder = 0, bool isImportant = false)
        {
            if (string.IsNullOrWhiteSpace(specificationName))
                throw new ArgumentException("Specification name cannot be empty");

            if (string.IsNullOrWhiteSpace(specificationValue))
                throw new ArgumentException("Specification value cannot be empty");

            ProductId = productId;
            SpecificationName = specificationName.Trim();
            SpecificationValue = specificationValue.Trim();
            Unit = unit?.Trim() ?? "";
            DisplayOrder = displayOrder;
            IsImportant = isImportant;
            SetUpdated();
        }

        // Business Methods
        public void UpdateValue(string newValue, string unit = "")
        {
            if (string.IsNullOrWhiteSpace(newValue))
                throw new ArgumentException("Specification value cannot be empty");

            SpecificationValue = newValue.Trim();
            Unit = unit?.Trim() ?? "";
            SetUpdated();
        }

        public void UpdateDisplayOrder(int order)
        {
            DisplayOrder = order;
            SetUpdated();
        }

        public void MarkAsImportant()
        {
            IsImportant = true;
            SetUpdated();
        }

        public void MarkAsNotImportant()
        {
            IsImportant = false;
            SetUpdated();
        }

        // Business Queries
        public string GetDisplayValue()
        {
            if (string.IsNullOrEmpty(Unit))
                return SpecificationValue;
            return $"{SpecificationValue} {Unit}";
        }

        public string GetFullSpecification()
        {
            return $"{SpecificationName}: {GetDisplayValue()}";
        }
    }
} 