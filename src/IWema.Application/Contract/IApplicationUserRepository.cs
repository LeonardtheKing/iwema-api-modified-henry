using IWema.Domain.Entity;

namespace IWema.Application.Contract
{
    public interface IApplicationUserRepository
    {
        Task<bool> Add(ApplicationUser user);
        Task<bool> Update(ApplicationUser user);
        Task<ApplicationUser> GetById(Guid id);
        Task<ApplicationUser> GetByEmail(string email);
    }
}
