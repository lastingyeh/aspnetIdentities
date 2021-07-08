FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["MvcClient/MvcClient.csproj", "MvcClient/"]
RUN dotnet restore "MvcClient/MvcClient.csproj"
COPY . .
WORKDIR "/src/MvcClient"
RUN dotnet build "MvcClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MvcClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MvcClient.dll"]
