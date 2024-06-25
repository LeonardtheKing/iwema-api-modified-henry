using IWema.Application.Contract;
using IWema.Domain.Entity;
using Moq;

namespace IWema.Application.UnitTests;

[TestFixture]
public abstract class BaseTestSetup
{
    public Mock<IMenuBarRepository> MenuBarRepository = new();

    public void BaseSetup()
    {
        // Menu Bar Repository
        MenuBarRepository.Setup(x => x.Add(It.IsAny<MenuBar>())).ReturnsAsync(true);
      //  MenuBarRepository.Setup(x => x.UpdateCountAsync(It.IsAny<Guid>(), It.IsAny<MenuBar>())).ReturnsAsync(1);
        MenuBarRepository.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<MenuBar>())).ReturnsAsync(1);
       // MenuBarRepository.Setup(x => x.UpdateAsync(It.IsAny<MenuBar>())).ReturnsAsync(true);
        MenuBarRepository.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(true);
        MenuBarRepository.Setup(x => x.Get()).ReturnsAsync(MockData.MenuBars);
        MenuBarRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.MenuBar);

    }
}
