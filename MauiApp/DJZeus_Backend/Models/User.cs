using System.ComponentModel.DataAnnotations;

namespace DJZeus_Backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; }
    }
}
