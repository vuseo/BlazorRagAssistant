# Blazor RAG Assistant (Proof of Concept)

An enterprise-ready, high-performance Blazor Server application demonstrating a robust Retrieval-Augmented Generation (RAG) pipeline utilizing the official production **Google.GenAI** SDK. 

This project showcases clean architectural separation, production-grade API mapping, and strict credential security workflows required for enterprise AI software development.

## 🛠️ Tech Stack & Architecture
- **Framework:** .NET Core / Blazor Server (Interactive Server Mode)
- **AI Core:** Google Gemini 2.5 Flash Engine (`Google.GenAI` SDK)
- **Design Pattern:** Dependency Injection with fully decoupled Service Architecture (`RagService`)

## 🚀 Key Architectural Highlights
- **Production-Ready SDK Mapping:** Bypasses experimental or legacy connector wrappers by targeting native Google GenAI production endpoints directly for 100% stable HTTP message routing.
- **Fail-Safe Context Retrieval:** Features a clean, keyword-proximity scoring mechanism that extracts domain-specific data section-by-section (`club-rules.txt`), featuring a dynamic full-document fallback context window loop.
- **Enterprise-Grade Secret Isolation:** Enforces zero hardcoded credential exposure, utilizing the `.NET Secret Manager` to completely isolate sensitive API keys away from source control.

## 📦 Local Configuration & Setup

1. Clone the repository to your environment.
2. Open your terminal in the root project folder and initialize the local user secrets vault:
   ```bash
   dotnet user-secrets init