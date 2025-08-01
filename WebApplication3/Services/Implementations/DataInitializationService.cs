using WebApplication3.Data;
using WebApplication3.Models.Common;
using WebApplication3.Models.Entities;

namespace WebApplication3.Services.Implementations;

public class DataInitializationService
{
    private readonly ApplicationDbContext _context;

    public DataInitializationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await InitializeRolesAsync();
    }

    private async Task InitializeRolesAsync()
    {
        if (!_context.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = Roles.Student, Description = "Студент - может просматривать курсы и выполнять задания" },
                new Role { Id = 2, Name = Roles.Teacher, Description = "Преподаватель - может создавать курсы, уроки и задания" },
                new Role { Id = 3, Name = Roles.Admin, Description = "Администратор - полный доступ к системе" }
            };

            await _context.Roles.AddRangeAsync(roles);
            await _context.SaveChangesAsync();
        }
    }
} 