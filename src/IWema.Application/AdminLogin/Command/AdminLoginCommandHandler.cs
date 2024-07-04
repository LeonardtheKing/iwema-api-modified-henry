using IWema.Application.Common.DTO;
using IWema.Application.Contract;
using IWema.Application.Contract.Login;
using IWema.Application.Contract.Roles;
using IWema.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace IWema.Application.Login.AdminLogin.Command
{
    public record AdminLoginCommand(string Email, string Password) : IRequest<ServiceResponse<LoginResponse>>;

    public class AdminLoginCommandHandler(
        IJwtTokenManager jwtManager,
        IActiveDirectoryService activeDirectoryService,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IApplicationUserRepository users) : IRequestHandler<AdminLoginCommand, ServiceResponse<LoginResponse>>
    {
        public async Task<ServiceResponse<LoginResponse>> Handle(AdminLoginCommand command, CancellationToken cancellationToken)
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

            bool emailExists = activeDirectoryService.DoesEmailExist(email);
            if (!emailExists)
            {
                return new ServiceResponse<LoginResponse>("Email is incorrect.");
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

            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Contains(Roles.ADMIN))
            {
                return new ServiceResponse<LoginResponse>("User is not an admin.");
            }

            // Manually create the authentication ticket and JWT token
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = jwtManager.GenerateJWT(authClaims);
            var refreshToken = await jwtManager.GenerateRefreshToken();
            var name = $"{user.FirstName} {user.LastName}";
            var userRole = roles.FirstOrDefault();
            var loginResponse = new LoginResponse(name, email, token, refreshToken, userRole);

            return new ServiceResponse<LoginResponse>("Login Successful.", true, loginResponse);
        }

        private static string UpperCaseFirstChar(string text) =>
            Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
    }
}
