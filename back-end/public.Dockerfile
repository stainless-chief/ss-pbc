FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "SSPBC/SSPBC.csproj"
COPY . .
WORKDIR "/src/SSPBC"
RUN dotnet build "SSPBC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SSPBC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SSPBC.dll"]