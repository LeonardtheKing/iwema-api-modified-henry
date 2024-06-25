using IWema.Application.Contract;

namespace IWema.Application.Login.Command;

public static class FailedLoginAttemptManager
{
    private static readonly Dictionary<string, (int EmailAttempts, int PasswordAttempts, int BothAttempts, DateTime? LockoutEnd)> _failedAttempts = new();
    private static readonly IActiveDirectoryService _activeDirectoryService;

    public static void RecordFailedAttempt(string email, string type)
    {
        if (_failedAttempts.ContainsKey(email))
        {
            var attemptsInfo = _failedAttempts[email];
            if (type == "email")
            {
                attemptsInfo.EmailAttempts++;
            }
            else if (type == "password")
            {
                attemptsInfo.PasswordAttempts++;
            }
            else if (type == "both")
            {
                attemptsInfo.BothAttempts++;
            }
            _failedAttempts[email] = attemptsInfo;
        }
        else
        {
            if (type == "email")
            {
                _failedAttempts[email] = (1, 0, 0, null);
            }
            else if (type == "password")
            {
                _failedAttempts[email] = (0, 1, 0, null);
            }
            else if (type == "both")
            {
                _failedAttempts[email] = (0, 0, 1, null);
            }
        }
    }

    public static int GetFailedAttempts(string email, string type)
    {
        if (_failedAttempts.TryGetValue(email, out var attemptsInfo))
        {
            if (type == "email") return attemptsInfo.EmailAttempts;
            if (type == "password") return attemptsInfo.PasswordAttempts;
            if (type == "both") return attemptsInfo.BothAttempts;
        }
        return 0;
    }

    public static void ResetFailedAttempts(string email)
    {
        if (_failedAttempts.ContainsKey(email))
        {
            _failedAttempts[email] = (0, 0, 0, null);
        }
    }

    public static void LockoutUser(string email)
    {
        if (_failedAttempts.ContainsKey(email))
        {
            var attemptsInfo = _failedAttempts[email];
            attemptsInfo.LockoutEnd = DateTime.Now.AddSeconds(10);
            _failedAttempts[email] = attemptsInfo;
        }
    }

    //public static bool IsLockedOut(string email)
    //{
    //    if (_failedAttempts.TryGetValue(email, out var attemptsInfo) && attemptsInfo.LockoutEnd.HasValue)
    //    {
    //        if (DateTime.Now < attemptsInfo.LockoutEnd.Value)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            _failedAttempts[email] = (attemptsInfo.EmailAttempts, attemptsInfo.PasswordAttempts, attemptsInfo.BothAttempts, null); // Reset lockout end if lockout period is over
    //        }
    //    }
    //    return false;
    //}

    public static bool IsLockedOut(string email)
    {
        if (_failedAttempts.TryGetValue(email, out var attemptsInfo))
        {
            // Calculate the total number of attempts
            int totalAttempts = attemptsInfo.EmailAttempts + attemptsInfo.PasswordAttempts + attemptsInfo.BothAttempts;

            // Check if the total attempts reach or exceed 5
            if (totalAttempts >= 5)
            {
                LockoutUser(email);
                return true;
            }

            // Check if the user is currently locked out
            if (attemptsInfo.LockoutEnd.HasValue)
            {
                if (DateTime.Now < attemptsInfo.LockoutEnd.Value)
                {
                    return true;
                }
                else
                {
                    // Reset lockout end if lockout period is over
                    _failedAttempts[email] = (attemptsInfo.EmailAttempts, attemptsInfo.PasswordAttempts, attemptsInfo.BothAttempts, null);
                }
            }
        }
        return false;
    }

    public static string CheckLogin(string email, string password)
    {
        if (IsLockedOut(email))
        {
            return "User is locked out.";
        }

        bool emailExists = _activeDirectoryService.DoesEmailExist(email);

        if (!emailExists)
        {
            RecordFailedAttempt(email, "email");
            if (GetFailedAttempts(email, "email") >= 4)
            {
                LockoutUser(email);
                throw new Exception("You have reached the maximum login attempt. refresh page and retry.");
            }
            return "Email is incorrect.";
        }

        try
        {
            bool isAuthenticated = _activeDirectoryService.AuthenticateStaff(email, password);

            if (isAuthenticated)
            {
                ResetFailedAttempts(email);
                return "Login successful.";
            }
            else
            {
                RecordFailedAttempt(email, "password");
                if (GetFailedAttempts(email, "password") >= 4)
                {
                    LockoutUser(email);
                    throw new Exception("You have reached the maximum login attempt. refresh page and retry.");
                }
                return "Password is incorrect.";
            }
        }
        catch (UnauthorizedAccessException)
        {
            RecordFailedAttempt(email, "password");
            if (GetFailedAttempts(email, "password") >= 4)
            {
                LockoutUser(email);
                throw new Exception("You have reached the maximum login attempt. Wait for 10 seconds to retry.");
            }
            return "Password is incorrect.";
        }
        catch (Exception ex)
        {
            RecordFailedAttempt(email, "both");
            if (GetFailedAttempts(email, "both") >= 4)
            {
                LockoutUser(email);
                throw new Exception("You have reached the maximum login attempt. Wait for 10 seconds to retry.");
            }
            return $"An error occurred: {ex.Message}";
        }
    }
}
