using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface IBlogRepository
{

    Task<bool> Add(BlogEntity managementTeam);
    Task<bool> Delete(Guid id);
    Task<List<BlogEntity>> Get();
    Task<BlogEntity> GetById(Guid id);
    // Task<bool> Update(Domain.Entity.ManagementTeamEntity managementTeam);
    Task<int> UpdateAsync(Guid id, BlogEntity managementTeam);
}
