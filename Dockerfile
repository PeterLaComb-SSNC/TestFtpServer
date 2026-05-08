FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:8a90a473da5205a16979de99d2fc20975e922c68304f5c79d564e666dc3982fc AS build
WORKDIR /App

# Copy project
COPY ./TestFtpServer.AppHost/*.csproj ./TestFtpServer.AppHost/
COPY ./TestFtpServer.SftpGo.Server/*.csproj ./TestFtpServer.SftpGo.Server/
COPY ./TestFtpServer.SftpGo.Tests/*.csproj ./TestFtpServer.SftpGo.Tests/
COPY ./TestFtpServer.SftpGo.Users/*.csproj ./TestFtpServer.SftpGo.Users/
COPY ./*.sln ./

# Restore as distinct layers
RUN dotnet restore

# Copy everything
COPY ./ ./

# Build as distinct layer
RUN dotnet build -c Release --no-restore --no-logo

# Build and publish a release
RUN dotnet publish ./TestFtpServer.SftpGo.Users/TestFtpServer.SftpGo.Users.csproj -o out -c Release --no-restore --no-logo --no-build

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:55e37c7795bfaf6b9cc5d77c155811d9569f529d86e20647704bc1d7dd9741d4
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TestFtpServer.SftpGo.Users.dll"]
