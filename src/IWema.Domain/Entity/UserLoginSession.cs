using System.ComponentModel.DataAnnotations;

namespace IWema.Domain.Entity;

public class UserLoginSession 
{
    [Key]
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public DateTime LoginTime { get; private set; }
    public DateTime? LogoutTime { get; private set; }
    public TimeSpan? DurationInSeconds { get; private set; }
    public bool IsActive { get; private set; }

    public UserLoginSession() { }

    public UserLoginSession(Guid id, string userId, DateTime loginTime, DateTime? logoutTime, TimeSpan? durationInSeconds, bool isActive)
    {
        Id = id;
        UserId = userId;
        LoginTime = loginTime;
        LogoutTime = logoutTime;
        DurationInSeconds = durationInSeconds;
        IsActive = isActive;
    }

    public UserLoginSession(Guid id, string userId, DateTime loginTime, bool isActive)
    {
        Id= id;
        UserId = userId;
        LoginTime = loginTime;
        IsActive = isActive;
    }


    public static UserLoginSession Create(Guid Id, string userId, DateTime loginTime, bool isActive)
    {
        return new ( Id,userId, loginTime, isActive);
    }

    public void UpdateLogOutTime(DateTime logoutTime)
    {
        LogoutTime = logoutTime;
    }

    public void Update(string userId, DateTime loginTime, DateTime? logoutTime, TimeSpan? durationInSeconds, bool isActive)
    {
        UserId = userId;
        LoginTime = loginTime;
        LogoutTime = logoutTime;
        DurationInSeconds = durationInSeconds;
        IsActive = isActive;
    }


    public void UpdateLoginTime(DateTime loginTime)
    {
        LoginTime = loginTime;
    }

    public void UpdateLogoutTime(DateTime? logoutTime)
    {
        LogoutTime = logoutTime;
    }

    public void UpdateDurationInSeconds(TimeSpan? durationInSeconds)
    {
        DurationInSeconds = durationInSeconds;
    }

    public void UpdateIsActive(bool isActive)
    {
        IsActive = isActive;
    }
}
