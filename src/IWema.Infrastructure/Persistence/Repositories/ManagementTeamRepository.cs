using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;

public class ManagementTeamRepository(IWemaDbContext context) : IManagementTeamRepository
{
    public async  Task<bool> Add(ManagementTeamEntity managementTeam)
    {
        await context.AddAsync(managementTeam);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<ManagementTeamEntity> GetById(Guid id) =>
      await context.ManagementTeams.FindAsync(id);

    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async  Task<List<ManagementTeamEntity>> Get()=>
        await context.ManagementTeams.ToListAsync();
    
    //public async Task<bool> Update(ManagementTeamEntity managementTeam)
    //{
    //    context.ManagementTeams.Update(managementTeam);
    //    return await context.SaveChangesAsync() > 1;
    //}

    public async Task<int> UpdateAsync(Guid id, ManagementTeamEntity managementTeam)
    {
        var updatedManagementTeam = await context
            .ManagementTeams
            .Where(_x => _x.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(s => s.Quote, managementTeam.Quote)
            .SetProperty(s => s.ImageLocation, managementTeam.ImageLocation)
            .SetProperty(s=>s.ImageName, managementTeam.ImageName)
            .SetProperty(s=>s.NameOfExecutive, managementTeam.NameOfExecutive)
            .SetProperty(s=>s.Position, managementTeam.Position)
            .SetProperty(s=>s.ProfileLink, managementTeam.ProfileLink)
            );
        return updatedManagementTeam;
    }
}



