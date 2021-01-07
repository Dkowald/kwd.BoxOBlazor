#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
#typescript needs node.
RUN apt-get update
RUN apt-get install -y npm
WORKDIR /src
COPY ["ForBrowser.Host/ForBrowser.Host.csproj", "ForBrowser.Host/"]
RUN dotnet restore "ForBrowser.Host/ForBrowser.Host.csproj"
COPY . .
WORKDIR "/src/ForBrowser.Host"
RUN dotnet build "ForBrowser.Host.csproj" -v normal -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForBrowser.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForBrowser.Host.dll"]