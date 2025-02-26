using Aspire.Hosting.ApplicationModel;

namespace TestFtpServer.SftpGo.Server;

public sealed class SftpServerResource(
    string name
) : ContainerResource(name)
{
     internal const string HttpEndpointName = "http";
     internal const string SftpEndpointName = "sftp";
}
