FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
# EXPOSE 443

# ENV ASPNETCORE_URLS=https://+:443
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["IdentityServer/IdentityServer.csproj", "IdentityServer/"]
RUN dotnet restore "IdentityServer/IdentityServer.csproj"
COPY . .
WORKDIR "/src/IdentityServer"
RUN dotnet build "IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]
