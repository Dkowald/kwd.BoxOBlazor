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
COPY ["ForServer/ForServer.csproj", "ForServer/"]
COPY ["kwd.BoxOBlazor/kwd.BoxOBlazor.csproj", "kwd.BoxOBlazor/"]
COPY ["kwd.BoxOBlazor.Demo/kwd.BoxOBlazor.Demo.csproj", "kwd.BoxOBlazor.Demo/"]

RUN dotnet restore "ForServer/ForServer.csproj"
COPY . .
WORKDIR "/src/ForServer"
RUN dotnet build "ForServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForServer.dll"]