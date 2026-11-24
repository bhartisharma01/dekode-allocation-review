using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Data;

public static class IdentitySeeder
{
    public const string AdminRole = "Admin";
    public const string ViewerRole = "Viewer";

    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration,
        ILogger logger)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { AdminRole, ViewerRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await EnsureUserAsync(userManager,
            configuration.GetSection("SeedUsers:Admin"), AdminRole, logger);

        await EnsureUserAsync(userManager,
            configuration.GetSection("SeedUsers:Viewer"), ViewerRole, logger);
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        IConfigurationSection section,
        string role,
        ILogger logger)
    {
        var userName = section["UserName"];
        var password = section["Password"];

        if (string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning(
                "Seed user for role {Role} skipped: UserName/Password not configured.", role);
            return;
        }

        if (await userManager.FindByNameAsync(userName) is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = section["Email"],
            EmailConfirmed = true,
            LockoutEnabled = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            // Sirf error codes, password kabhi log nahi.
            throw new InvalidOperationException(
                $"Could not seed {role} user: " +
                string.Join(", ", result.Errors.Select(e => e.Code)));
        }

        await userManager.AddToRoleAsync(user, role);
        logger.LogInformation("Seeded {Role} user.", role);
    }
}