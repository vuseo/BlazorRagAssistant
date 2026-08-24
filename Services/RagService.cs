using Google.GenAI;

namespace BlazorRagAssistant.Services;

public class RagService
{
    private readonly Client _aiClient;
    private readonly IDocumentService _documentService;

    // 🚀 Production Engine Target
    private const string ModelName = "gemini-2.5-flash";

    // Inject IDocumentService alongside the Google GenAI Client
    public RagService(Client aiClient, IDocumentService documentService)
    {
        _aiClient = aiClient;
        _documentService = documentService;
    }

    public async Task InitializeKnowledgeBaseAsync()
    {
        // Handled dynamically on every query call to guarantee fresh context!
        await Task.CompletedTask;
    }

    public async Task<(string Answer, string ContextUsed)> AskQuestionAsync(string userQuestion)
    {
        try
        {
            // 1. Fetch live text from IDocumentService (picks up any edits/uploads from the Data page)
            string fullText = await _documentService.GetDocumentContentAsync();

            if (string.IsNullOrWhiteSpace(fullText))
            {
                return ("No knowledge base content found. Please upload or add content in the Data tab.", "Empty context file.");
            }

            // 2. Chunk the fresh document text
            var knowledgeChunks = fullText.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            string context = "No relevant context found.";

            // 3. Keyword proximity scoring
            var keywords = userQuestion.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var bestMatch = knowledgeChunks
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
            else if (knowledgeChunks.Any())
            {
                // 🚀 FOOLPROOF FALLBACK: Pass full context if keyword matching fails
                context = string.Join("\n\n", knowledgeChunks);
            }

            string prompt = $"""
            You are a helpful company assistant. Answer the user's question accurately using ONLY the provided context below. 
            If the answer cannot be found or inferred from the context, state: "I cannot find that information in the club rules."

            [CONTEXT]
            {context}

            [USER QUESTION]
            {userQuestion}
            """;

            // 4. Send request to Gemini API
            var response = await _aiClient.Models.GenerateContentAsync(
                model: ModelName,
                contents: prompt
            );

            string generatedText = response.Text ?? "No response text found.";

            return (generatedText, context);
        }
        catch (Exception ex)
        {
            return ($"Google AI Connection Error: {ex.Message}", "Failed context retrieval.");
        }
    }
}