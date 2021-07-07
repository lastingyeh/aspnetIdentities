FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
# EXPOSE 443

# ENV ASPNETCORE_URLS=https://+:443
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ApiOne/ApiOne.csproj", "ApiOne/"]
RUN dotnet restore "ApiOne/ApiOne.csproj"
COPY . .
WORKDIR "/src/ApiOne"
RUN dotnet build "ApiOne.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiOne.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiOne.dll"]
