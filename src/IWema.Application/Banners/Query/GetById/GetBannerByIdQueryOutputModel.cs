namespace IWema.Application.Banners.Query.GetById;

public record GetBannerByIdQueryOutputModel(Guid Id, string FileLocation, string Title, bool IsActive, string DateCreated);