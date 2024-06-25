namespace IWema.Application.News.Query.GetAll;

public record GetAllNewsQueryOutputModel(
    Guid Id,
    string Title,
    string Content,
    bool IsActive,
    DateTime CreatedAt);
