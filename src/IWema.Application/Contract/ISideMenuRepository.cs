using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface ISideMenuRepository
{
    Task<List<ChildSideMenuEntity>> GetAllChildSideMenu();
    Task<bool> Add(ParentSideMenuEntity mainMenu);
    Task<bool> AddSubNenu(ChildSideMenuEntity subMenu);
    Task<ParentSideMenuEntity> GetParentSideMenuById(Guid Id);
    Task<(ParentSideMenuEntity parentSideMenu, List<ChildSideMenuEntity> childSideMenu)> GetParentSideMenu(Guid id);

    Task<ChildSideMenuEntity> GetChildSideMenuById(Guid Id);
    Task<bool> UpdateChildSideMenu(Guid Id, ChildSideMenuEntity childSideMenu);
    Task<ChildSideMenuEntity> GetSubMenuById(Guid id);
    Task<bool> UpdateParentSideMenu(ParentSideMenuEntity parentToggle);
    Task<bool> AddSubMenu(ChildSideMenuEntity subMenu);
    Task<bool> DeleteParentSideMenu(Guid id);
    Task<bool> DeleteChildSideMenu(Guid id);
    Task<List<ChildSideMenuEntity>> GetSubMenu();
    Task<List<ParentSideMenuEntity>> GetAllParentSideMenu();
    Task<List<(ParentSideMenuEntity parentSideMenu, List<ChildSideMenuEntity> childSideMenu)>> GetAllParentMenusWithChildren();
}
