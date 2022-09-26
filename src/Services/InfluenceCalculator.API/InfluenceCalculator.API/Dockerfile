#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InfluenceCalculator.API/InfluenceCalculator.API.csproj", "InfluenceCalculator.API/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
RUN dotnet restore "InfluenceCalculator.API/InfluenceCalculator.API.csproj"
COPY . .
WORKDIR "/src/InfluenceCalculator.API"
RUN dotnet build "InfluenceCalculator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InfluenceCalculator.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InfluenceCalculator.API.dll"]
