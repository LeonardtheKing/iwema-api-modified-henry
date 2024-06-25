using IWema.Application.Contract;
using IWema.Application.Contract.Roles;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories
{
    public class ApplicationUserRepository(IWemaDbContext context) : IApplicationUserRepository 
    {
        public async Task<bool> Add(ApplicationUser user)
        {
            await context.AddAsync(user);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<ApplicationUser> GetById(Guid id) =>
            await context.Users.FindAsync(id);

        public async Task<ApplicationUser> GetByEmail(string email)
        {
            email = email.ToLower();
            if(context.Users == null)
                return new ApplicationUser();

            return context.Users.Where(w => w.Email.ToLower() == email).FirstOrDefault();

        }
        public async Task<bool> Delete(Guid id)
        {
            var entity = await GetById(id);
            context.Remove(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(ApplicationUser user)
        {
            context.Users.Update(user);
            return await context.SaveChangesAsync() > 1;
        }

        public async Task<bool> GetRoles(string roleName)
        {
            // Query the database for the specified role name
            var role = await context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);

            // Check if the role exists
            if (role != null)
            {
                // Role found
                return true;
            }
            else
            {
                // Role not found
                return false;
            }
        }

    }
}
