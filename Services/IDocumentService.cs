namespace BlazorRagAssistant.Services
{
    public interface IDocumentService
    {
        Task<string> GetDocumentContentAsync();
        Task SaveDocumentContentAsync(string content);
    }
}
