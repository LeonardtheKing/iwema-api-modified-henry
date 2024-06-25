using IWema.Application.Announcements.Query.GetAllAnnouncements;
using IWema.Application.Announcements.Query.GetAnnouncementById;
using IWema.Domain.Entity;

namespace IWema.Application.Contract;

public interface IAnnouncementRepository
{
    Task<bool> Add(AnnouncementEntity announcements);
    // Task<bool> Delete(Guid id);
    Task<List<AnnouncementEntity>> Get();
    Task<AnnouncementEntity> GetById(Guid id);
    Task<bool> Update(AnnouncementEntity announcements);
    Task<int> UpdateAsync(Guid id, AnnouncementEntity announcement);
    Task<List<GetAllAnnouncementsOutputModel>> GetAllAnnouncements();
    Task<GetAnnouncementByIdOutputModel> GetSingleAnnouncement(Guid announcementId);
    Task<bool> DeleteAnnouncement(Guid id);

}
