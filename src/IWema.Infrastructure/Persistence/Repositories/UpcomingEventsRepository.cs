using IWema.Application.Contract;
using IWema.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace IWema.Infrastructure.Persistence.Repositories;
public class UpcomingEventsRepository(IWemaDbContext context) : IUpcomingEventsRepository
{
    public async Task<bool> Add(UpcomingEventEntity announcement)
    {
        await context.AddAsync(announcement);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<UpcomingEventEntity> GetById(Guid id) =>
        await context.UpcomingEvents.FindAsync(id);
    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        context.Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<List<UpcomingEventEntity>> Get() =>
       await context.UpcomingEvents.ToListAsync();



    public async Task<bool> Update(UpcomingEventEntity upcomingEvent)
    {
        context.UpcomingEvents.Update(upcomingEvent);
        return await context.SaveChangesAsync() > 1;
    }

    public async Task<int> UpdateAsync(Guid id, UpcomingEventEntity upcomingEvent)
    {
        var updatedAnnouncements = await context
            .UpcomingEvents
            .Where(_x => _x.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(s => s.NameOfEvent, upcomingEvent.NameOfEvent)
            .SetProperty(s => s.ImageLocation, upcomingEvent.ImageLocation)
            .SetProperty(s => s.Date, upcomingEvent.Date)
            );
        return updatedAnnouncements;
    }
}
