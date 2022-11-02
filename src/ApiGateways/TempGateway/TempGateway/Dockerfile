#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /src
COPY ["src/Certificates/localhost.crt", "/usr/local/share/ca-certificates/"]
RUN chmod 644 /usr/local/share/ca-certificates/localhost.crt && update-ca-certificates

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ApiGateways/TempGateway/TempGateway/TempGateway.csproj", "src/ApiGateways/TempGateway/TempGateway/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
RUN dotnet restore "src/ApiGateways/TempGateway/TempGateway/TempGateway.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/TempGateway/TempGateway"
RUN dotnet build "TempGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TempGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TempGateway.dll"]