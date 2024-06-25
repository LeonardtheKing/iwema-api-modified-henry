//using IWema.Application.Contract;
//using IWema.Application.Contract.Login;
//using Microsoft.Extensions.Options;
//using System.DirectoryServices;
//using System.DirectoryServices.Protocols;
//using System.Net;

//namespace IWema.Infrastructure.Adapters.ActiveDirectory.Services;

//public class ActiveDirectoryService(IOptions<ActiveDirectoryConfigOptions> options) : IActiveDirectoryService
//{
//    private readonly ActiveDirectoryConfigOptions _options = options.Value;

//    public bool AuthenticateStaff(string StaffEmail, string StaffPassword)
//    {
//        if (!StaffEmail.Contains(_options.Domain))
//        {
//            StaffEmail = StaffEmail + _options.Domain;
//        }
//        try
//        {
//            LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier(_options.LDapServerIP, _options.LDapServerPort);
//            using (var ldapConnection = new LdapConnection(ldi))
//            {
//                // LDap successfully created at this point
//                ldapConnection.AuthType = AuthType.Basic;
//                ldapConnection.SessionOptions.ProtocolVersion = 3;
//                NetworkCredential networkCredential = new NetworkCredential(StaffEmail, StaffPassword);

//                // LDap Binds and Authenticates at this point
//                ldapConnection.Bind(networkCredential);

//                return true;
//            }
//        }
//        catch (LdapException e)
//        {
//            // Check if the error is due to invalid credentials
//            if (e.ErrorCode == 49) // Invalid credentials error code for LDAP
//            {
//                throw new UnauthorizedAccessException("Incorrect password.", e);
//            }
//            else
//            {
//                throw new Exception("An error occurred during LDAP authentication.", e);
//            }
//        }
//        catch (Exception e)
//        {
//            throw new Exception("An unexpected error occurred during authentication.", e);
//        }
//    }

//    public string GetCurrentDomainPath()
//    {
//        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://RootDSE");

//        return "LDAP://" + directoryEntry.Properties["defaultNamingContext"][0].ToString();
//    }

//    public StaffLookUpDto LookUpWemaStaff(string staffEmail)
//    {
//        try
//        {
//            using var directoryEntry = new DirectoryEntry(GetCurrentDomainPath());
//            using var directorySearcher = BuildUserSearcher(directoryEntry);
//            directorySearcher.Filter = $"(&(objectCategory=User)(objectClass=person)(mail={staffEmail}))";

//            var searchResult = directorySearcher.FindOne();

//            if (searchResult != null)
//            {
//                return new StaffLookUpDto
//                {
//                    No = GetProperty(searchResult, "sn"),
//                    Name = GetProperty(searchResult, "name"),
//                    Email = GetProperty(searchResult, "mail"),
//                    PrincipalName = GetProperty(searchResult, "userPrincipalName")
//                };
//            }
//        }
//        catch (LdapException ex)
//        {
//            throw new LdapException(ex.Message);
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }
//        return null;
//    }

//    private static DirectorySearcher BuildUserSearcher(DirectoryEntry directoryEntry)
//    {
//        var directorySearcher = new DirectorySearcher(directoryEntry);

//        directorySearcher.PropertiesToLoad.AddRange(new[]
//        {
//            "name",
//            "mail",
//            "givenname",
//            "sn",
//            "userPrincipalName",
//            "distinguishedName",
//            "staffno"
//        });

//        return directorySearcher;
//    }

//    private string GetProperty(SearchResult searchResult, string propertyName)
//    {
//        var property = searchResult.Properties[propertyName];
//        return property != null && property.Count > 0 ? property[0].ToString() : string.Empty;
//    }
//}

using IWema.Application.Contract;
using IWema.Application.Contract.Login;
using Microsoft.Extensions.Options;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace IWema.Infrastructure.Adapters.ActiveDirectory.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly ActiveDirectoryConfigOptions _options;

        public ActiveDirectoryService(IOptions<ActiveDirectoryConfigOptions> options)
        {
            _options = options.Value;
        }

        public bool AuthenticateStaff(string StaffEmail, string StaffPassword)
        {
            if (!StaffEmail.Contains(_options.Domain))
            {
                StaffEmail = StaffEmail + _options.Domain;
            }
            try
            {
                LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier(_options.LDapServerIP, _options.LDapServerPort);
                using (var ldapConnection = new LdapConnection(ldi))
                {
                    ldapConnection.AuthType = AuthType.Basic;
                    ldapConnection.SessionOptions.ProtocolVersion = 3;
                    NetworkCredential networkCredential = new NetworkCredential(StaffEmail, StaffPassword);
                    ldapConnection.Bind(networkCredential);
                    return true;
                }
            }
            catch (LdapException e)
            {
                if (e.ErrorCode == 49)
                {
                    throw new UnauthorizedAccessException("Incorrect password.", e);
                }
                else
                {
                    throw new Exception("An error occurred during LDAP authentication.", e);
                }
            }
            catch (Exception e)
            {
                throw new Exception("An unexpected error occurred during authentication.", e);
            }
        }

        public bool DoesEmailExist(string staffEmail)
        {
            try
            {
                using var directoryEntry = new DirectoryEntry(GetCurrentDomainPath());
                using var directorySearcher = BuildUserSearcher(directoryEntry);
                directorySearcher.Filter = $"(&(objectCategory=User)(objectClass=person)(mail={staffEmail}))";
                var searchResult = directorySearcher.FindOne();
                return searchResult != null;
            }
            catch
            {
                return false;
            }
        }

        public string GetCurrentDomainPath()
        {
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://RootDSE");
            return "LDAP://" + directoryEntry.Properties["defaultNamingContext"][0].ToString();
        }

        public StaffLookUpDto LookUpWemaStaff(string staffEmail)
        {
            try
            {
                using var directoryEntry = new DirectoryEntry(GetCurrentDomainPath());
                using var directorySearcher = BuildUserSearcher(directoryEntry);
                directorySearcher.Filter = $"(&(objectCategory=User)(objectClass=person)(mail={staffEmail}))";
                var searchResult = directorySearcher.FindOne();
                if (searchResult != null)
                {
                    return new StaffLookUpDto
                    {
                        No = GetProperty(searchResult, "sn"),
                        Name = GetProperty(searchResult, "name"),
                        Email = GetProperty(searchResult, "mail"),
                        PrincipalName = GetProperty(searchResult, "userPrincipalName")
                    };
                }
            }
            catch (LdapException ex)
            {
                throw new LdapException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        private static DirectorySearcher BuildUserSearcher(DirectoryEntry directoryEntry)
        {
            var directorySearcher = new DirectorySearcher(directoryEntry);
            directorySearcher.PropertiesToLoad.AddRange(new[]
            {
                "name",
                "mail",
                "givenname",
                "sn",
                "userPrincipalName",
                "distinguishedName",
                "staffno"
            });
            return directorySearcher;
        }

        private string GetProperty(SearchResult searchResult, string propertyName)
        {
            var property = searchResult.Properties[propertyName];
            return property != null && property.Count > 0 ? property[0].ToString() : string.Empty;
        }
    }
}
