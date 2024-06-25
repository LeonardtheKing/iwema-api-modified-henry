namespace IWema.Application.News.Command.Update;

public class UpdateNewsInputModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsActive { get; set; }
}