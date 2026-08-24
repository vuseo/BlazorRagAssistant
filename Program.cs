using BlazorRagAssistant.Components;
using BlazorRagAssistant.Services;
using Google.GenAI; // 🚀 Use the official production SDK namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ==========================================
// 🚀 NATIVE GOOGLE AI CONFIGURATION
// ==========================================

// Pull the API key securely from configuration instead of hardcoding it
string? apiKey = builder.Configuration["GoogleAI:ApiKey"];

if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("Google AI API Key is missing. Please set the 'GoogleAI:ApiKey' user secret.");
}

// Register the official Google Client as a singleton
builder.Services.AddSingleton(new Client(apiKey: apiKey));

// Register your Custom RAG Service
builder.Services.AddScoped<RagService>();

//Document Service
builder.Services.AddScoped<IDocumentService, DocumentService>();

// ==========================================

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();