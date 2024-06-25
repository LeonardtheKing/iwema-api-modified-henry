using IWema.Application.News.Command.Update;
using IWema.Domain.Entity;

namespace IWema.Application.UnitTests.NewsScrolls;

public class UpdateNewsTests : NewsBaseTestSetup
{
    private readonly UpdateNewsCommandHandler handler;
    public UpdateNewsTests() => handler = new(NewsRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Update_NewsScroll_Updated_New_Not_Does_Exist_Returns_Error_Message()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(MockData.UpdateNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False);
            Assert.That(expected.Message, Is.EqualTo("News not found."));
        });
    }

    [Test]
    public async Task Update_NewsScroll_Failed_To_Update_Returns_Error_Message()
    {
        // Arrange
        NewsRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.News);
        NewsRepository.Setup(x => x.Update(It.IsAny<NewsEntity>())).ReturnsAsync(false);

        // Act
        var expected = await handler.Handle(MockData.UpdateNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False);
            Assert.That(expected.Message, Is.EqualTo("Failed to update news."));
        });
    }

    [Test]
    public async Task Update_News_Updated_Successfully_Returns_Success()
    {
        // Arrange
        NewsRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.News);
        NewsRepository.Setup(x => x.Update(It.IsAny<NewsEntity>())).ReturnsAsync(true);

        // Act
        var expected = await handler.Handle(MockData.UpdateNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.EqualTo("Updated"));
        });
    }

}