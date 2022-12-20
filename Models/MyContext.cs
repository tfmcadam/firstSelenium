
using Microsoft.EntityFrameworkCore;
namespace firstSelenium.Models;

public class MyContext : DbContext
{

    public MyContext(DbContextOptions options) : base(options) { }

    // Models for Database
    public DbSet<User> Users { get; set; }
    // public DbSet<Wedding> Weddings { get; set; }
    // association db
    // public DbSet<Reservation> Reservations { get; set; }
}