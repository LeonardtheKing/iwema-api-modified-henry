using IWema.Application.MenuBars.Query.GetById;

namespace IWema.Application.UnitTests.MenuBars;

public class GetMenuBarByIdTests : MenuBarBaseTestSetup
{
    private readonly GetMenuBarByIdQueryHandler handler;
    public GetMenuBarByIdTests() => handler = new(MenuBarRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Get_MenuBar_By_Id_Is_Null_Returns_False()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(MockData.GetMenuBarByIdQuery, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False); // Check if the result is true
            Assert.That(expected.Message, Is.EqualTo("Record not found!"));
        });
    }


    [Test]
    public async Task Get_MenuBar_By_Id_Returns_Success()
    {
        // Arrange
        MenuBarRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.MenuBar);
        MenuBarRepository.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var expected = await handler.Handle(MockData.GetMenuBarByIdQuery, new CancellationToken());
        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.Empty);
        });
    }
}
