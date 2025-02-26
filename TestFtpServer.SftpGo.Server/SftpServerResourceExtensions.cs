using Aspire.Hosting.ApplicationModel;

using TestFtpServer.SftpGo.Server;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Aspire.Hosting;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class SftpServerResourceExtensions
{
    public static IResourceBuilder<SftpServerResource> AddSftpServer(
        this IDistributedApplicationBuilder builder,
        string name = "SftpServer",
        int? httpPort = null,
        IResourceBuilder<ParameterResource>? adminUser = null,
        IResourceBuilder<ParameterResource>? adminPassword = null,
        string version = SftpServerContainerImageTags.Tag
    )
    {
        var resource = new SftpServerResource(name);

        return builder.AddResource(resource)
            .WithImage(SftpServerContainerImageTags.Image)
            .WithImageRegistry(SftpServerContainerImageTags.Registry)
            .WithImageTag(version ?? SftpServerContainerImageTags.Tag)
            .WithEnvironment(env =>
            {
                if (adminUser is { } && adminPassword is { })
                {
                    env.EnvironmentVariables.Add("SFTPGO_DATA_PROVIDER__CREATE_DEFAULT_ADMIN", "1");
                    env.EnvironmentVariables.Add("SFTPGO_DEFAULT_ADMIN_USERNAME", adminUser.Resource);
                    env.EnvironmentVariables.Add("SFTPGO_DEFAULT_ADMIN_PASSWORD", adminPassword.Resource);
                }
            })
            .WithHttpEndpoint(
                targetPort: 8080,
                port: httpPort,
                name: SftpServerResource.HttpEndpointName
            )
            .WithEndpoint(
                targetPort: 2022,
                name: SftpServerResource.SftpEndpointName
            )
            ;
    }
}
