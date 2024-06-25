using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface IMenuBarRepository
{
    Task<bool> Add(MenuBar menuBar);
    Task<bool> Delete(Guid id);
    Task<List<MenuBar>> Get();
    Task<MenuBar> GetById(Guid id);
    Task<List<MenuBar>> GetTop(int count);
    Task<int> UpdateAsync(Guid id, MenuBar menuBar);
    Task<bool> Update(MenuBar menuBar);
}
