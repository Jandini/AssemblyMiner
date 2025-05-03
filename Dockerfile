# Use .NET 9 SDK image to build and run
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and build
COPY src/ ./src/
WORKDIR /app/src/AssemblyMiner
RUN dotnet publish -c Release -o /publish /p:DisableGitVersionTask=true

# Final image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /publish .

# Default entrypoint, parameters are passed at runtime
ENTRYPOINT ["dotnet", "AssemblyMiner.dll"]
