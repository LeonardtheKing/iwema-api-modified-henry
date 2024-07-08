using System.ComponentModel.DataAnnotations;

namespace IWema.Domain.Entity;

public class UserLoginSession
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime? LogoutTime { get; set; }
    public double? DurationInSeconds { get; set; }
}
