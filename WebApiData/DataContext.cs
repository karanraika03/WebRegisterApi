using Microsoft.EntityFrameworkCore;
using WebApiDomain.Model;

namespace WebApiData;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Blog> Blog { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Role> Role { get; set; }
}
