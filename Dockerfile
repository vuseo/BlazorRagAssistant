# Stage 1: Build the application using .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish the release build
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose standard container ports
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "BlazorRagAssistant.dll"]