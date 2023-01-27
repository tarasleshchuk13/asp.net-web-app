using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}