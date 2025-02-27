# TestFtpServer.SftpGo.Server - a .NET Aspire component for SFTPGo Server

This component allows you to quickly add and configure an SFTPGo server in
your .NET Aspire application.

## Usage

***This is intended for development purposes only***
Please do not run this directly in a public-facing location - it is not secure by default.

No configuration is required to get started. The UI link from the Aspire
dashboard will allow you to create an administrator login and you can go from there.

### Optional Configuration

- Create a default Admin User:

  If you pass an `IResourceBuilder<ParameterResource>` for both the `adminUser` and `adminPassword`,
  the application will configure those before the component starts.
- `httpPort` for the UI can also be customized in typical fashion for Aspire components.
- Other Configuration can be added via environment variables.

  Please refer to the [SFTPGo Documentation](https://docs.sftpgo.com/2.6/config-file/) for details, and note the `SFTPGO_` prefix, upper-case naming requirement, and use of double-underscores to configure nested properties.

## Release Notes

- v0.0.1:
  Initial Release of the SFTPGo server component for .NET Aspire
