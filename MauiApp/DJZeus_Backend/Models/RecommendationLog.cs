using System.ComponentModel.DataAnnotations;

namespace DJZeus_Backend.Models
{
    public class RecommendationLog
    {
        [Key]
        public int RecommendId { get; set; }

        [Required]
        public string Weather { get; set; }

        [Required]
        public string SongTitle { get; set; }

        [Required]
        public string VideoId { get; set; }

        public DateTime RecommendedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
