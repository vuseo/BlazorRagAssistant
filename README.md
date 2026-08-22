# Blazor RAG Assistant (Proof of Concept)

An enterprise-ready, high-performance Blazor Server application demonstrating a robust Retrieval-Augmented Generation (RAG) pipeline utilizing the official production **Google.GenAI** SDK. 

This project showcases clean architectural separation, production-grade API mapping, and strict credential security workflows required for enterprise AI software development.

## 🛠️ Tech Stack & Architecture
- **Framework:** .NET Core / Blazor Server (Interactive Server Mode)
- **AI Core:** Google Gemini 2.5 Flash Engine (`Google.GenAI` SDK)
- **Containerization:** Docker & Docker Compose
- **Design Pattern:** Dependency Injection with fully decoupled Service Architecture (`RagService`)

## 🚀 Key Architectural Highlights
- **Production-Ready SDK Mapping:** Bypasses experimental or legacy connector wrappers by targeting native Google GenAI production endpoints directly for 100% stable HTTP message routing.
- **Fail-Safe Context Retrieval:** Features a clean, keyword-proximity scoring mechanism that extracts domain-specific data section-by-section (`club-rules.txt`), featuring a dynamic full-document fallback context window loop.
- **Enterprise-Grade Secret Isolation:** Enforces zero hardcoded credential exposure, utilizing the `.NET Secret Manager` for local execution and environment abstraction for containerized runtime.

## 🐳 Enterprise Containerization (Docker)

This application is fully containerized using a multi-stage Docker build and managed via Docker Compose. This completely eliminates the need to have the .NET 10 SDK or local development tools pre-installed on the host machine, matching modern cloud-native deployment standards.

### Prerequisites
- **Docker Desktop** running on your host machine.

### Production-Grade Secret Injection
To maintain strict credential isolation without relying on local `.NET User Secrets` inside a container, the runtime architecture injects your Google GenAI credentials safely via environment variable mapping. The container configuration automatically maps host settings directly into the `.NET Core` configuration provider hierarchy safely.

### How to Spin Up the Stack Locally

1. Create a `.env` file in the root directory (this file is pre-configured in `.gitignore` and safely isolated from source control) and append your credential token:
   ```env
   GOOGLE_API_KEY=your_actual_gemini_api_key_here
Open your terminal in the root solution directory and execute the orchestrated container build:

```Bash
docker compose up --build
```
The application will instantly compile within an isolated Linux image layer and begin routing traffic. Navigate your browser to:
👉 http://localhost:8080

📦 Traditional Local Configuration & Setup (Alternative)
If you prefer to run the application natively via the CLI or Visual Studio without Docker:

Clone the repository to your environment.

Open your terminal in the root project folder and initialize the local user secrets vault:

```bash
dotnet user-secrets init
```
Set your Google AI Studio API key inside the local system secrets store:

```Bash
dotnet user-secrets set "GoogleAI:ApiKey" "your_actual_gemini_api_key_here"
```
Restore dependencies and launch the application profile:

```Bash
dotnet run
```
