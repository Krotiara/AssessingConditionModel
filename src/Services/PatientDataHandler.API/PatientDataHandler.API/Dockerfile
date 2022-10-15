#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/PatientDataHandler.API/PatientDataHandler.API/PatientDataHandler.API.csproj", "src/Services/PatientDataHandler.API/PatientDataHandler.API/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["src/Services/PatientDataHandler.API/PatientDataHandler.API.Entities/PatientDataHandler.API.Entities.csproj", "src/Services/PatientDataHandler.API/PatientDataHandler.API.Entities/"]
COPY ["src/Services/PatientDataHandler.API/PatientDataHandler.API.Messaging.Receive/PatientDataHandler.API.Messaging.Receive.csproj", "src/Services/PatientDataHandler.API/PatientDataHandler.API.Messaging.Receive/"]
COPY ["src/Services/PatientDataHandler.API/PatientDataHandler.API.Service/PatientDataHandler.API.Service.csproj", "src/Services/PatientDataHandler.API/PatientDataHandler.API.Service/"]
COPY ["src/Services/PatientDataHandler.API/PatientDataHandler.API.Messaging.Send/PatientDataHandler.API.Messaging.Send.csproj", "src/Services/PatientDataHandler.API/PatientDataHandler.API.Messaging.Send/"]
RUN dotnet restore "src/Services/PatientDataHandler.API/PatientDataHandler.API/PatientDataHandler.API.csproj"
COPY . .
WORKDIR "/src/src/Services/PatientDataHandler.API/PatientDataHandler.API"
RUN dotnet build "PatientDataHandler.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientDataHandler.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientDataHandler.API.dll"]