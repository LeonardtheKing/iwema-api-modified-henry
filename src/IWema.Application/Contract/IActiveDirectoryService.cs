using IWema.Application.Contract.Login;

namespace IWema.Application.Contract;

public interface IActiveDirectoryService
{
    bool AuthenticateStaff(string StaffEmail, string StaffPassword);
    StaffLookUpDto LookUpWemaStaff(string StaffEmail);
    string GetCurrentDomainPath();
    bool DoesEmailExist(string staffEmail);
}
