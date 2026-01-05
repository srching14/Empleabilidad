FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY backend/OnlineCoursePlatform.API/*.csproj ./backend/OnlineCoursePlatform.API/
COPY backend/OnlineCoursePlatform.Application/*.csproj ./backend/OnlineCoursePlatform.Application/
COPY backend/OnlineCoursePlatform.Domain/*.csproj ./backend/OnlineCoursePlatform.Domain/
COPY backend/OnlineCoursePlatform.Infrastructure/*.csproj ./backend/OnlineCoursePlatform.Infrastructure/
COPY tests/OnlineCoursePlatform.Tests/*.csproj ./tests/OnlineCoursePlatform.Tests/

RUN dotnet restore ./backend/OnlineCoursePlatform.API/OnlineCoursePlatform.API.csproj

# Copy everything else and build
COPY backend/ ./backend/
COPY tests/ ./tests/

WORKDIR /app/backend/OnlineCoursePlatform.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "OnlineCoursePlatform.API.dll"]
