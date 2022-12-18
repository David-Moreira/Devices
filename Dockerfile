FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

COPY . .

RUN dotnet restore src/Device.sln

#RUN dotnet test ./tests/Device.Api.Tests/Device.Api.Tests.csproj -c Release
RUN dotnet publish src/Device.sln -c Release -o release


FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build-env /app/release .

EXPOSE 80
ENTRYPOINT ["dotnet", "Device.Api.dll"]