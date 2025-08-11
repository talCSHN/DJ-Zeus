using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DJZeus_Client.Models
{
    public class WeatherData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 상세 날씨 정보 배열. 보통 첫 번째 요소만 사용합니다.
        /// </summary>
        [JsonPropertyName("weather")]
        public WeatherInfo[] Weather { get; set; }

        /// <summary>
        /// 주요 날씨 정보 (온도 등)
        /// </summary>
        [JsonPropertyName("main")]
        public MainInfo Main { get; set; }
    }

    /// <summary>
    /// "main" JSON 객체에 해당하는 클래스
    /// </summary>
    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public float Temp { get; set; }
    }

    /// <summary>
    /// "weather" JSON 배열의 각 요소에 해당하는 클래스
    /// </summary>
    public class WeatherInfo
    {
        /// <summary>
        /// 날씨 상태 (예: "Clear", "Rain", "Clouds")
        /// </summary>
        [JsonPropertyName("main")]
        public string Condition { get; set; }

        /// <summary>
        /// 한국어로 된 상세 날씨 설명 (예: "맑음", "구름낀 하늘")
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// 날씨 아이콘 ID (예: "01d", "10n")
        /// </summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}

