using Google.GenAI;

namespace BlazorRagAssistant.Services;

public class RagService
{
    private readonly Client _aiClient;
    private readonly List<string> _knowledgeChunks = new();
    private bool _isInitialized = false;

    // 🚀 THE CRITICAL FIX: Upgrade from the retired 1.5 flash string to the 2.5 production engine
    private const string ModelName = "gemini-2.5-flash";

    public RagService(Client aiClient)
    {
        _aiClient = aiClient;
    }

    public async Task InitializeKnowledgeBaseAsync()
    {
        if (_isInitialized) return;

        try
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "club-rules.txt");
            if (!File.Exists(filePath)) return;

            string fullText = await File.ReadAllTextAsync(filePath);
            var sections = fullText.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            _knowledgeChunks.Clear();
            _knowledgeChunks.AddRange(sections);

            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RAG INIT ERROR]: {ex.Message}");
        }
    }

    public async Task<(string Answer, string ContextUsed)> AskQuestionAsync(string userQuestion)
    {
        try
        {
            if (!_isInitialized)
            {
                await InitializeKnowledgeBaseAsync();
            }

            string context = "No relevant context found.";

            // 1. Try a broad search by splitting words
            var keywords = userQuestion.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var bestMatch = _knowledgeChunks
                .Select(chunk => new {
                    Chunk = chunk,
                    Score = keywords.Count(kw => chunk.Contains(kw, StringComparison.OrdinalIgnoreCase))
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (bestMatch != null)
            {
                context = bestMatch.Chunk;
            }
            else if (_knowledgeChunks.Any())
            {
                // 🚀 THE FOOLPROOF FALLBACK: If keyword extraction fails, feed the entire document!
                // Gemini 2.5 Flash has a massive context window and handles the full file effortlessly.
                context = string.Join("\n\n", _knowledgeChunks);
            }

            string prompt = $"""
            You are a helpful company assistant. Answer the user's question accurately using ONLY the provided context below. 
            If the answer cannot be found or inferred from the context, state: "I cannot find that information in the club rules."

            [CONTEXT]
            {context}

            [USER QUESTION]
            {userQuestion}
            """;

            // Execute using the official production client endpoints
            var response = await _aiClient.Models.GenerateContentAsync(
                model: ModelName,
                contents: prompt
            );

            // Access the string property natively provided by the official SDK response model wrapper
            string generatedText = response.Text ?? "No response text found.";

            return (generatedText, context);
        }
        catch (Exception ex)
        {
            return ($"Google AI Connection Error: {ex.Message}", "Failed context retrieval.");
        }
    }
}