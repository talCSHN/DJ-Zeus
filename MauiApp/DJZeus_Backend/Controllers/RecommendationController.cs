//using DJZeus_Backend.Data;
using DJZeus_Backend.Models;
using DJZeus_Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DJZeus_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public RecommendationController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendation([FromQuery] double lat, [FromQuery] double lon)
        {
            try
            {
                var openWeatherApiKey = _config["ApiKeys:OpenWeatherMap"];
                var youTubeApiKey = _config["ApiKeys:YouTube"];

                using var client = new HttpClient();

                // OpenWeatherMap 지역, 날씨
                string reverseGeocodingUrl = $"https://api.openweathermap.org/geo/1.0/reverse?lat={lat}&lon={lon}&limit=1&appid={openWeatherApiKey}";
                var geoResponse = await client.GetAsync(reverseGeocodingUrl);
                if (!geoResponse.IsSuccessStatusCode) return BadRequest("도시 이름 변환 실패");

                var geocodingResult = await geoResponse.Content.ReadFromJsonAsync<List<OwmGeocodingResponse>>();
                var cityName = geocodingResult?.FirstOrDefault()?.Name;

                string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={openWeatherApiKey}&units=metric&lang=en";
                var weatherResponse = await client.GetAsync(weatherUrl);
                if (!weatherResponse.IsSuccessStatusCode) return BadRequest("날씨 정보 로드 실패");

                var weatherData = await weatherResponse.Content.ReadFromJsonAsync<WeatherData>();

                // YouTube 음악 검색
                string weatherCondition = weatherData.Weather.FirstOrDefault()?.Condition;
                string searchQuery = GetMusicSearchQuery(weatherCondition);
                string encodedSearchQuery = HttpUtility.UrlEncode(searchQuery);

                string youtubeUrl = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={encodedSearchQuery}&type=video&videoCategoryId=10&maxResults=1&key={youTubeApiKey}";
                var youtubeResponse = await client.GetAsync(youtubeUrl);
                if (!youtubeResponse.IsSuccessStatusCode) return BadRequest("음악 검색 실패");

                var youtubeResult = await youtubeResponse.Content.ReadFromJsonAsync<YouTubeMusic>();
                var firstVideo = youtubeResult?.Items?.FirstOrDefault();

                // 추천 기록 저장
                var log = new RecommendationLog
                {
                    Weather = weatherData.Weather.FirstOrDefault()?.Description,
                    SongTitle = firstVideo?.Snippet.Title,
                    VideoId = firstVideo?.Id.VideoId,
                    UserId = 1 // TODO: 실제 로그인 기능 구현 시 현재 사용자 ID로 변경
                };
                _context.RecommendationLogs.Add(log);
                await _context.SaveChangesAsync();

                // 응답 데이터
                var response = new RecommendationDTO
                {
                    Location = cityName,
                    Temperature = $"{weatherData.Main.Temp:F0}°C",
                    WeatherDescription = weatherData.Weather.FirstOrDefault()?.Description,
                    WeatherIconUrl = $"https://openweathermap.org/img/wn/{weatherData.Weather.FirstOrDefault()?.Icon}@2x.png",
                    MusicTitle = firstVideo?.Snippet.Title,
                    MusicThumbnailUrl = firstVideo?.Snippet.Thumbnails.Medium.Url,
                    MusicVideoId = firstVideo?.Id.VideoId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"서버 내부 오류: {ex.Message}");
            }
       }

        // 이 메서드들은 MainViewModel에서 그대로 가져옴
        //private string GetMusicSearchQuery(string weather) { /* ... 이전과 동일 ... */ }
        //private string ToNaturalKorean(string description) { /* ... 이전과 동일 ... */ }
        private string GetMusicSearchQuery(string weather)
        {
            int hour = DateTime.Now.Hour;
            return weather switch
            {
                "Rain" => "비 오는 날 듣기 좋은 감성 플레이리스트",
                "Snow" => "눈 오는 날 듣기 좋은 재즈",
                "Drizzle" => "가랑비 내리는 날 듣기 좋은 인디 음악",
                "Thunderstorm" => "천둥번개 치는 날 듣기 좋은 락 메탈",
                "Clouds" => "흐린 날 듣기 좋은 인디팝",
                "Clear" when hour >= 6 && hour < 12 => "상쾌한 아침 팝송",
                "Clear" when hour >= 12 && hour < 18 => "나른한 오후 드라이브 음악",
                _ => "자기 전 듣기 좋은 Lo-fi"
            };
        }
    }
}
