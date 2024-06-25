namespace IWema.Application.News.Query.GetById;

public record GetNewsByIdQueryOutputModel(
    Guid Id,
    string Title,
    string Content,
    bool IsActive,
    DateTime CreatedAt);