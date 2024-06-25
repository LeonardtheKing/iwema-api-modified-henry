using IWema.Application.News.Query.GetById;

namespace IWema.Application.UnitTests.NewsScrolls;

public class GetNewsByIdTests : NewsBaseTestSetup
{
    private readonly GetNewsByIdQueryHandler handler;
    public GetNewsByIdTests() => handler = new(NewsRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Get_News_By_Id_IsNull_Returns_Failed()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(MockData.GetNewsByIdQuery, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.False);
            Assert.That(expected.Message, Is.EqualTo("News not found."));
            Assert.That(expected.Response, Is.Null);
        });
    }

    [Test]
    public async Task Get_News_By_Id_Successfully_Returns_Success()
    {
        // Arrange
        NewsRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.News);

        // Act
        var expected = await handler.Handle(MockData.GetNewsByIdQuery, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.Empty);
            Assert.That(expected.Response.Title, Is.EqualTo(MockData.News.Title));
            Assert.That(expected.Response.Content, Is.EqualTo(MockData.News.Content));
        });
    }

}