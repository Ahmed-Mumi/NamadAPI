using Microsoft.EntityFrameworkCore;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Data
{
    public class ReactionRepository : IReactionsRepository
    {
        private readonly DataContext _context;

        public ReactionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserReaction> GetUserReaction(int reactedUserId, int reactedByUserId)
        {
            return await _context.UserReactions.FindAsync(reactedByUserId, reactedUserId);
        }

        public async Task<IEnumerable<ReactionDto>> GetUsersReacted(int userId)
        {
            var reactions = _context.UserReactions.AsQueryable();

            reactions = reactions.Where(reaction => reaction.ReactedByUserId == userId);

            var users = reactions.Select(reaction => reaction.ReactedByUser);

            return await users.Select(user => new ReactionDto
            {
                FullName = user.FullName,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                Id = user.Id
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserWithReactions(int userId)
        {
            return await _context.Users
                .Include(x => x.ReactedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
