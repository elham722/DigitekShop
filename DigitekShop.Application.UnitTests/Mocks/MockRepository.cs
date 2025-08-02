using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitekShop.Domain.Interfaces;
using Moq;

namespace DigitekShop.Application.UnitTests.Mocks
{
    public static class MockRepository
    {
        public static Mock<IProductRepository> GetProductRepository()
        {
            return null;
        }
        
    }
}
