using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;

//internal class BlogRepository
//{
//}

public class BlogRepository(IWemaDbContext context) : IBlogRepository
{
    public async Task<bool> Add(BlogEntity blog)
    {
        await context.AddAsync(blog);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<BlogEntity> GetById(Guid id) =>
      await context.BlogEntities.FindAsync(id);

    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<BlogEntity>> Get() =>
        await context.BlogEntities.ToListAsync();

    //public async Task<bool> Update(ManagementTeamEntity managementTeam)
    //{
    //    context.ManagementTeams.Update(managementTeam);
    //    return await context.SaveChangesAsync() > 1;
    //}

    public async Task<int> UpdateAsync(Guid id, BlogEntity blog)
    {
        var blogEntity = await context
            .BlogEntities
            .Where(_x => _x.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(s => s.ImageLocation, blog.ImageLocation)
            .SetProperty(s => s.Title, blog.Title)
            .SetProperty(s => s.Summary, blog.Summary)
            .SetProperty(s => s.ReadMoreLink, blog.ReadMoreLink)
            );
        return blogEntity;
    }
}




