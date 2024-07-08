using IWema.Application.Common.DTO;
using IWema.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence;
public class IWemaDbContext : IdentityDbContext<ApplicationUser>
{
    public IWemaDbContext(DbContextOptions<IWemaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }

    public DbSet<MenuBar> MenuBars { get; set; }
    public DbSet<NewsEntity> News { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<UpcomingEventEntity> UpcomingEvents { get; set; }
    public DbSet<ManagementTeamEntity> ManagementTeams { get; set; }
    public DbSet<ParentSideMenuEntity> ParentMenuEntities { get; set; }
    public DbSet<ChildSideMenuEntity> ChildMenuEntities { get; set; }
    public DbSet<AnnouncementEntity> Announcements { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<BlogEntity> BlogEntities { get; set; }
    public DbSet<AnnouncementDto> AnnouncementDtos { get; set; }
    public DbSet<UserLoginSession> UserLoginSessions { get; set; }

}
