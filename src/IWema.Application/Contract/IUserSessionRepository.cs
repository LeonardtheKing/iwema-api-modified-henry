using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface IUserSessionRepository
{
    Task AddSessionAsync(UserLoginSession session);
    Task<UserLoginSession> GetLatestSessionByUserIdAsync(string userId);
    Task UpdateSessionAsync(UserLoginSession session);
}
