# Build stage: compiles and publishes the application.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore AIpaca.Rumbo.Platform/AIpaca.Rumbo.Platform.csproj
RUN dotnet publish AIpaca.Rumbo.Platform/AIpaca.Rumbo.Platform.csproj \
    -c Release -o /app/publish --no-restore

# Runtime stage: ships only what is needed to run, without the SDK.
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AIpaca.Rumbo.Platform.dll"]