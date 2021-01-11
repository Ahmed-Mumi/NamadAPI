using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NomadAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<NomadDto> GetNomadAsync(string email);
        Task<PagedList<NomadDto>> GetNomadsAsync(UserParams userParams);
        void AddReport(Report report);
        Task<ReportDto> GetReport(int id);
        Task<PagedList<ReportDto>> GetReportsAsync(ReportParams reportParams);
        void AddFriendship(Friendship friendship);
        void RemoveFriendship(Friendship friendship);
        Task<Friendship> GetFriendship(int userSentRequestId, int userReceivedRequestId);
    }
}
