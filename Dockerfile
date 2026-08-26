FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:e1ffd2a92ae84c1291bc1b6887501f8af98e6331e7af6d4c8d37168c5e87a64c AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:a4556ed033fa96f984bb7a8d348851cb2d36b1281dd2420070045f664fbb5f94
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TestFtpServer.SftpGo.Users.dll"]
