FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:747cbade5bfd918a175ac32f8f1d931edec2c7d0d5397a3de72eaef1ac1f66d2 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:8af87eba5e7cdafb1514d7388be287365da97c132509e84dfe252ffd552398ca
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TestFtpServer.SftpGo.Users.dll"]
