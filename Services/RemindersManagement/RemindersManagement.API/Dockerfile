ARG VERSION=3.1-alpine3.12

# Define build environment and working directory
FROM mcr.microsoft.com/dotnet/core/sdk:${VERSION} AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c release -o out

# Define runtime environment and working directory
FROM mcr.microsoft.com/dotnet/core/aspnet:${VERSION}

# Execute without root previleges
RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app

# Copy build result and asset to folder
WORKDIR /app
COPY --from=build /app/out .

# Define environment variable
ENV DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:8000

# Define port and entry point for running docker
EXPOSE 8000
ENTRYPOINT ["dotnet", "RemindersManagement.API.dll"]