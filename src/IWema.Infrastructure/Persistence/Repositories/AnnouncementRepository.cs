using IWema.Application.Announcements.Query.GetAllAnnouncements;
using IWema.Application.Announcements.Query.GetAnnouncementById;
using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;

public class AnnouncementRepository(IWemaDbContext context) : IAnnouncementRepository
{
    public async Task<bool> Add(AnnouncementEntity announcements)
    {
        await context.AddAsync(announcements);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<AnnouncementEntity> GetById(Guid id)
    {
        var announcement = await context.Announcements
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(a=>a.Id==id);
        return announcement;
    }


    public async Task<bool> DeleteAnnouncement(Guid announcementId)
    {
        // SQL query to delete an announcement by its Id
        var sqlQuery = @"
        DELETE FROM Announcements
        WHERE Id = {0}";

        // Execute the delete query
        var rowsAffected = await context.Database.ExecuteSqlRawAsync(sqlQuery, announcementId);

        // If rowsAffected is greater than 0, the delete was successful
        return rowsAffected > 0;
    }

    public async Task<List<AnnouncementEntity>> Get()
    {
        var announcements = await context.Announcements
                                     .AsNoTracking()
                                     .ToListAsync();
        return announcements;
    }


    public async Task<GetAnnouncementByIdOutputModel> GetSingleAnnouncement(Guid announcementId)
    {
        var sqlQuery = @"
            SELECT 
                Id,
                Title,
                Date,
                ImageLocation,
                Content,
                COALESCE(Link, '') AS Link
                
            FROM Announcements
            WHERE Id = {0}";

        var result = await context.AnnouncementDtos
            .FromSqlRaw(sqlQuery, announcementId)
            .SingleOrDefaultAsync();


        var output = new GetAnnouncementByIdOutputModel
        {
            AnnouncementId = result.Id,
            Title = result.Title,
            Date = result.Date,
            ImageLocation = result.ImageLocation,
            Content = result.Content,
            Link = result.Link,  // Already handled null in the query
        };

        return output;
    }

    public async Task<List<GetAllAnnouncementsOutputModel>> GetAllAnnouncements()
    {
        var sqlQuery = @"
            SELECT 
                Id,
                Title,
                Date,
                ImageLocation,
                Content,
                COALESCE(Link, '') AS Link
            FROM Announcements";

        var result = await context.AnnouncementDtos
            .FromSqlRaw(sqlQuery)
            .ToListAsync();

        var output = result.Select(x => new GetAllAnnouncementsOutputModel
        {
            AnnouncementId = x.Id,
            Title = x.Title,
            Date = x.Date,
            ImageLocation = x.ImageLocation,
            Content = x.Content,
            Link = x.Link ?? "",  // Handle null Link values
        }).ToList();

        return output;
    }

    public async Task<bool> Update(AnnouncementEntity announcements)
    {
        context.Announcements.Update(announcements);
        return await context.SaveChangesAsync() > 1;
    }

    public async Task<int> UpdateAsync(Guid id, AnnouncementEntity announcement)
    {
        var updatedAnnouncements = await context
            .Announcements
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(s => s.Title, announcement.Title)
            .SetProperty(s => s.Date, announcement.Date)
            .SetProperty(s => s.ImageLocation, announcement.ImageLocation)
            .SetProperty(s => s.Content, announcement.Content)
            .SetProperty(s => s.Link, announcement.Link));
        return updatedAnnouncements;
    }
}
