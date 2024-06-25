namespace IWema.Application.Contract;

public interface IManagementTeamRepository
{
    Task<bool> Add(Domain.Entity.ManagementTeamEntity managementTeam);
    Task<bool> Delete(Guid id);
    Task<List<Domain.Entity.ManagementTeamEntity>> Get();
    Task<Domain.Entity.ManagementTeamEntity> GetById(Guid id);
    // Task<bool> Update(Domain.Entity.ManagementTeamEntity managementTeam);
    Task<int> UpdateAsync(Guid id, Domain.Entity.ManagementTeamEntity managementTeam);

}
