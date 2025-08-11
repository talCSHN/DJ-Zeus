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

        private string _youTubeVideoId;

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
                using var client = new HttpClient();
                string serverUrl = DeviceInfo.Platform == DevicePlatform.Android
                                   ? "http://10.0.2.2:5062" // Android 에뮬레이터
                                   : "http://localhost:5062"; // Windows 또는 기타
                string requestUrl = $"{serverUrl}/api/Recommendation?lat={currentLocation.Latitude}&lon={currentLocation.Longitude}";
                var response = await client.GetFromJsonAsync<RecommendationDTO>(requestUrl);
                if (response != null)
                {
                    // 4. 서버가 보내준 데이터로 UI 업데이트
                    Location = response.Location;
                    Temperature = response.Temperature;
                    WeatherDescription = response.WeatherDescription;
                    WeatherIconUrl = response.WeatherIconUrl;
                    PlaylistTitle = response.MusicTitle;
                    PlaylistThumbnailUrl = response.MusicThumbnailUrl;
                    _youTubeVideoId = response.MusicVideoId;
                }
            }
            
            catch (Exception ex)
            {
                Location = "위치 로드 실패";
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        [RelayCommand]
        async Task PlayMusic()
        {
            if (!string.IsNullOrEmpty(_youTubeVideoId))
            {
                string url = $"https://www.youtube.com/watch?v={_youTubeVideoId}";
                await Launcher.Default.OpenAsync(url);
            }
        }

    }
}
