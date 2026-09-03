using System.Text.Json.Serialization;

namespace BlazorRagAssistant.Models;

public class WeatherResponse
{
    [JsonPropertyName("hourly")]
    public HourlyData? Hourly { get; set; }
}

public class HourlyData
{
    [JsonPropertyName("time")]
    public List<string>? Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public List<double>? Temperature { get; set; }

    [JsonPropertyName("precipitation")]
    public List<double>? Precipitation { get; set; }
}

public class MatchWeather
{
    public double Temperature { get; set; }
    public double Precipitation { get; set; }
    public string PitchCondition => Precipitation > 0.5 ? "🌧️ Wet Pitch (Fast ball movement)" : "🌤️ Dry Pitch (Normal bounce)";
}