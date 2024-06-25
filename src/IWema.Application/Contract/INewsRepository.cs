using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface INewsRepository
{
    Task<bool> Add(NewsEntity news);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<NewsEntity>> Get();
    Task<IEnumerable<NewsEntity>> GetActive();
    Task<NewsEntity> GetById(Guid id);
    Task<bool> Update(NewsEntity news);
}
