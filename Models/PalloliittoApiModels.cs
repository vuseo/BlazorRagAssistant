using System.Text.Json.Serialization;

namespace BlazorRagAssistant.Models;

// --- STANDINGS MODELS ---

public class TasoGroupsResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("groups")]
    public List<TasoGroup>? Groups { get; set; }
}

public class TasoGroup
{
    [JsonPropertyName("competition_id")]
    public string CompetitionId { get; set; } = string.Empty;

    [JsonPropertyName("category_id")]
    public string CategoryId { get; set; } = string.Empty;

    [JsonPropertyName("group_id")]
    public string GroupId { get; set; } = string.Empty;

    [JsonPropertyName("group_name")]
    public string GroupName { get; set; } = string.Empty;

    [JsonPropertyName("teams")]
    public List<TasoTeamGroupStanding>? Teams { get; set; }
}

public class TasoTeamGroupStanding
{
    [JsonPropertyName("team_id")]
    public string TeamId { get; set; } = string.Empty;

    [JsonPropertyName("team_name")]
    public string TeamName { get; set; } = string.Empty;

    [JsonPropertyName("current_standing")]
    public int CurrentStanding { get; set; }

    [JsonPropertyName("matches_played")]
    public int MatchesPlayed { get; set; }

    [JsonPropertyName("matches_won")]
    public int MatchesWon { get; set; }

    [JsonPropertyName("matches_tied")]
    public int MatchesTied { get; set; }

    [JsonPropertyName("matches_lost")]
    public int MatchesLost { get; set; }

    [JsonPropertyName("points")]
    public int Points { get; set; }
}

// --- MATCHES MODELS (Resolves TasoMatch and TasoMatchesResponse missing error) ---

public class TasoMatchesResponse
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("matches")]
    public List<TasoMatch>? Matches { get; set; }
}

public class TasoMatch
{
    [JsonPropertyName("match_id")]
    public string MatchId { get; set; } = string.Empty;

    [JsonPropertyName("team_A_name")]
    public string HomeTeamName { get; set; } = string.Empty;

    [JsonPropertyName("team_B_name")]
    public string AwayTeamName { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;

    [JsonPropertyName("venue_name")]
    public string VenueName { get; set; } = string.Empty;
}
public class TulospalveluMatchDto
{
    [JsonPropertyName("team_A_name")]
    public string? HomeTeamName { get; set; }

    [JsonPropertyName("team_B_name")]
    public string? AwayTeamName { get; set; }

    [JsonPropertyName("date")]
    public string? MatchDate { get; set; }

    [JsonPropertyName("time")]
    public string? MatchTime { get; set; }

    [JsonPropertyName("venue_name")]
    public string? VenueName { get; set; }
}