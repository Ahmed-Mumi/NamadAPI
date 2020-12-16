using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Helpers;
using NomadAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        //public async Task<bool> SaveAllAsync()
        //{
        //    return await _context.SaveChangesAsync() > 0;
        //}

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<NomadDto> GetNomadAsync(string email)
        {
            return await _context.Users
                .Where(x => x.Email == email)
                .ProjectTo<NomadDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<NomadDto>> GetNomadsAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.Email != userParams.CurrentUserEmail);

            return await PagedList<NomadDto>.CreateAsync(query.ProjectTo<NomadDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                userParams.PageNumber, userParams.PageSize);
        }

        public void AddReport(Report report)
        {
            _context.Reports.Add(report);
        }

        public async Task<ReportDto> GetReport(int id)
        {
            return await _context.Reports
                .Where(x => x.Id == id)
                .ProjectTo<ReportDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<ReportDto>> GetReportsAsync(ReportParams reportParams)
        {
            var query = _context.Reports.AsQueryable();

            query = query.Where(u => u.UserReported.FullName == reportParams.FullName);

            return await PagedList<ReportDto>.CreateAsync(query.ProjectTo<ReportDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                reportParams.PageNumber, reportParams.PageSize);
        }

        public void AddFriendship(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
        }

        public void RemoveFriendship(Friendship friendship)
        {
            _context.Friendships.Remove(friendship);
        }

        public async Task<Friendship> GetFriendship(int userSentRequestId, int userReceivedRequestId)
        {
            return await _context.Friendships.SingleOrDefaultAsync(f => f.UserSentRequestId == userSentRequestId &&
                f.UserReceivedRequestId == userReceivedRequestId);
        }
    }
}
