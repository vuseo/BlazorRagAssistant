namespace BlazorRagAssistant.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly string _filePath;

        public DocumentService(IWebHostEnvironment env)
        {
            // Points to club-rules.txt inside your project directory (or wwwroot/data)
            _filePath = Path.Combine(env.ContentRootPath, "Data", "club-rules.txt");
        }

        public async Task<string> GetDocumentContentAsync()
        {
            if (!File.Exists(_filePath))
            {
                return string.Empty;
            }
            return await File.ReadAllTextAsync(_filePath);
        }

        public async Task SaveDocumentContentAsync(string content)
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            await File.WriteAllTextAsync(_filePath, content);
        }
    }
}
