FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Library.Api/Library.Api.csproj", "Library.Api/"]
COPY ["Library.Infrastructure/Library.Infrastructure.csproj", "Library.Infrastructure/"]
COPY ["Library.Domain/Library.Domain.csproj", "Library.Domain/"]
COPY ["Library.Application/Library.Application.csproj", "Library.Application/"]

RUN dotnet restore "Library.Api/Library.Api.csproj"
COPY . .
WORKDIR "/src/Library.Api"
RUN dotnet build "Library.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Library.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
# Create the Uploads directory and set permissions
RUN mkdir -p /app/Uploads && chmod -R 777 /app/Uploads
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Library.Api.dll"]
