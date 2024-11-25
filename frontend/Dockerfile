FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY mover.sln .
COPY frontend/frontend.csproj ./frontend/
COPY shared/shared.csproj ./shared/

RUN dotnet restore ./frontend/frontend.csproj

COPY frontend/ ./frontend/
COPY shared/ ./shared/

WORKDIR /src/frontend
RUN dotnet publish -c Release -o /app/dist

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/dist/wwwroot ./
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
