using IWema.Application.News.Command.Delete;

namespace IWema.Application.UnitTests.NewsScrolls;

public class DeleteNewsTests : NewsBaseTestSetup
{
    private readonly DeleteNewsCommandHandler handler;
    public DeleteNewsTests() => handler = new(NewsRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Delete_NewsScroll_Not_Found_Returns_Error_Message()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(MockData.DeleteNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False);
            Assert.That(expected.Message, Is.EqualTo("News not found."));
        });
    }

    [Test]
    public async Task Delete_NewsScroll_Failed_To_Delete_Returns_Error_Message()
    {
        // Arrange
        NewsRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.News);

        // Act
        var expected = await handler.Handle(MockData.DeleteNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False);
            Assert.That(expected.Message, Is.EqualTo("Failed to delete news."));
        });
    }

    [Test]
    public async Task Delete_News_Deleted_Successfully_Returns_Success()
    {
        // Arrange
        NewsRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.News);
        NewsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var expected = await handler.Handle(MockData.DeleteNewsCommand, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.EqualTo("Deleted"));
        });
    }

}