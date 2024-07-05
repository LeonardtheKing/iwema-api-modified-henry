using IWema.Application.Contract;
using IWema.Application.Contract.Login;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;

namespace IWema.Infrastructure.Adapters.ActiveDirectory.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly ActiveDirectoryConfigOptions _options;
        private readonly ILogger<ActiveDirectoryService> _logger;

        public ActiveDirectoryService(IOptions<ActiveDirectoryConfigOptions> options, ILogger<ActiveDirectoryService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

       
        public bool AuthenticateStaff(string StaffEmail, string StaffPassword)
        {
            if (!string.IsNullOrEmpty(StaffEmail) && !StaffEmail.Contains("@wemabank.com"))
            {
                StaffEmail += "@wemabank.com";
            }

            string LDapServerIP = _options.LDapServerIP;
            int LDapServerPort = _options.LDapServerPort;

            try
            {
                LdapDirectoryIdentifier ldi = new(LDapServerIP, LDapServerPort);
                LdapConnection ldapConnection = new(ldi)
                {
                    AuthType = AuthType.Basic
                };

                ldapConnection.SessionOptions.ProtocolVersion = 3;

                NetworkCredential nc = new(StaffEmail, StaffPassword);

                ldapConnection.Bind(nc);

                return true;
            }
            catch (LdapException ex)
            {
                _logger.LogError($"An LdapException was thrown :: {ex.Message}\n");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An LdapException was thrown :: {ex.Message}\n");
                return false;
            }
        }

        public bool DoesEmailExist(string staffEmail)
        {
           // _logger.LogInformation("Checking if email exists in Active Directory: {StaffEmail}", staffEmail);

            try
            {
                using var directoryEntry = new DirectoryEntry(GetCurrentDomainPath());
                using var directorySearcher = BuildUserSearcher(directoryEntry);
                directorySearcher.Filter = $"(&(objectCategory=User)(objectClass=person)(mail={staffEmail}))";

                _logger.LogInformation("Checking if email exists in Active Directory: {StaffEmail}", staffEmail);

                var searchResult = directorySearcher.FindOne();

                var result = JsonConvert.SerializeObject(searchResult);

                _logger.LogInformation("Search Result Serialized: {Result}", result);

                bool emailExists = searchResult != null;
                _logger.LogInformation("Email {StaffEmail} exists: {Exists}", staffEmail, emailExists);
                return emailExists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking email existence in Active Directory: {StaffEmail}", staffEmail);
                return false;
            }
        }

        public string GetCurrentDomainPath()
        {
            _logger.LogInformation("Retrieving current domain path from Active Directory");

            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://RootDSE");
                string domainPath = "LDAP://" + directoryEntry.Properties["defaultNamingContext"][0].ToString();
                _logger.LogInformation("Current domain path: {DomainPath}", domainPath);
                return domainPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the current domain path");
                throw;
            }
        }

        public StaffLookUpDto LookUpWemaStaff(string staffEmail)
        {
            _logger.LogInformation("Looking up Wema staff with email: {StaffEmail}", staffEmail);

            try
            {
                using var directoryEntry = new DirectoryEntry(GetCurrentDomainPath());
                using var directorySearcher = BuildUserSearcher(directoryEntry);
                directorySearcher.Filter = $"(&(objectCategory=User)(objectClass=person)(mail={staffEmail}))";
                var searchResult = directorySearcher.FindOne();

                if (searchResult != null)
                {
                    var staffLookUpDto = new StaffLookUpDto
                    {
                        No = GetProperty(searchResult, "sn"),
                        Name = GetProperty(searchResult, "name"),
                        Email = GetProperty(searchResult, "mail"),
                        PrincipalName = GetProperty(searchResult, "userPrincipalName")
                    };
                    _logger.LogInformation("Wema staff found: {StaffLookUpDto}", staffLookUpDto);
                    return staffLookUpDto;
                }

                _logger.LogWarning("No Wema staff found with email: {StaffEmail}", staffEmail);
                return null;
            }
            catch (LdapException ex)
            {
                _logger.LogError(ex, "LDAP error occurred while looking up Wema staff with email: {StaffEmail}", staffEmail);
                throw new LdapException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while looking up Wema staff with email: {StaffEmail}", staffEmail);
                throw new Exception(ex.Message);
            }
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

