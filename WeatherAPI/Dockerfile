FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WeatherAPI/WeatherAPI.csproj", "WeatherAPI/"]
COPY ["BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["DatabaseAccess/DatabaseAccess.csproj", "DatabaseAccess/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "WeatherAPI/WeatherAPI.csproj"
COPY . .
WORKDIR "/src/WeatherAPI"
RUN dotnet build "WeatherAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherAPI.dll"]
