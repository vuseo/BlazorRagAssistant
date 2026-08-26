using Google.GenAI;
using BlazorRagAssistant.Models;

namespace BlazorRagAssistant.Services;

public class MatchPredictionService
{
    private readonly Client _aiClient;
    private const string ModelName = "gemini-2.5-flash";

    public MatchPredictionService(Client aiClient)
    {
        _aiClient = aiClient;
    }

    public async Task<MatchPredictionResult> PredictMatchAsync(TeamStats team, TeamStats opponent)
    {
        string prompt = $"""
        You are an expert soccer tactical analyst. Perform a statistical match prediction between two teams in the 'Puistofutis 6v6, Tampere' league.

        TEAM 1 ({team.TeamName}):
        - Position: {team.Position}
        - Matches Played: {team.Played} (W: {team.Wins}, D: {team.Draws}, L: {team.Losses})
        - Goal Differential: {team.GoalsFor} scored, {team.GoalsAgainst} conceded (Diff: {team.GoalDifference})
        - Points: {team.Points}

        TEAM 2 ({opponent.TeamName}):
        - Position: {opponent.Position}
        - Matches Played: {opponent.Played} (W: {opponent.Wins}, D: {opponent.Draws}, L: {opponent.Losses})
        - Goal Differential: {opponent.GoalsFor} scored, {opponent.GoalsAgainst} conceded (Diff: {opponent.GoalDifference})
        - Points: {opponent.Points}

        Based on these statistics, provide:
        1. A predicted score line (e.g., "3 - 2").
        2. Estimated Win Probability for {team.TeamName} (%), Draw (%), and {opponent.TeamName} (%).
        3. A concise tactical analysis (2-3 bullet points) recommending key tactics for {team.TeamName} to exploit {opponent.TeamName}'s defensive/offensive metrics.

        Respond in clean bullet points.
        """;

        var response = await _aiClient.Models.GenerateContentAsync(
            model: ModelName,
            contents: prompt
        );

        // Simple heuristic probability calculations based on points per game
        double ppg1 = team.Played > 0 ? (double)team.Points / team.Played : 1.0;
        double ppg2 = opponent.Played > 0 ? (double)opponent.Points / opponent.Played : 1.0;
        double totalPPG = ppg1 + ppg2 + 0.5;

        double winProb = Math.Round((ppg1 / totalPPG) * 100, 1);
        double oppWinProb = Math.Round((ppg2 / totalPPG) * 100, 1);
        double drawProb = Math.Max(0, 100 - winProb - oppWinProb);

        return new MatchPredictionResult
        {
            TargetTeam = team.TeamName,
            OpponentTeam = opponent.TeamName,
            TargetWinProbability = winProb,
            DrawProbability = drawProb,
            OpponentWinProbability = oppWinProb,
            PredictedScore = winProb > oppWinProb ? "3 - 1" : "1 - 2",
            TacticalAnalysis = response.Text ?? "Tactical analysis unavailable."
        };
    }
}