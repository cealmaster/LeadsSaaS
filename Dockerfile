# ===== Etapa de build =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar el csproj primero (cache)
COPY ["LeadsSaas.Api/LeadsSaas.Api.csproj", "LeadsSaas.Api/"]
RUN dotnet restore "LeadsSaas.Api/LeadsSaas.Api.csproj"

# Copiar todo el repo
COPY . .

WORKDIR "/src/LeadsSaas.Api"
RUN dotnet publish -c Release -o /app/publish

# ===== Etapa de runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "LeadsSaas.Api.dll"]
