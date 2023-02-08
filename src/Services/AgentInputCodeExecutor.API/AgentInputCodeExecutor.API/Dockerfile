#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.csproj", "src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API/"]
COPY ["src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.Service/AgentInputCodeExecutor.API.Service.csproj", "src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.Service/"]
COPY ["src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.Interfaces/AgentInputCodeExecutor.API.Interfaces.csproj", "src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.Interfaces/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.Entities/AgentInputCodeExecutor.API.Entities.csproj", "src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.Entities/"]
RUN dotnet restore "src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API.csproj"
COPY . .
WORKDIR "/src/src/Services/AgentInputCodeExecutor.API/AgentInputCodeExecutor.API"
RUN dotnet build "AgentInputCodeExecutor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgentInputCodeExecutor.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgentInputCodeExecutor.API.dll"]