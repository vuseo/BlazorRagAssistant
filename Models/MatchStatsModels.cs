namespace BlazorRagAssistant.Models;

public class TeamStats
{
    public int Position { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int Played { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int Points { get; set; }

    public int GoalDifference => GoalsFor - GoalsAgainst;
    public double AvgGoalsScored => Played > 0 ? (double)GoalsFor / Played : 0;
    public double AvgGoalsConceded => Played > 0 ? (double)GoalsAgainst / Played : 0;
}

public class MatchPredictionResult
{
    public string TargetTeam { get; set; } = string.Empty;
    public string OpponentTeam { get; set; } = string.Empty;
    public double TargetWinProbability { get; set; }
    public double DrawProbability { get; set; }
    public double OpponentWinProbability { get; set; }
    public string PredictedScore { get; set; } = string.Empty;
    public string TacticalAnalysis { get; set; } = string.Empty;
}