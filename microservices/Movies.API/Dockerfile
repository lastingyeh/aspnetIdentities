#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["microservices/Movies.API/Movies.API.csproj", "microservices/Movies.API/"]
RUN dotnet restore "microservices/Movies.API/Movies.API.csproj"
COPY . .
WORKDIR "/src/microservices/Movies.API"
RUN dotnet build "Movies.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movies.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.API.dll"]
