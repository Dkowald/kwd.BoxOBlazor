#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS buildPrep
#typescript needs node.
RUN apt-get update
RUN apt-get install -y npm
WORKDIR /src
COPY NuGet.Config .

FROM buildPrep AS build
#typescript needs node.
RUN apt-get update
RUN apt-get install -y npm
WORKDIR /src
COPY NuGet.Config .
COPY ["kwd.BoxOBlazor.Server/kwd.BoxOBlazor.Server.csproj", "kwd.BoxOBlazor.Server/"]
COPY ["kwd.BoxOBlazor/kwd.BoxOBlazor.csproj", "kwd.BoxOBlazor/"]
COPY ["kwd.BoxOBlazor.Demo/kwd.BoxOBlazor.Demo.csproj", "kwd.BoxOBlazor.Demo/"]
COPY ["kwd.BoxOBlazor.Hosting/kwd.BoxOBlazor.Hosting.csproj", "kwd.BoxOBlazor.Hosting/"]

RUN dotnet restore "kwd.BoxOBlazor.Server/kwd.BoxOBlazor.Server.csproj"
COPY . .
WORKDIR "/src/kwd.BoxOBlazor.Server"
RUN dotnet build "kwd.BoxOBlazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kwd.BoxOBlazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kwd.BoxOBlazor.Server.dll"]