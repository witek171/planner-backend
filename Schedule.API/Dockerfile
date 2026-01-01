# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Schedule.API/Schedule.API.csproj", "Schedule.API/"]
COPY ["Schedule.Application/Schedule.Application.csproj", "Schedule.Application/"]
COPY ["Schedule.Domain/Schedule.Domain.csproj", "Schedule.Domain/"]
COPY ["Schedule.Infrastructure/Schedule.Infrastructure.csproj", "Schedule.Infrastructure/"]
COPY ["Schedule.Contracts/Schedule.Contracts.csproj", "Schedule.Contracts/"]
COPY ["Common/ProjectBase.props", "Common/"]

RUN dotnet restore "Schedule.API/Schedule.API.csproj"
COPY . .
WORKDIR "/src/Schedule.API"
RUN dotnet build "Schedule.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Schedule.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Schedule.API.dll"]