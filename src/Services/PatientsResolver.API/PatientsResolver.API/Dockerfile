#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/PatientsResolver.API/PatientsResolver.API/PatientsResolver.API.csproj", "src/Services/PatientsResolver.API/PatientsResolver.API/"]
COPY ["src/Services/PatientsResolver.API/PatientsResolver.API.Entities/PatientsResolver.API.Entities.csproj", "src/Services/PatientsResolver.API/PatientsResolver.API.Entities/"]
COPY ["Interfaces/ASMLib.csproj", "Interfaces/"]
COPY ["src/Services/PatientsResolver.API/PatientsResolver.API.Service/PatientsResolver.API.Service.csproj", "src/Services/PatientsResolver.API/PatientsResolver.API.Service/"]
COPY ["src/Services/PatientsResolver.API/PatientsResolver.API.Data/PatientsResolver.API.Data.csproj", "src/Services/PatientsResolver.API/PatientsResolver.API.Data/"]
RUN dotnet restore "src/Services/PatientsResolver.API/PatientsResolver.API/PatientsResolver.API.csproj"
COPY . .
WORKDIR "/src/src/Services/PatientsResolver.API/PatientsResolver.API"
RUN dotnet build "PatientsResolver.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientsResolver.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientsResolver.API.dll"]