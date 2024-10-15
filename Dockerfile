FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /docker

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build --configuration Release --no-restore

RUN dotnet test --configuration Release --no-build

RUN dotnet publish --configuration Release --output /docker/bin/Release/net8.0/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /docker
COPY --from=build-env /docker/bin/Release/net8.0/publish/ .

ENTRYPOINT ["dotnet", "PasswordManager.dll"]

