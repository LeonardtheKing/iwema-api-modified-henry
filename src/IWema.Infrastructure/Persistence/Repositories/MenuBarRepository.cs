using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;

public class MenuBarRepository(IWemaDbContext context) : IMenuBarRepository
{
    public async Task<List<MenuBar>> Get() =>
        await context.MenuBars.ToListAsync();

    public async Task<List<MenuBar>> GetTop(int count) =>
        await context.MenuBars
            .OrderByDescending(c => c.Hits)
            .Take(count).ToListAsync();

    public async Task<MenuBar> GetById(Guid id) =>
        await context.MenuBars.FindAsync(id);

    public async Task<bool> Add(MenuBar menuBar)
    {
        await context.AddAsync(menuBar);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Update(MenuBar menuBar)
    {
        context.MenuBars.Update(menuBar);
        return await context.SaveChangesAsync() > 1;
    }

    public async Task<int> UpdateAsync(Guid id, MenuBar menuBar)
    {
        var updatedMenuBar = await context
            .MenuBars
            .Where(_x => _x.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(s => s.Name, menuBar.Name)
            .SetProperty(s => s.Description, menuBar.Description)
            .SetProperty(s => s.Link, menuBar.Link)
            );
        return updatedMenuBar;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }


}
