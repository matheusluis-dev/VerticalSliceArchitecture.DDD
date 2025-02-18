# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the solution and project files
COPY ["VerticalSliceArchitecture.DDD.sln", "."]
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]

COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["tests/Application.Architecture.Tests/Application.Architecture.Tests.csproj", "tests/Application.Architecture.Tests/"]
COPY ["tests/Application.Common.Tests/Application.Common.Tests.csproj", "tests/Application.Common.Tests/"]
COPY ["tests/Application.EndToEnd.Tests/Application.EndToEnd.Tests.csproj", "tests/Application.EndToEnd.Tests/"]
COPY ["tests/Application.Integration.Tests/Application.Integration.Tests.csproj", "tests/Application.Integration.Tests/"]
COPY ["tests/Application.Unit.Tests/Application.Unit.Tests.csproj", "tests/Application.Unit.Tests/"]

# Restore dependencies
RUN dotnet restore "VerticalSliceArchitecture.DDD.sln"

# Copy the rest of the application code
COPY . .

# Build the project
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
