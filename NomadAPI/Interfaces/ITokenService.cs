using NomadAPI.Entities;
using System.Threading.Tasks;

namespace NomadAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
