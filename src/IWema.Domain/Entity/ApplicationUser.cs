using Microsoft.AspNetCore.Identity;

namespace IWema.Domain.Entity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

}
