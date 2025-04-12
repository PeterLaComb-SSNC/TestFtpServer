# TestFtpServer

This project is a set of .NET Aspire resources for testing SFTP server connections. It makes use of [SFTPGo](https://sftpgo.com/) as the server.

## Usage

- This project can be run directly. This allows anything running on the host to use this application as a standalone SFTP server for testing.
- This project can also be added as NUGET packages to your Aspire AppHost to allow projects within an Aspire solution to use this as an SFTP server.
  - [Server Configuration Details](./TestFtpServer.SftpGo.Server/README.MD)
  - [User Configuration Details](./TestFtpServer.SftpGo.Users/README.MD)

## SFTPGo

- [Documentation](https://docs.sftpgo.com/latest/)
- [GitHub](https://github.com/drakkan/sftpgo)
- [DockerHub](https://hub.docker.com/r/drakkan/sftpgo)
- [GitHub Container Registry](https://github.com/drakkan/sftpgo/pkgs/container/sftpgo)

## Building

Tasks in the [`.vscode`](.vscode/) folder are configured to work with [podman desktop](https://podman-desktop.io/).
With that installed, you should be able to launch with the default task.

## Release Notes

- Initial Release
  - .NET 8.0 Compatible
  - SFTPGo 2.6
