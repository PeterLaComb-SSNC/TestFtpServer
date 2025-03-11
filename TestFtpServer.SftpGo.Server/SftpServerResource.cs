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
    /// <summary>
    /// Name of the endpoint where the UI is exposed.
    /// </summary>
     public const string HttpEndpointName = "http";

     /// <summary>
     /// Name of the endpoint where the SFTP server is exposed.
     /// </summary>
     public const string SftpEndpointName = "sftp";
}
