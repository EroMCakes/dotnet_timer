FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TimerMicroservice.csproj", "./"]
RUN dotnet restore "TimerMicroservice.csproj"
COPY . .
RUN dotnet build "TimerMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimerMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimerMicroservice.dll"]