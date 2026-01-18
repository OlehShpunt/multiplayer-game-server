# Build environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-environment

WORKDIR /app

COPY . ./
RUN dotnet restore

# Build the application
RUN dotnet publish -o out

# Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Copy the built application from the build environment
COPY --from=build-environment /app/out .

EXPOSE 5000

CMD [ "dotnet", "game-server.dll" ]