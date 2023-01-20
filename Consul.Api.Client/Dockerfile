#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Consul.Api.Client/Consul.Api.Client.csproj", "Consul.Api.Client/"]
COPY ["ServiceRegistration/ServiceRegistration.csproj", "ServiceRegistration/"]
RUN dotnet restore "Consul.Api.Client/Consul.Api.Client.csproj"
COPY . .
WORKDIR "/src/Consul.Api.Client"
RUN dotnet build "Consul.Api.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consul.Api.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consul.Api.Client.dll"]