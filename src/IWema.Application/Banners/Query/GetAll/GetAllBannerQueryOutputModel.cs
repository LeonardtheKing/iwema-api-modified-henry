namespace IWema.Application.Banners.Query.GetAll;

public record GetAllBannerQueryOutputModel(Guid Id, string FileLocation, string Title, bool IsActive, string DateCreated);