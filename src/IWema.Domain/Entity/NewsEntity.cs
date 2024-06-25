namespace IWema.Domain.Entity
{
    public class NewsEntity : EntityBase
    {
        public NewsEntity() { }

        public NewsEntity(string title, string content, bool isActive = true)
        {
            Title = title;
            Content = content;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public bool IsActive { get; private set; }

        public void Update(string title, string content, bool isActive)
        {
            Title = title;
            Content = content;
            IsActive = isActive;
            UpdatedAt = DateTime.Now;
        }
    }

}