//using IWema.Application.Common.DTO;
//using IWema.Application.Contract;
//using IWema.Application.Contract.Login;
//using IWema.Application.Contract.Roles;
//using IWema.Domain.Entity;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;
//using System.Text.RegularExpressions;

//namespace IWema.Application.Login.Command
//{
//    public class LoginCommandHandler(
//        IJwtTokenManager jwtManager,
//        IActiveDirectoryService activeDirectoryService,
//        IApplicationUserRepository users,
//        UserManager<ApplicationUser> userManager,
//        RoleManager<IdentityRole> roleManager
//    ) : IRequestHandler<LoginCommand, ServiceResponse<LoginResponse>>
//    {
//        public async Task<ServiceResponse<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
//        {
//            var email = command.Email.ToLower();
//            var user = await userManager.FindByEmailAsync(email);

//            if (user != null)
//            {
//                // Check if the user is locked out
//                if (await userManager.IsLockedOutAsync(user))
//                {
//                    return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
//                }
//            }

//            bool isAuthenticated = false;

//            try
//            {
//                isAuthenticated = activeDirectoryService.AuthenticateStaff(email, command.Password);
//            }
//            catch (UnauthorizedAccessException)
//            {
//                if (user != null)
//                {
//                    await userManager.AccessFailedAsync(user);
//                    if (await userManager.IsLockedOutAsync(user))
//                    {
//                        return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
//                    }
//                }
//                return new ServiceResponse<LoginResponse>("Password is incorrect.");
//            }
//            catch (Exception ex)
//            {
//                return new ServiceResponse<LoginResponse>($"An error occurred: {ex.Message}");
//            }

//            if (!isAuthenticated)
//            {
//                if (user != null)
//                {
//                    await userManager.AccessFailedAsync(user);
//                    if (await userManager.IsLockedOutAsync(user))
//                    {
//                        return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
//                    }
//                }
//                return new ServiceResponse<LoginResponse>("User Authentication Failed");
//            }

//            // Reset failed attempts if the user is authenticated successfully via Active Directory
//            if (user != null)
//            {
//                await userManager.ResetAccessFailedCountAsync(user);
//            }

//            // Check if the user exists in the local database, if not, create a new user
//            if (user == null)
//            {
//                var nameList = email.Split('@')[0].Split('.');
//                var firstName = nameList.Length > 0 ? UpperCaseFirstChar(nameList[0]) : "";
//                var lastName = nameList.Length > 1 ? UpperCaseFirstChar(nameList[1]) : "";

//                user = new ApplicationUser
//                {
//                    FirstName = firstName,
//                    LastName = lastName,
//                    UserName = email,
//                    NormalizedUserName = email.ToUpper(),
//                    Email = email,
//                    NormalizedEmail = email.ToUpper(),
//                    CreatedAt = DateTime.Now
//                };

//                var createdUser = await userManager.CreateAsync(user);
//                if (!createdUser.Succeeded)
//                {
//                    var errorString = "User Creation Failed Because: ";
//                    foreach (var error in createdUser.Errors)
//                    {
//                        errorString += " # " + error.Description;
//                    }
//                    return new ServiceResponse<LoginResponse>(errorString);
//                }

//                await userManager.AddToRoleAsync(user, Roles.STAFF);
//            }

//            // Manually create the authentication ticket and JWT token
//            var userRoles = await userManager.GetRolesAsync(user);
//            var authClaims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.UserName),
//                new Claim(ClaimTypes.NameIdentifier, user.Id),
//                new Claim("JWTID", Guid.NewGuid().ToString()),
//                new Claim("FirstName", user.FirstName),
//                new Claim("LastName", user.LastName),
//            };

//            foreach (var role in userRoles)
//            {
//                authClaims.Add(new Claim(ClaimTypes.Role, role));
//            }

//            var token = jwtManager.GenerateJWT(authClaims);
//            var refreshToken = await jwtManager.GenerateRefreshToken();
//            var name = $"{user.FirstName} {user.LastName}";
//            var userRole = userRoles.FirstOrDefault();
//            var loginResponse = new LoginResponse(name, email, token, refreshToken, userRole);

