using AutoMapper;
using IWema.Application.Announcements.Query.GetAllAnnouncements;
using IWema.Application.ManagementTeam.Command.Add;
using IWema.Application.MenuBars.Command.Add;
using IWema.Application.MenuBars.Command.Update;
using IWema.Application.MenuBars.Query.GetFavourite;
using IWema.Application.SideMenu.ParentSideMenu.Query.GetParentSideMenu;
using IWema.Domain.Entity;

namespace IWema.Application.Contract.Mapping;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<UpdateMenuBarCommand, MenuBar>().ReverseMap();
        CreateMap<AddMenuBarInputModel, MenuBar>();
        CreateMap<UpdateHitsCommand, MenuBar>();
        CreateMap<MenuBar, GetFavoriteOutputModel>();
        CreateMap<AddManagementTeamCommandInputModel, ManagementTeamEntity>();
        CreateMap<AddManagementTeamCommandInputModel, AddManagementTeamCommand>();

        CreateMap<AnnouncementEntity, GetAllAnnouncementsOutputModel>()
            .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link ?? ""));

    }

}
