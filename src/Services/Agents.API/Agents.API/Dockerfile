#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Agents.API/Agents.API/Agents.API.csproj", "src/Services/Agents.API/Agents.API/"]
COPY ["src/Services/Agents.API/Agents.API.Service/Agents.API.Service.csproj", "src/Services/Agents.API/Agents.API.Service/"]
COPY ["Interfaces/ASMLib.csproj", "Interfaces/"]
COPY ["src/Services/Agents.API/Agents.API.Data/Agents.API.Data.csproj", "src/Services/Agents.API/Agents.API.Data/"]
COPY ["src/Services/Agents.API/Agents.API.Entities/Agents.API.Entities.csproj", "src/Services/Agents.API/Agents.API.Entities/"]
COPY ["src/Services/Agents.API/Agents.API.Interfaces/Agents.API.Interfaces.csproj", "src/Services/Agents.API/Agents.API.Interfaces/"]
RUN dotnet restore "src/Services/Agents.API/Agents.API/Agents.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Agents.API/Agents.API"
RUN dotnet build "Agents.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Agents.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Agents.API.dll"]