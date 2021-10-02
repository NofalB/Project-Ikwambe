using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ITokenService
    {
        Task<LoginResult> CreateToken(LoginRequest Login);
        Task<ClaimsPrincipal> GetByValue(string Value);
    }
}
