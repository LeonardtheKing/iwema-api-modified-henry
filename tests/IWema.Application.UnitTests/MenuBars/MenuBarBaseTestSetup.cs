using AutoMapper;
using IWema.Application.Contract;
using IWema.Domain.Entity;

namespace IWema.Application.UnitTests.MenuBars;

public abstract class MenuBarBaseTestSetup
{
    public Mock<IMenuBarRepository> MenuBarRepository = new();
    public Mock<IMapper> Mapper = new();

    public void BaseSetup()
    {
        // Menu Bar Repository
        MenuBarRepository.Setup(x => x.Add(It.IsAny<MenuBar>())).ReturnsAsync(false);
        MenuBarRepository.Setup(x => x.Update(It.IsAny<MenuBar>())).ReturnsAsync(false);
        MenuBarRepository.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(false);
        MenuBarRepository.Setup(x => x.Get()).ReturnsAsync(MockData.MenuBars_Empty_List);
        MenuBarRepository.Setup(x => x.GetTop(It.IsAny<int>())).ReturnsAsync(MockData.MenuBars_Empty_List);
        MenuBarRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.MenuBar_Null);
    }
}
