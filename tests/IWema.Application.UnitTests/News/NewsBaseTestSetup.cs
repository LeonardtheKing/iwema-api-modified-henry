using IWema.Application.Contract;
using IWema.Domain.Entity;

namespace IWema.Application.UnitTests.NewsScrolls;

public abstract class NewsBaseTestSetup
{
    public Mock<INewsRepository> NewsRepository = new();

    public void BaseSetup()
    {
        // News Bar Repository
        NewsRepository.Setup(x => x.Add(It.IsAny<NewsEntity>())).ReturnsAsync(false);
        NewsRepository.Setup(x => x.Update(It.IsAny<NewsEntity>())).ReturnsAsync(false);
        NewsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(false);
        NewsRepository.Setup(x => x.Get()).ReturnsAsync(MockData.News_List_Is_Zero);
        NewsRepository.Setup(x => x.GetActive()).ReturnsAsync(MockData.News_List_Is_Zero);
        NewsRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(MockData.News_Null);
    }
}
