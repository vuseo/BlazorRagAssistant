using System.Net.Http.Json;
using BlazorRagAssistant.Models;

namespace BlazorRagAssistant.Services;

public class PalloliittoApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PalloliittoApiService> _logger;

    public PalloliittoApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<PalloliittoApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Fetches live standings for a given category/group ID from Palloliitto TASO API.
    /// </summary>
    public async Task<List<TeamStats>> GetLiveGroupStandingsAsync(string competitionId = "spljp15", string categoryId = "VL", string groupId = "1")
    {
        try
        {
            // Public endpoint call without requiring an api_key
            var requestUrl = $"https://spl.torneopal.fi/taso/rest/getGroups?competition_id={competitionId}&category_id={categoryId}&group_id={groupId}";
            var response = await _httpClient.GetFromJsonAsync<TasoGroupsResponse>(requestUrl);

            var firstGroup = response?.Groups?.FirstOrDefault();
            if (firstGroup?.Teams != null && firstGroup.Teams.Any())
            {
                return firstGroup.Teams.Select(t => new TeamStats
                {
                    Position = t.CurrentStanding,
                    TeamName = t.TeamName,
                    Played = t.MatchesPlayed,
                    Wins = t.MatchesWon,
                    Draws = t.MatchesTied,
                    Losses = t.MatchesLost,
                    Points = t.Points
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"[PALLOLIITTO API NOTICE] Could not fetch live group standings: {ex.Message}. Using fallback data.");
        }

        return GetFallbackStandings();
    }

    /// <summary>
    /// Fetches upcoming matches for a given club or venue ID.
    /// </summary>
    public async Task<List<TasoMatch>> GetUpcomingMatchesAsync(int clubId = 0)
    {
        string apiKey = _configuration["Palloliitto:ApiKey"] ?? string.Empty;

        if (!string.IsNullOrEmpty(apiKey) && clubId > 0)
        {
            try
            {
                var requestUrl = $"https://spl.torneopal.fi/taso/rest/getMatches?api_key={apiKey}&club_id={clubId}&nopassed=1&limit=5";
                var response = await _httpClient.GetFromJsonAsync<TasoMatchesResponse>(requestUrl);
                return response?.Matches ?? new List<TasoMatch>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"[PALLOLIITTO API NOTICE] Failed to fetch live matches: {ex.Message}");
            }
        }

        // Mock Next Upcoming Match
        return new List<TasoMatch>
        {
            new TasoMatch
            {
                HomeTeamName = "Vuoreksen Peikot",
                AwayTeamName = "Salomon Kalou AC",
                Date = DateTime.Now.AddDays(3).ToString("dd.MM.yyyy"),
                Time = "18:30",
                VenueName = "Kauppi 1 Tekonurmif Kenttä"
            }
        };
    }

    private List<TeamStats> GetFallbackStandings()
    {
        return new()
        {
            new TeamStats { Position = 1, TeamName = "Salomon Kalou AC", Played = 12, Wins = 11, Draws = 1, Losses = 0, GoalsFor = 65, GoalsAgainst = 18, Points = 34 },
            new TeamStats { Position = 2, TeamName = "Vuoreksen Peikot", Played = 12, Wins = 7, Draws = 1, Losses = 4, GoalsFor = 48, GoalsAgainst = 38, Points = 22 },
            new TeamStats { Position = 3, TeamName = "Jalismaanit", Played = 12, Wins = 7, Draws = 0, Losses = 5, GoalsFor = 41, GoalsAgainst = 39, Points = 21 },
            new TeamStats { Position = 4, TeamName = "NopeeHanska", Played = 12, Wins = 7, Draws = 0, Losses = 5, GoalsFor = 34, GoalsAgainst = 29, Points = 21 },
            new TeamStats { Position = 5, TeamName = "Paperitähdet Akatemia", Played = 12, Wins = 6, Draws = 0, Losses = 6, GoalsFor = 30, GoalsAgainst = 37, Points = 18 },
            new TeamStats { Position = 6, TeamName = "KJK", Played = 12, Wins = 4, Draws = 0, Losses = 8, GoalsFor = 40, GoalsAgainst = 54, Points = 12 },
            new TeamStats { Position = 7, TeamName = "AC Puutarhurit", Played = 12, Wins = 3, Draws = 0, Losses = 9, GoalsFor = 22, GoalsAgainst = 44, Points = 9 },
            new TeamStats { Position = 8, TeamName = "JK Kanuuna", Played = 12, Wins = 1, Draws = 2, Losses = 9, GoalsFor = 20, GoalsAgainst = 41, Points = 5 }
        };
    }
}