//            return new ServiceResponse<LoginResponse>("Login Successful.", true, loginResponse);
//        }

//        private static string UpperCaseFirstChar(string text) =>
//            Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
//    }
//}


using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Application.Contract.Login;
using IWema.Application.Contract.Roles;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace IWema.Application.Login.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResponse<LoginResponse>>
    {
        private readonly IJwtTokenManager _jwtManager;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IApplicationUserRepository _users;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public LoginCommandHandler(
            IJwtTokenManager jwtManager,
            IActiveDirectoryService activeDirectoryService,
            IApplicationUserRepository users,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<LoginCommandHandler> logger)
        {
            _jwtManager = jwtManager;
            _activeDirectoryService = activeDirectoryService;
            _users = users;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<ServiceResponse<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email.ToLower();
            _logger.LogInformation("Processing login for {Email}", email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                // Check if the user is locked out
                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarning("User {Email} is locked out.", email);
                    return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
                }
            }

            bool isAuthenticated = false;

            try
            {
                isAuthenticated = _activeDirectoryService.AuthenticateStaff(email, command.Password);
                _logger.LogInformation("Active Directory authentication for {Email} succeeded: {IsAuthenticated}", email, isAuthenticated);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Unauthorized access for {ex.Message} ", email);
                if (user != null)
                {
                    await _userManager.AccessFailedAsync(user);
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        _logger.LogWarning("User {Email} is locked out after failed login attempt.", email);
                        return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
                    }
                }
                return new ServiceResponse<LoginResponse>("Unauthorized access. Please check your credentials and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authentication for {Email}", email);
                return new ServiceResponse<LoginResponse>($"An error occurred: {ex.Message}");
            }

            if (!isAuthenticated)
            {
                if (user != null)
                {
                    await _userManager.AccessFailedAsync(user);
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        _logger.LogWarning("User {Email} is locked out after failed authentication.", email);
                        return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
                    }
                }
                _logger.LogInformation("User authentication failed for {Email}", email);
                return new ServiceResponse<LoginResponse>("User Authentication Failed");
            }

            // Reset failed attempts if the user is authenticated successfully via Active Directory
            if (user != null)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                _logger.LogInformation("Reset access failed count for {Email}", email);
            }

            // Check if the user exists in the local database, if not, create a new user
            if (user == null)
            {
                var nameList = email.Split('@')[0].Split('.');
                var firstName = nameList.Length > 0 ? UpperCaseFirstChar(nameList[0]) : "";
                var lastName = nameList.Length > 1 ? UpperCaseFirstChar(nameList[1]) : "";

                user = new ApplicationUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    CreatedAt = DateTime.Now
                };

                var createdUser = await _userManager.CreateAsync(user);
                if (!createdUser.Succeeded)
                {
                    var errorString = "User Creation Failed Because: ";
                    foreach (var error in createdUser.Errors)
                    {
                        errorString += " # " + error.Description;
                    }
                    _logger.LogError("User creation failed for {Email}: {Errors}", email, errorString);
                    return new ServiceResponse<LoginResponse>(errorString);
                }

                await _userManager.AddToRoleAsync(user, Roles.STAFF);
                _logger.LogInformation("Created new user {Email} and added to role {Role}", email, Roles.STAFF);
            }

            // Manually create the authentication ticket and JWT token
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _jwtManager.GenerateJWT(authClaims);
            var refreshToken = await _jwtManager.GenerateRefreshToken();
            var name = $"{user.FirstName} {user.LastName}";
            var userRole = userRoles.FirstOrDefault();
            var loginResponse = new LoginResponse(name, email, token, refreshToken, userRole);

            _logger.LogInformation("Login successful for {Email}", email);
            return new ServiceResponse<LoginResponse>("Login Successful.", true, loginResponse);
        }

        private static string UpperCaseFirstChar(string text) =>
            Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
    }
}
