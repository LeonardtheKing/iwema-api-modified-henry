using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;
public class SideMenuRepository(IWemaDbContext context) : ISideMenuRepository
{
    public async Task<bool> Add(ParentSideMenuEntity mainMenu)
    {
        await context.AddAsync(mainMenu);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddSubNenu(ChildSideMenuEntity subMenu)
    {
        await context.AddAsync(subMenu);
        return await context.SaveChangesAsync() > 0;
    }
    public async Task<ParentSideMenuEntity> GetParentSideMenuById(Guid Id)
    {
        ParentSideMenuEntity parentSideMenu = await context.ParentMenuEntities.FindAsync(Id);
        return parentSideMenu;
    }
    public async Task<ChildSideMenuEntity> GetChildSideMenuById(Guid Id)
    {
        ChildSideMenuEntity childToggle = await context.ChildMenuEntities.FindAsync(Id);
        return childToggle;
    }
    public async Task<(ParentSideMenuEntity parentSideMenu, List<ChildSideMenuEntity> childSideMenu)> GetParentSideMenu(Guid id)
    {
        ParentSideMenuEntity parentSideMenu = await context.ParentMenuEntities
                                               .FirstOrDefaultAsync(m => m.Id == id);

        List<ChildSideMenuEntity> childSideMenu = await context.ChildMenuEntities
                                                    .Where(s => s.ParentSideMenuId == id)
                                                    .ToListAsync();

        return (parentSideMenu, childSideMenu);
    }

    public async Task<ChildSideMenuEntity> GetSubMenuById(Guid id)
    {
        return await context.ChildMenuEntities
                             .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> DeleteParentSideMenu(Guid id)
    {
        var entity = await GetParentSideMenuById(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteChildSideMenu(Guid id)
    {
        var entity = await GetChildSideMenuById(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }


    public async Task<List<ChildSideMenuEntity>> GetSubMenu() =>
     await context.ChildMenuEntities
                   .ToListAsync();

    public async Task<bool> UpdateParentSideMenu(ParentSideMenuEntity parentToggle)
    {
        context.ParentMenuEntities.Update(parentToggle);
        return await context.SaveChangesAsync() > 1;
    }

    public async Task<bool> UpdateChildSideMenu(Guid Id, ChildSideMenuEntity childToggle)
    {
        context.ChildMenuEntities.Update(childToggle);
        return await context.SaveChangesAsync() > 1;
    }

    public async Task<bool> AddSubMenu(ChildSideMenuEntity subMenu)
    {
        await context.AddAsync(subMenu);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<ParentSideMenuEntity>> GetAllParentSideMenu() =>
        await context.ParentMenuEntities
                      .ToListAsync();

    public async Task<List<ChildSideMenuEntity>> GetAllChildSideMenu() =>
     await context.ChildMenuEntities
                   .ToListAsync();

    public async Task<List<(ParentSideMenuEntity parentSideMenu, List<ChildSideMenuEntity> childSideMenu)>> GetAllParentMenusWithChildren()
    {
        // Retrieve all parent menus
        List<ParentSideMenuEntity> parentSideMenus = await context.ParentMenuEntities.ToListAsync();

        // Prepare a list to hold the result
        List<(ParentSideMenuEntity parentSideMenu, List<ChildSideMenuEntity> childSideMenu)> result = new List<(ParentSideMenuEntity, List<ChildSideMenuEntity>)>();

        // Loop through each parent and get their children
        foreach (var parentSideMenu in parentSideMenus)
        {
            List<ChildSideMenuEntity> childSideMenus = await context.ChildMenuEntities
                                                           .Where(c => c.ParentSideMenuId == parentSideMenu.Id)
                                                           .ToListAsync();

            result.Add((parentSideMenu, childSideMenus));
        }
        return result;
    }

}

