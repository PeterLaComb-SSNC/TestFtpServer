FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:483d6f3faa583c93d522c4ca9ee54e08e535cb112dceb252b2fbb7ef94839cc8 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore -p:BuildWhat=Container
# Build and publish a release
RUN dotnet publish -o out -p:BuildWhat=Container

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:e223bd5d93b3042215c7aed59568933631121f7ff4f5268a5092ab54a7e20136
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "TestFtpServer.SftpGo.Users.dll"]
