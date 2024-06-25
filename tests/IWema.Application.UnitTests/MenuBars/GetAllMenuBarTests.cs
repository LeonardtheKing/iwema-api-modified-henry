using IWema.Application.MenuBars.Query.GetAll;

namespace IWema.Application.UnitTests.MenuBars;

public class GetAllMenuBarTests : MenuBarBaseTestSetup
{
    private readonly GetAllMenuBarQueryHandler handler;
    public GetAllMenuBarTests() => handler = new(MenuBarRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Get_All_MenuBar_Returns_Success()
    {
        // Arrange
        MenuBarRepository.Setup(x => x.Get()).ReturnsAsync(MockData.MenuBars);

        // Act
        var expected = await handler.Handle(MockData.GetAllMenuBarQuery, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {

            Assert.That(expected.Successful, Is.True); // Check if the result is not null
            Assert.That(expected.Message, Is.Empty);
        });
    }

}