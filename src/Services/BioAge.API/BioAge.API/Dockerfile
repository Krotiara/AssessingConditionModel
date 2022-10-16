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
COPY ["src/Services/BioAge.API/BioAge.API/BioAge.API.csproj", "src/Services/BioAge.API/BioAge.API/"]
COPY ["src/Services/BioAge.API/BioAge.API.Service/BioAge.API.Service.csproj", "src/Services/BioAge.API/BioAge.API.Service/"]
COPY ["src/Services/BioAge.API/BioAge.API.Entites/BioAge.API.Entites.csproj", "src/Services/BioAge.API/BioAge.API.Entites/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
RUN dotnet restore "src/Services/BioAge.API/BioAge.API/BioAge.API.csproj"
COPY . .
WORKDIR "/src/src/Services/BioAge.API/BioAge.API"
RUN dotnet build "BioAge.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BioAge.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BioAge.API.dll"]