using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitekShop.Application.Models.Identity;

namespace DigitekShop.Application.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest  request);
        Task<RegistrationResponse> Register(RegisterationRequest request);
    }
}
