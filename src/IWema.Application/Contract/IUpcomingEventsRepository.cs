using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface IUpcomingEventsRepository
{
    Task<bool> Add(UpcomingEventEntity announcements);
    Task<bool> Delete(Guid id);
    Task<List<UpcomingEventEntity>> Get();
    Task<UpcomingEventEntity> GetById(Guid id);
    Task<bool> Update(UpcomingEventEntity announcements);
    Task<int> UpdateAsync(Guid id, UpcomingEventEntity announcement);
}
