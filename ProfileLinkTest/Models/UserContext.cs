using Microsoft.EntityFrameworkCore;

namespace ProfileLinkTest.Models;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        modelBuilder.UseSerialColumns();
	}

	public DbSet<User> Users { get; set; }
    public DbSet<Social> Socials { get; set; }
    public DbSet<Link> Links { get; set; }
}
