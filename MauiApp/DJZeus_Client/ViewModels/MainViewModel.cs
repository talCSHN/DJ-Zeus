using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DJZeus_Client.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DJZeus_Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            LoadData();
        }
        [ObservableProperty]
        string location = "로딩 중";

        [ObservableProperty]
        string temperature = "??℃";

        [ObservableProperty]
        string weatherDescription = "test";

        [ObservableProperty]
        string weatherIconUrl = "dotnet_bot.png";

        [ObservableProperty]
        string playlistThumbnailUrl = "dotnet_bot.png";

        [ObservableProperty]
        string playlistTitle = "test";

        private readonly string openweatherApiKey = "d25717f19e97e8c7e284790fb9e28328";
        private readonly string youtubeApiKey = "AIzaSyDTk-iV9rGpVBCkpPQTjX9NxXrhEOpqgkU";
        private string youTubeVideoId;

        [RelayCommand]
        async Task LoadData()
        {
            try
            {
                var currentLocation = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1)));
                if (currentLocation == null)
                {
                    Location = "위치 정보 로드 불가";
                    return;
                }
                string reverseGeocodingUrl = $"https://api.openweathermap.org/geo/1.0/reverse?lat={currentLocation.Latitude}&lon={currentLocation.Longitude}&limit=1&appid={openweatherApiKey}";

                using var client = new HttpClient();
                var geoResponse = await client.GetAsync(reverseGeocodingUrl);
                if (!geoResponse.IsSuccessStatusCode)
                {
                    Location = "도시 이름 변환 실패";
                    Debug.WriteLine($"Reverse Geocoding API Error: {geoResponse.StatusCode}");
                    return;
                    
                }
                var geocodingResult = await geoResponse.Content.ReadFromJsonAsync<List<OwmGeocodingResponse>>();
                var cityName = geocodingResult?.FirstOrDefault()?.Name;
                if (!string.IsNullOrEmpty(cityName))
                {
                    Location = cityName;
                }
                else
                {
                    Location = "도시명 로드 불가";
                }
                string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={openweatherApiKey}&units=metric&lang=kr";
                var weatherResponse = await client.GetAsync(weatherUrl);

                if (!weatherResponse.IsSuccessStatusCode)
                {
                    WeatherDescription = "날씨 정보 로드 실패";
                    return;
                }

                var weatherData = await weatherResponse.Content.ReadFromJsonAsync<WeatherData>();
                //Debug.WriteLine(weatherData.Weather);
                Temperature = $"{weatherData.Main.Temp:F0}°C";
                WeatherDescription = weatherData.Weather.FirstOrDefault()?.Description;
                WeatherIconUrl = $"https://openweathermap.org/img/wn/{weatherData.Weather.FirstOrDefault()?.Icon}@2x.png";
                string weatherCondition = weatherData.Weather.FirstOrDefault()?.Condition ?? "Clear";
                string searchQuery = GetMusicSearchQuery(weatherCondition);
                string encodedSearchQuery = HttpUtility.UrlEncode(searchQuery); // 검색어를 URL 인코딩

                string youtubeUrl = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={encodedSearchQuery}&type=video&videoCategoryId=10&maxResults=1&key={youtubeApiKey}";
                var youtubeResponse = await client.GetAsync(youtubeUrl);
                if (youtubeResponse.IsSuccessStatusCode)
                {
                    var youtubeResult = await youtubeResponse.Content.ReadFromJsonAsync<YouTubeMusic>();
                    var firstVideo = youtubeResult?.Items?.FirstOrDefault();
                    if (firstVideo != null)
                    {
                        PlaylistTitle = firstVideo.Snippet.Title;
                        PlaylistThumbnailUrl = firstVideo.Snippet.Thumbnails.Medium.Url;
                        youTubeVideoId = firstVideo.Id.VideoId;
                    }
                }
                else
                {
                    PlaylistTitle = "음악을 찾을 수 없어요.";
                    Debug.WriteLine($"YouTube API Error: {youtubeResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Location = "위치 로드 실패";
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }
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

        [RelayCommand]
        async Task PlayMusic()
        {
            if (!string.IsNullOrEmpty(youTubeVideoId))
            {
                string url = $"https://www.youtube.com/watch?v={youTubeVideoId}";
                await Launcher.Default.OpenAsync(url);
            }
        }

    }
}
