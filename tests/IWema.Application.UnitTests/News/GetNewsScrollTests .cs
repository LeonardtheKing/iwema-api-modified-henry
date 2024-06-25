using IWema.Application.News.Query.GetScroll;

namespace IWema.Application.UnitTests.NewsScrolls;

public class GetNewsScrollTests : NewsBaseTestSetup
{
    private readonly GetNewsScrollQueryHandler handler;
    public GetNewsScrollTests() => handler = new(NewsRepository.Object);

    [SetUp] public void Setup() => BaseSetup();

    [Test]
    public async Task Get_News_By_Scroll_No_News()
    {
        // Arrange

        // Act
        var expected = await handler.Handle(new GetNewsScrollQuery(), new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.Empty);
            Assert.That(expected.Response.Scroll, Is.Empty);
        });
    }

    [Test]
    public async Task Get_News_By_Scroll_Returns_News()
    {
        // Arrange
        NewsRepository.Setup(x => x.GetActive()).ReturnsAsync(MockData.NewsList);

        // Act
        var expected = await handler.Handle(new GetNewsScrollQuery(), new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(expected.Successful, Is.True);
            Assert.That(expected.Message, Is.Empty);
            Assert.That(expected.Response.Scroll, Is.Not.Empty);
        });
    }

}