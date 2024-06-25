using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;

public class NewsRepository(IWemaDbContext context) : INewsRepository
{
    public async Task<IEnumerable<NewsEntity>> Get() =>
         await context.News
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<NewsEntity>> GetActive() =>
        await context.News
            .Where(x => x.IsActive)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

    public async Task<NewsEntity?> GetById(Guid id) =>
        await context.News.FindAsync(id);

    public async Task<bool> Add(NewsEntity news)
    {
        await context.AddAsync(news);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(NewsEntity news)
    {
        context.Update(news);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await context.News.FindAsync(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }
}
