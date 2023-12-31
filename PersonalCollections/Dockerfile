#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PersonalCollections/PersonalCollections.csproj", "PersonalCollections/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "PersonalCollections/PersonalCollections.csproj"
COPY . .
WORKDIR "/src/PersonalCollections"
RUN dotnet build "PersonalCollections.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonalCollections.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalCollections.dll"]