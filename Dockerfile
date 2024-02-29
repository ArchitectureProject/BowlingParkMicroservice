FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BowlingParkMicroService/BowlingParkMicroService.csproj", "BowlingParkMicroService/"]
RUN dotnet restore "BowlingParkMicroService/BowlingParkMicroService.csproj"
COPY . .
WORKDIR "/src/BowlingParkMicroService"
RUN dotnet build "BowlingParkMicroService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BowlingParkMicroService.csproj" -c Release -o /app/publish /p:UseAppHost=false
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BowlingParkMicroService.dll"]