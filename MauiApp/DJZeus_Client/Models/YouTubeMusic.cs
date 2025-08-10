using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DJZeus_Client.Models
{
    public class YouTubeMusic
    {
        [JsonPropertyName("items")]
        public YoutubeItem[] Items { get; set; }
    }
    public class YoutubeItem
    {
        [JsonPropertyName("id")]
        public YoutubeVideoId Id { get; set; }

        [JsonPropertyName("snippet")]
        public YoutubeSnippet Snippet { get; set; }
    }

    public class YoutubeVideoId
    {
        [JsonPropertyName("videoId")]
        public string VideoId { get; set; }
    }

    public class YoutubeSnippet
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("thumbnails")]
        public YoutubeThumbnails Thumbnails { get; set; }
    }

    public class YoutubeThumbnails
    {
        [JsonPropertyName("medium")]
        public YoutubeThumbnail Medium { get; set; }
    }

    public class YoutubeThumbnail
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
