#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CFEMS.API/CFEMS.API.csproj", "CFEMS.API/"]
COPY ["Core.Application/Core.Application.csproj", "Core.Application/"]
COPY ["Shared.DTOs/Shared.DTOs.csproj", "Shared.DTOs/"]
COPY ["Core.Domain/Core.Domain.csproj", "Core.Domain/"]
COPY ["CFEMS.Infrastructure/CFEMS.Infrastructure.csproj", "CFEMS.Infrastructure/"]
RUN dotnet restore "CFEMS.API/CFEMS.API.csproj"
COPY . .
WORKDIR "/src/CFEMS.API"
RUN dotnet build "CFEMS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CFEMS.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CFEMS.API.dll"]