using Aspire.Hosting.ApplicationModel;

namespace TestFtpServer.SftpGo.Server;

/// <summary>
/// An Aspire resource for SFTPGo Server
/// </summary>
/// <param name="name">The name of the component</param>
public sealed class SftpServerResource(
    string name
) : ContainerResource(name)
{
     internal const string HttpEndpointName = "http";
     internal const string SftpEndpointName = "sftp";
}
