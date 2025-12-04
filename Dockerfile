# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar sln y proyectos
COPY ../LeadsSaas.sln .
COPY . ./LeadsSaas.Api/

WORKDIR /src/LeadsSaas.Api
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "LeadsSaas.Api.dll"]
