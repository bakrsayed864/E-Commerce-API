using Own_Service.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Own_Service.Services
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateToken(ApplicationUser user);
    }
}
