//using IWema.Domain.Entity;

//namespace IWema.Application.Contract;

//public interface IUserSessionRepository
//{
//    Task AddSessionAsync(UserLoginSession session);
//    Task<UserLoginSession> GetLatestSessionByUserIdAsync(string userId);
//    Task UpdateSessionAsync(UserLoginSession session);
//    Task<List<DateTime>> GetActiveUserLoginTimes();
//}

using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface IUserSessionRepository
{
    Task<List<DateTime>> GetActiveUserLoginTimes();
    Task AddSessionAsync(UserLoginSession session);
    Task<UserLoginSession> GetActiveSessionByUserIdAsync(string userId);
    Task DeactivateSessionAsync(UserLoginSession session);
    Task<UserLoginSession> GetLatestSessionByUserIdAsync(string userId);
    Task UpdateSessionAsync(UserLoginSession session);
    Task<IQueryable<UserLoginSession>> GetQueryableSession();
}
