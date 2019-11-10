FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS base
EXPOSE 5432
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS build
COPY . /src
WORKDIR /src
RUN mkdir -p /usr/local/share/dotnet/sdk/NuGetFallbackFolder
RUN dotnet build -c Release -o /app

FROM build AS publish
WORKDIR /src
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Yugawara.dll"]
