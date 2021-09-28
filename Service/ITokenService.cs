using ProjectIkwambe.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ProjectIkwambe.Service
{
    public interface ITokenService
    {
        Task<LoginResult> CreateToken(LoginRequest Login);
        Task<ClaimsPrincipal> GetByValue(string Value);
    }
}
