FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/VersioningFE/VersioningFE.csproj", "VersioningFE/"]
RUN dotnet restore "src/VersioningFE/VersioningFE.csproj"
COPY . .
WORKDIR "/src/VersioningFE"
RUN dotnet build "VersioningFE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VersioningFE.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VersioningFE.dll"]
