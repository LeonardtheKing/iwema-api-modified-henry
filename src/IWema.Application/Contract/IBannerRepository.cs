using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface IBannerRepository
{
    Task<bool> Add(Banner banner);
    Task<bool> Delete(Guid id);
    Task<List<Banner>> Get();
    Task<Banner> GetById(Guid id);
    Task<bool> Update(Banner banner);
}