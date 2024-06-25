using IWema.Application.Contract.Roles;
using IWema.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace IWema.Infrastructure.Persistence.Seeding;

public class DataInitializer
{

    public static async Task Initialize(IWemaDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.MenuBars.Count() > 0)
        {
            return;
        }

        string filePath = Path.Combine(AppContext.BaseDirectory, @"Persistence\Seeding\MenuBar.json");
        string menuBarJSON = await File.ReadAllTextAsync(filePath);
        if (string.IsNullOrWhiteSpace(menuBarJSON))
        {
            return;
        }

        List<MenuBar> menuBars = JsonConvert.DeserializeObject<List<MenuBar>>(menuBarJSON);

        if (menuBars == null || menuBars.Count == 0)
        {
            return;
        }

        await context.MenuBars.AddRangeAsync(menuBars);
        await context.SaveChangesAsync();
    }

    public static async Task InitializeUsersAndRolesAsync(IWemaDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { Roles.ADMIN, Roles.STAFF, Roles.SUPERADMIN };

        foreach (string role in roles)
        {
            if (!context.Roles.Any(r => r.Name == role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                await context.SaveChangesAsync();
            }
        }


        var users = SeedAdminUsers();

        foreach (var user in users)
        {
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = password.HashPassword(user, "secret"); // Set your desired password here
                await userManager.CreateAsync(user);
                await context.SaveChangesAsync();

                await AssignRoles(userManager, user.Email, roles);
            }
        }

    }

    public static async Task<IdentityResult> AssignRoles(UserManager<ApplicationUser> userManager, string email, string[] roles)
    {
        ApplicationUser user = await userManager.FindByEmailAsync(email);
        var result = await userManager.AddToRolesAsync(user, roles);

        return result;
    }

    public static ApplicationUser[] SeedAdminUsers()
    {
        ApplicationUser[] users = [ new ApplicationUser{
            FirstName = "Akinkunmi",
            LastName = "Okunola",
            Email = "Akinkunmi.Okunola@wemabank.com",
            NormalizedEmail = "AKINKUNMI.OKUNOLA@WEMABANK.COM",
            UserName = "Akinkunmi.Okunola@wemabank.com",
            NormalizedUserName = "AKINKUNMI.OKUNOLA@WEMABANK.COM",
            PhoneNumber = "+2347082018781",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "Abisola",
            LastName = "Smith",
            Email = "Abisola.Smith@wemabank.com",
            NormalizedEmail = "ABISOLA.SMITH@WEMABANK.COM",
            UserName = "Abisola.Smith@wemabank.com",
            NormalizedUserName = "ABISOLA.SMITH@WEMABANK.COM",
            PhoneNumber = "+234708201878",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
          new ApplicationUser
        {
            FirstName = "Oluwasayo",
            LastName = "Alaka",
            Email = "Oluwasayo.Alaka@wemabank.com",
            NormalizedEmail = "OLUWASAYO.ALAKA@WEMABANK.COM",
            UserName = "Oluwasayo.Alaka@wemabank.com",
            NormalizedUserName = "OLUWASAYO.ALAKA@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
          new ApplicationUser
        {
            FirstName = "Folasade",
            LastName = "Adeyemo",
            Email = "Folasade.Adeyemo@wemabank.com",
            NormalizedEmail = "FOLASADE.ADEYEMO@WEMABANK.COM",
            UserName = "Folasade.Adeyemo@wemabank.com",
            NormalizedUserName = "FOLASADE.ADEYEMO@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
          new ApplicationUser
        {
            FirstName = "Praise",
            LastName = "Omoniyi",
            Email = "Praise.Omoniyi@wemabank.com",
            NormalizedEmail = "PRAISE.OMONIYI@WEMABANK.COM",
            UserName = "Praise.Omoniyi@wemabank.com",
            NormalizedUserName = "IZUCHUKWU.OKORIE@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "Izuchukwu",
            LastName = "Okorie",
            Email = "Izuchukwu.Okorie@wemabank.com",
            NormalizedEmail = "IZUCHUKWU.OKORIE@WEMABANK.COM",
            UserName = "Izuchukwu.Okorie@wemabank.com",
            NormalizedUserName = "IZUCHUKWU.OKORIE@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "Evelyn",
            LastName = "Ita",
            Email = "Evelyn.Ita@wemabank.com",
            NormalizedEmail = "EVELYN.ITA@WEMABANK.COM",
            UserName = "Evelyn.Ita@wemabank.com",
            NormalizedUserName = "EVELYN.ITA@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
          new ApplicationUser
        {
            FirstName = "Deborah",
            LastName = "Aladi",
            Email = "Deborah.Aladi@wemabank.com",
            NormalizedEmail = "DEBORAH.ALADI@WEMABANK.COM",
            UserName = "Deborah.Aladi@wemabank.com",
            NormalizedUserName = "DEBORAH.ALADI@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "Ololade",
            LastName = "Williams",
            Email = "ololade.williams@wemabank.com",
            NormalizedEmail = "OLOLADE.WILLIAMS@WEMABANK.COM",
            UserName = "ololade.williams@wemabank.com",
            NormalizedUserName = "OLOLADE.WILLIAMS@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "oluwasayo",
            LastName = "Oyenuga",
            Email = "oluwasayo.oyenuga@wemabank.com",
            NormalizedEmail = "OLUWASAYO.OYENUGA@WEMABANK.COM",
            UserName = "oluwasayo.oyenuga@wemabank.com",
            NormalizedUserName = "OLUWASAYO.OYENUGA@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "Stephen",
            LastName = "Nathaniel",
            Email = "stephen.nathaniel@wemabank.com",
            NormalizedEmail = "STEPHEN.NATHANIEL@WEMABANK.COM",
            UserName = "stephen.nathaniel@wemabank.com",
            NormalizedUserName = "STEPHEN.NATHANIEL@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "Jennifer",
            LastName = "Egwuatu",
            Email = "Jennifer.Egwuatu@wemabank.com",
            NormalizedEmail = "JENNIFER.EGWUATU@WEMABANK.COM",
            UserName = "Jennifer.Egwuatu@wemabank.com",
            NormalizedUserName = "JENNIFER.EGWUATU@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
         new ApplicationUser
        {
            FirstName = "Christopher",
            LastName = "Adekunle",
            Email = "Christopher.Adekunle@wemabank.com",
            NormalizedEmail = "CHRISTOPHER.ADEKUNLE@WEMABANK.COM",
            UserName = "Christopher.Adekunle@wemabank.com",
            NormalizedUserName = "CHRISTOPHER.ADEKUNLE@WEMABANK.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        }];
        return users;
    }





}