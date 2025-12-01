FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:c7445f141c04f1a6b454181bd098dcfa606c61ba0bd213d0a702489e5bd4cd71 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore -p:BuildWhat=Container
# Build and publish a release
RUN dotnet publish -o out -p:BuildWhat=Container

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:7c4246c1c384319346d45b3e24a10a21d5b6fc9b36a04790e1588148ff8055b0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TestFtpServer.SftpGo.Users.dll"]
