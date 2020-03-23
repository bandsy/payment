FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY payment.sln ./
COPY payment.data/*.csproj ./payment.data/
COPY payment.api/*.csproj ./payment.api/
COPY payment.unit-tests/*.csproj ./payment.unit-tests/

RUN dotnet restore
COPY . .
WORKDIR /src/payment.data
RUN dotnet build -c Release -o /app

WORKDIR /src/payment.api
RUN dotnet build -c Release -o /app

WORKDIR /src/payment.unit-tests
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "payment.api.dll"]