using IWema.Application.MenuBars.Command.Add;
using IWema.Domain.Entity;

namespace IWema.Application.UnitTests.MenuBars;

public class AddMenuBarTests : MenuBarBaseTestSetup
{
    private readonly AddMenuBarCommandHandler handler;
    

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Add_MenuBar_Failed_To_Save_Returns_Error_Message()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(MockData.AddMenuBarCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False);
            Assert.That(expected.Message, Is.EqualTo("Failed to add menu."));
        });
    }

    [Test]
    public async Task Add_MenuBar_Saved_Successfully_Returns_Success()
    {
        // Arrange
        MenuBarRepository.Setup(x => x.Add(It.IsAny<MenuBar>())).ReturnsAsync(true);

        // Act
        var expected = await handler.Handle(MockData.AddMenuBarCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.EqualTo("menu successfully added."));
        });
    }

}