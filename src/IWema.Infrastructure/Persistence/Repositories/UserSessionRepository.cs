using IWema.Application.Contract;
using IWema.Domain.Entity;
using IWema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly IWemaDbContext _dbContext;

        public UserSessionRepository(IWemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSessionAsync(UserLoginSession session)
        {
            await _dbContext.UserLoginSessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserLoginSession> GetLatestSessionByUserIdAsync(string userId)
        {
            return await _dbContext.UserLoginSessions
                .Where(s => s.UserId == userId && s.LogoutTime == null)
                .OrderByDescending(s => s.LoginTime)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateSessionAsync(UserLoginSession session)
        {
            _dbContext.UserLoginSessions.Update(session);
            await _dbContext.SaveChangesAsync();
        }
    }
}
