#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/ContainerRegister.API/ContainerRegister.API/ContainerRegister.API.csproj", "src/Services/ContainerRegister.API/ContainerRegister.API/"]
COPY ["src/Services/ContainerRegister.API/ContainerRegister.API.Service/ContainerRegister.API.Service.csproj", "src/Services/ContainerRegister.API/ContainerRegister.API.Service/"]
RUN dotnet restore "src/Services/ContainerRegister.API/ContainerRegister.API/ContainerRegister.API.csproj"
COPY . .
WORKDIR "/src/src/Services/ContainerRegister.API/ContainerRegister.API"
RUN dotnet build "ContainerRegister.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContainerRegister.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContainerRegister.API.dll"]