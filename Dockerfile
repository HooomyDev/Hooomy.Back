FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Hooome.Backend.sln ./
COPY Hooome.WebApi/Hooome.WebApi.csproj Hooome.WebApi/
COPY Hooome.Application/Hooome.Application.csproj Hooome.Application/
COPY Hooome.Domain/Hooome.Domain.csproj Hooome.Domain/
COPY Hooomy.Persistance/Hooome.Persistance.csproj Hooomy.Persistance/

RUN dotnet restore Hooome.Backend.sln

COPY . .
RUN dotnet publish Hooome.WebApi/Hooome.WebApi.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/internal-data && chmod 755 /app/internal-data
COPY --chmod=644 Hooome.WebApi/static/minsk_addresses.csv /app/internal-data/

RUN echo "=== CSV file check ===" && \
    ls -la /app/static/ && \
    wc -l /app/static/minsk_addresses.csv

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}
ENTRYPOINT ["dotnet", "Hooome.WebApi.dll"]
