# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM registry.access.redhat.com/ubi8/dotnet-80-runtime:8.0-2 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
FROM registry.access.redhat.com/ubi8/dotnet-80:8.0-2 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#COPY ["TaskManagerApp/TaskManagerAppNotes.csproj", "TaskManagerApp/"]
COPY --chown=1001:0 . .
RUN dotnet restore "./TaskManagerApp/TaskManagerAppNotes.csproj"
#COPY --chown=1001:0 . .
WORKDIR "/src/TaskManagerApp"
RUN dotnet build "./TaskManagerAppNotes.csproj" -c $BUILD_CONFIGURATION -o /src/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskManagerAppNotes.csproj" -c $BUILD_CONFIGURATION -o /src/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --chown=1001:0 --from=publish /src/publish .
COPY --chown=1001:0 taskdb.db /tmp/taskdb.db
ENTRYPOINT ["dotnet", "TaskManagerAppNotes.dll"]