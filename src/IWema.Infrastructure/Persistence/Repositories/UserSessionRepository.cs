//using IWema.Application.Contract;
//using IWema.Domain.Entity;
//using IWema.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;

//namespace IWema.Infrastructure.Repositories
//{
//    public class UserSessionRepository : IUserSessionRepository
//    {
//        private readonly IWemaDbContext _dbContext;

//        public UserSessionRepository(IWemaDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task AddSessionAsync(UserLoginSession session)
//        {
//            await _dbContext.UserLoginSessions.AddAsync(session);
//            await _dbContext.SaveChangesAsync();
//        }

//        public async Task<UserLoginSession> GetLatestSessionByUserIdAsync(string userId)
//        {
//            return await _dbContext.UserLoginSessions
//                .Where(s => s.UserId == userId && s.LogoutTime == null)
//                .OrderByDescending(s => s.LoginTime)
//                .FirstOrDefaultAsync();
//        }

//        public async Task UpdateSessionAsync(UserLoginSession session)
//        {
//            _dbContext.UserLoginSessions.Update(session);
//            await _dbContext.SaveChangesAsync();
//        }


//        public async Task<List<DateTime>> GetActiveUserLoginTimes()
//        {
//            return await _dbContext.UserLoginSessions
//                                 .Where(u => !u.LogoutTime.HasValue)
//                                 .Select(u => u.LoginTime)
//                                 .ToListAsync();
//        }
//    }
//}

using IWema.Application.Contract;
using IWema.Domain.Entity;
using IWema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Repositories
{
    public class UserSessionRepository(IWemaDbContext context) : IUserSessionRepository
    {
        public async Task<List<DateTime>> GetActiveUserLoginTimes()
        {
            return await context.UserLoginSessions
                                 .Where(uls => uls.IsActive)
                                 .Select(uls => uls.LoginTime)
                                 .ToListAsync();
        }

        public async Task AddSessionAsync(UserLoginSession session)
        {
            await context.UserLoginSessions.AddAsync(session);
            await context.SaveChangesAsync();
        }

        public async Task<UserLoginSession> GetActiveSessionByUserIdAsync(string userId)
        {
            return await context.UserLoginSessions
                                 .Where(uls => uls.UserId == userId && uls.IsActive)
                                 .FirstOrDefaultAsync();
        }

        public async Task DeactivateSessionAsync(UserLoginSession session)
        {
             session.UpdateIsActive(false);
            context.UserLoginSessions.Update(session);
            await context.SaveChangesAsync();
        }

        public async Task<UserLoginSession> GetLatestSessionByUserIdAsync(string userId)
        {
            return await context.UserLoginSessions
                .Where(s => s.UserId == userId && s.LogoutTime == null)
                .OrderByDescending(s => s.LoginTime)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateSessionAsync(UserLoginSession session)
        {
            context.UserLoginSessions.Update(session);
            await context.SaveChangesAsync();
        }


        public async Task<IQueryable<UserLoginSession>> GetQueryableSession()
        {
            return context.UserLoginSessions.AsQueryable();
        }
    }
}
