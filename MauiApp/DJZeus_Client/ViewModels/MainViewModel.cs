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

namespace DJZeus_Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            LoadData();
        }
        [ObservableProperty]
        string location = "로딩 중...";

        [ObservableProperty]
        string temperature = "??℃";

        [ObservableProperty]
        string weatherDescription = "날씨를 불러오고 있어요.";

        [ObservableProperty]
        string weatherIconUrl = "dotnet_bot.png";

        [ObservableProperty]
        string playlistThumbnailUrl = "dotnet_bot.png";

        [ObservableProperty]
        string playlistTitle = "날씨에 맞는 플레이리스트를 찾고 있어요.";

        private readonly string apiKey = "d25717f19e97e8c7e284790fb9e28328";

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
                string apiKey = "d25717f19e97e8c7e284790fb9e28328";
                string reverseGeocodingUrl = $"https://api.openweathermap.org/geo/1.0/reverse?lat={currentLocation.Latitude}&lon={currentLocation.Longitude}&limit=1&appid={apiKey}";

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
                string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric&lang=kr";
                var weatherResponse = await client.GetAsync(weatherUrl);

                if (!weatherResponse.IsSuccessStatusCode)
                {
                    WeatherDescription = "날씨 정보 로드 실패";
                    return;
                }

                var weatherData = await weatherResponse.Content.ReadFromJsonAsync<WeatherData>();
                Temperature = $"{weatherData.Main.Temp:F0}°C";
                WeatherDescription = weatherData.Weather.FirstOrDefault()?.Description ?? "정보 없음";
                WeatherIconUrl = $"https://openweathermap.org/img/wn/{weatherData.Weather.FirstOrDefault()?.Icon}@2x.png";

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
            // TODO: 여기에 유튜브 링크를 여는 로직 구현
        }

    }
}
