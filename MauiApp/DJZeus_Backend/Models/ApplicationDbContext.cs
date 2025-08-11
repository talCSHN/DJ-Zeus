using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DJZeus_Backend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<RecommendationLog> RecommendationLogs { get; set; }
    }
}
