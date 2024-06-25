using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface ISpotlightRepository
{
    Task<bool> Add(Spotlight news);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<Spotlight>> Get();
    Task<IEnumerable<Spotlight>> GetActive();
    Task<Spotlight> GetById(Guid id);
    Task<bool> Update(Spotlight news);
}
