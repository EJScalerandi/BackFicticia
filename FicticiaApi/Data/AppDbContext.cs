using FicticiaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FicticiaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
