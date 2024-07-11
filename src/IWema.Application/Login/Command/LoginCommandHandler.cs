using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Application.Contract.Login;
using IWema.Application.Contract.Roles;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace IWema.Application.Login.Command
{
    public class LoginCommandHandler(
        IJwtTokenManager jwtManager,
        IActiveDirectoryService activeDirectoryService,
        IApplicationUserRepository users,
        IUserSessionRepository userSessionRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
    ) : IRequestHandler<LoginCommand, ServiceResponse<LoginResponse>>
    {
        public async Task<ServiceResponse<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var email = command.Email.ToLower();
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                // Check if the user is locked out
                if (await userManager.IsLockedOutAsync(user))
                {
                    return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
                }
            }

            bool isAuthenticated = false;

            try
            {
                isAuthenticated = activeDirectoryService.AuthenticateStaff(email, command.Password);
            }
            catch (UnauthorizedAccessException)
            {
                if (user != null)
                {
                    await userManager.AccessFailedAsync(user);
                    if (await userManager.IsLockedOutAsync(user))
                    {
                        return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
                    }
                }
                return new ServiceResponse<LoginResponse>("Password is incorrect.");
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginResponse>($"An error occurred: {ex.Message}");
            }

            if (!isAuthenticated)
            {
                if (user != null)
                {
                    await userManager.AccessFailedAsync(user);
                    if (await userManager.IsLockedOutAsync(user))
                    {
                        return new ServiceResponse<LoginResponse>("You have been locked out. Please try again later.");
                    }
                }
                return new ServiceResponse<LoginResponse>("User Authentication Failed");
            }

            // Reset failed attempts if the user is authenticated successfully via Active Directory
            if (user != null)
            {
                await userManager.ResetAccessFailedCountAsync(user);
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

                var createdUser = await userManager.CreateAsync(user);
                if (!createdUser.Succeeded)
                {
                    var errorString = "User Creation Failed Because: ";
                    foreach (var error in createdUser.Errors)
                    {
                        errorString += " # " + error.Description;
                    }
                    return new ServiceResponse<LoginResponse>(errorString);
                }

                await userManager.AddToRoleAsync(user, Roles.STAFF);
            }

            // Create a new login session and set IsActive to true
            var loginSession = UserLoginSession.Create(Guid.NewGuid(), user.Id, DateTime.UtcNow, true);
            await userSessionRepository.AddSessionAsync(loginSession);

            // Manually create the authentication ticket and JWT token
            var userRoles = await userManager.GetRolesAsync(user);
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

            var token = jwtManager.GenerateJWT(authClaims);
            var refreshToken = await jwtManager.GenerateRefreshToken();
            var name = $"{user.FirstName} {user.LastName}";
            var userRole = userRoles.FirstOrDefault();
            var loginResponse = new LoginResponse(name, email, token, refreshToken, userRole);

            return new ServiceResponse<LoginResponse>("Login Successful.", true, loginResponse);
        }

        private static string UpperCaseFirstChar(string text) =>
            Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
    }
}
