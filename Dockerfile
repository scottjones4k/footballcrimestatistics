#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine3.12 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.12 AS dependencies
WORKDIR /src
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore "FootballCrimeStatistics/FootballCrimeStatistics.csproj"

FROM dependencies AS build
COPY . .
WORKDIR "/src/FootballCrimeStatistics"
RUN dotnet build "FootballCrimeStatistics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FootballCrimeStatistics.csproj" -c Release -o /app/publish

FROM dependencies AS test
WORKDIR /src
COPY FootballCrimeStatistics.sln ./
RUN dotnet restore "FootballCrimeStatistics.sln"
COPY . .
RUN dotnet build "FootballCrimeStatistics.sln" -c Release --no-restore
RUN dotnet test --no-build -c Release --results-directory /testresults --logger "trx;LogFileName=test_results.trx" /p:CollectCoverage=true /p:CoverletOutputFormat=json%2cCobertura /p:CoverletOutput=/testresults/coverage/ -p:MergeWith=/testresults/coverage/coverage.json  "FootballCrimeStatistics.sln"#; exit 0;

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FootballCrimeStatistics.dll"]