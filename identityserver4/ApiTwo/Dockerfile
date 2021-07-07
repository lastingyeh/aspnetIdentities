FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
# EXPOSE 443

# ENV ASPNETCORE_URLS=https://+:443
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ApiTwo/ApiTwo.csproj", "ApiTwo/"]
RUN dotnet restore "ApiTwo/ApiTwo.csproj"
COPY . .
WORKDIR "/src/ApiTwo"
RUN dotnet build "ApiTwo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiTwo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiTwo.dll"]
