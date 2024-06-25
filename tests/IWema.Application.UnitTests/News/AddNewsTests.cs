using IWema.Application.News.Command.Add;
using IWema.Domain.Entity;

namespace IWema.Application.UnitTests.NewsScrolls;

public class AddNewsTests : NewsBaseTestSetup
{
    private readonly AddNewsCommandHandler handler;
    public AddNewsTests() => handler = new(NewsRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Add_NewsScroll_Failed_To_Save_Returns_Error_Message()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(MockData.AddNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False);
            Assert.That(expected.Message, Is.EqualTo("Failed to add news."));
        });
    }

    [Test]
    public async Task Add_News_Saved_Successfully_Returns_Success()
    {
        // Arrange
        NewsRepository.Setup(x => x.Add(It.IsAny<NewsEntity>())).ReturnsAsync(true);

        // Act
        var expected = await handler.Handle(MockData.AddNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.EqualTo("Created"));
        });
    }
}
