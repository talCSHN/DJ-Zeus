namespace DJZeus_Backend.Models
{
    public class RecommendationDTO
    {
        public string Location { get; set; }
        public string Temperature { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIconUrl { get; set; }
        public string MusicTitle { get; set; }
        public string MusicThumbnailUrl { get; set; }
        public string MusicVideoId { get; set; }
        public string MusicStreamUrl { get; set; }
    }
}
