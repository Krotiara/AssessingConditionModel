#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PatientDataHandler.API/PatientDataHandler.API.csproj", "PatientDataHandler.API/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
RUN dotnet restore "PatientDataHandler.API/PatientDataHandler.API.csproj"
COPY . .
WORKDIR "/src/PatientDataHandler.API"
RUN dotnet build "PatientDataHandler.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientDataHandler.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientDataHandler.API.dll"]
#
#FROM postgresql
#ADD ./sql/create_tables.sql /docker-entrypoint-initdb.d/
