FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY FactoryManager.API/*.csproj FactoryManager.API/
COPY FactoryManager.Application/*.csproj FactoryManager.Application/
COPY FactoryManager.Domain/*.csproj FactoryManager.Domain/
COPY FactoryManager.Infrastructure/*.csproj FactoryManager.Infrastructure/
COPY FactoryManager.Tests/*.csproj FactoryManager.Tests/
RUN dotnet restore

COPY . .
RUN dotnet publish FactoryManager.API/FactoryManager.API.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FactoryManager.API.dll"]