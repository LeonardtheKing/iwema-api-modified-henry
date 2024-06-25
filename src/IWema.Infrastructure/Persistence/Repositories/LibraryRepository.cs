using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories
{
    public class LibraryRepository(IWemaDbContext context) : ILibraryRepository
    {
        public async Task<bool> Add(Library library)
        {
            await context.AddAsync(library);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await GetById(id);
            context.Remove(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<Library>> Get() =>
         await context.Libraries
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        public async Task<Library> GetById(Guid id) =>
             await context.Libraries.FindAsync(id);
    }
}
