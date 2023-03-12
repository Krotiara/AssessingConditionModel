#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Models.API/Models.API/Models.API.csproj", "src/Services/Models.API/Models.API/"]
COPY ["src/Services/Models.API/Models.API.Data/Models.API.Data.csproj", "src/Services/Models.API/Models.API.Data/"]
COPY ["src/Services/Models.API/Models.API.Entities/Models.API.Entities.csproj", "src/Services/Models.API/Models.API.Entities/"]
RUN dotnet restore "src/Services/Models.API/Models.API/Models.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Models.API/Models.API"
RUN dotnet build "Models.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Models.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Models.API.dll"]