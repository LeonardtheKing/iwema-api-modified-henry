using AutoMapper;
using IWema.Application.Contract.Mapping;
using IWema.Application.MenuBars.Command.Add;
using IWema.Application.MenuBars.Command.Delete;
using IWema.Application.MenuBars.Command.Update;
using IWema.Application.MenuBars.Query.GetAll;
using IWema.Application.MenuBars.Query.GetById;
using IWema.Application.News.Command.Add;
using IWema.Application.News.Command.Delete;
using IWema.Application.News.Command.Update;
using IWema.Application.News.Query.GetById;
using IWema.Domain.Entity;

namespace IWema.Application.UnitTests;

public static class MockData
{
    public static AutoMapper.IMapper Mapper => new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
    #region Menu Bar

    public static MenuBar MenuBar => new("Finacle", "http://finacle.wemabank.com", "Finacle environment", "fav-icon");
    public static MenuBar MenuBar_Null => null;
    public static List<MenuBar> MenuBars => [MenuBar, MenuBar, MenuBar, MenuBar];
    public static List<MenuBar> MenuBars_Empty_List => [];

    public static AddMenuBarInputModel AddMenuBarInputModel => new()
    {
        Name = MenuBar.Name,
        Link = MenuBar.Link,
        Description = MenuBar.Description,
        Icon = MenuBar.Icon,
        Hits = MenuBar.Hits
    };

    public static UpdateMenuBarInputModel UpdateMenuBarInputModel => new()
    {

        Name = MenuBar.Name,
        Link = MenuBar.Link,
        Description = MenuBar.Description,

    };

    public static AddMenuBarCommand AddMenuBarCommand = new(MenuBar.Name, MenuBar.Link, MenuBar.Description, MenuBar.Icon, MenuBar.Hits);
    public static DeleteMenuBarCommand DeleteMenuBarCommand = new(MenuBar.Id);
    //public static UpdateMenuBarCommand UpdateMenuBarCommand = new();
    public static GetAllMenuBarQuery GetAllMenuBarQuery = new();
    public static GetMenuBarByIdQuery GetMenuBarByIdQuery = new(MenuBar.Id);
    //  public static UpdateHitsCommand UpdateClickCommand = new();


    public static void DeleteMenuBar(MenuBar menuBarToDelete)
    {
        MenuBars.Remove(menuBarToDelete);
    }


    public static List<MenuBar> GetAll()
    {
        return MenuBars;
    }

    public static MenuBar GetById(Guid id)
    {
        return MenuBars.FirstOrDefault(m => m.Id == id);
    }

    public static bool Add(MenuBar menuBar)
    {
        MenuBars.Add(menuBar);
        return true;
    }

    public static bool Delete(Guid id)
    {
        var menuBarToDelete = MenuBars.FirstOrDefault(m => m.Id == id);
        if (menuBarToDelete != null)
        {
            MenuBars.Remove(menuBarToDelete);
            return true;
        }
        return false;
    }

    public static bool UpdateAsync(Guid id, MenuBar menuBar)
    {
        var existingMenuBar = MenuBars.FirstOrDefault(m => m.Id == id);

        if (existingMenuBar != null)
        {
            Mapper.Map(menuBar, existingMenuBar);
            return true;
        }
        return false;
    }

    #endregion

    #region News

    public static NewsEntity News => new("AFB", "AFB just got bigger");
    public static NewsEntity News_Null => null;
    public static List<NewsEntity> NewsList => [News, News, News, News];
    public static List<NewsEntity> News_List_Is_Zero => [];

    public static AddNewsInputModel AddNewsInputModel => new()
    {
        Title = News.Title,
        Content = News.Content
    };

    public static AddNewsCommand AddNewsCommand = new(News.Title, News.Content);

    public static UpdateNewsCommand UpdateNewsCommand = new(News.Id, News.Title, News.Content, true);
    public static DeleteNewsCommand DeleteNewsCommand = new(Guid.NewGuid());
    public static GetNewsByIdQuery GetNewsByIdQuery = new(Guid.NewGuid());
    #endregion
}
