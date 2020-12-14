using NomadAPI.Dtos;
using NomadAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NomadAPI.Interfaces
{
    public interface IReactionsRepository
    {
        Task<IEnumerable<ReactionDto>> GetUsersReacted(int userId);
        Task<UserReaction> GetUserReaction(int reactedUserId, int reactedByUserId);
        Task<AppUser> GetUserWithReactions(int userId);
    }
}
