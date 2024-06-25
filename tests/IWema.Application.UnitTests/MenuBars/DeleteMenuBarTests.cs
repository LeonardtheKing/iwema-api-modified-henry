using IWema.Application.MenuBars.Command.Delete;

namespace IWema.Application.UnitTests.MenuBars;

public class DeleteMenuBarTests : MenuBarBaseTestSetup
{
    private readonly DeleteMenuBarCommandHandler handler;
    public DeleteMenuBarTests() => handler = new(MenuBarRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Delete_MenuBar_Failed_To_Delete_Returns_False()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(MockData.DeleteMenuBarCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False); // Check if the result is false
            Assert.That(expected.Message, Is.EqualTo("Menu Bar not found."));
        });
    }

    [Test]
    public async Task Delete_MenuBar_Deleted_Successfully_Returns_Success()
    {
        // Arrange
        MenuBarRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.MenuBar);
        MenuBarRepository.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var expected = await handler.Handle(MockData.DeleteMenuBarCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True); // Check if the result is true
            Assert.That(expected.Message, Is.EqualTo("Menu Bar deleted successfully."));
        });
    }
}
