FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY mover.sln .
COPY backend/backend.csproj ./backend/
COPY shared/shared.csproj ./shared/

RUN dotnet restore ./backend/backend.csproj

COPY backend/ ./backend/
COPY shared/ ./shared/

WORKDIR /src/backend
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

EXPOSE 80
ENTRYPOINT ["dotnet", "backend.dll"]
