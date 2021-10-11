FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /src
COPY ./src/Api/*.csproj ./Api/

FROM build-env AS restore
RUN dotnet restore ./Api/Api.csproj

FROM restore AS publish
COPY ./src ./
RUN dotnet publish ./Api/Api.csproj -c Release -o dist

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as stage
EXPOSE 80
EXPOSE 443

FROM stage
WORKDIR /app
COPY --from=publish /src/dist .
ENTRYPOINT ["dotnet", "Api.dll"]