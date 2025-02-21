FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AppAvailabilityTracker.Services.AppStorage.Web/AppAvailabilityTracker.Services.AppStorage.Web.csproj", "AppAvailabilityTracker.Services.AppStorage.Web/"]
RUN dotnet restore "AppAvailabilityTracker.Services.AppStorage.Web/AppAvailabilityTracker.Services.AppStorage.Web.csproj"
COPY . .
WORKDIR "/src/AppAvailabilityTracker.Services.AppStorage.Web"
RUN dotnet build "AppAvailabilityTracker.Services.AppStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AppAvailabilityTracker.Services.AppStorage.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppAvailabilityTracker.Services.AppStorage.Web.dll"]
