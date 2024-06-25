using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;

public class BannerRepository(IWemaDbContext context) : IBannerRepository
{
    public async Task<bool> Add(Banner banner)
    {
        await context.AddAsync(banner);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<Banner>> Get() =>
     await context.Banners
        .AsNoTracking()
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync();

    public async Task<Banner> GetById(Guid id) =>
         await context.Banners.FindAsync(id);

    public async Task<bool> Update(Banner banner)
    {
        context.Update(banner);
        return await context.SaveChangesAsync() > 0;
    }

}
