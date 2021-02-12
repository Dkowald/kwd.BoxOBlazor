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
WORKDIR /src
COPY ["kwd.BoxOBlazor.Browser.Host/kwd.BoxOBlazor.Browser.Host.csproj", "kwd.BoxOBlazor.Browser.Host/"]
COPY ["kwd.BoxOBlazor.Browser/kwd.BoxOBlazor.Browser.csproj", "kwd.BoxOBlazor.Browser/"]
RUN dotnet restore "kwd.BoxOBlazor.Browser.Host/kwd.BoxOBlazor.Browser.Host.csproj"
COPY . .
WORKDIR "/src/kwd.BoxOBlazor.Browser.Host"
RUN dotnet build "kwd.BoxOBlazor.Browser.Host.csproj" -v normal -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kwd.BoxOBlazor.Browser.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kwd.BoxOBlazor.Browser.Host.dll"]