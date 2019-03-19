FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 5001

## REQUIRED: https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/nanoserver-1709/amd64/Dockerfile#L31
USER ContainerAdministrator 

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY WebOTronic.GameApp/WebOTronic.GameApp.csproj WebOTronic.GameApp/
RUN dotnet restore WebOTronic.GameApp/WebOTronic.GameApp.csproj
COPY . .
WORKDIR /src/WebOTronic.GameApp
RUN dotnet build WebOTronic.GameApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebOTronic.GameApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebOTronic.GameApp.dll"]
