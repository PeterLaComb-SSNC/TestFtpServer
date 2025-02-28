using Aspire.Hosting.ApplicationModel;

using TestFtpServer.SftpGo.Server;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Aspire.Hosting;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A set of extensions to integrate this with .NET Aspire
/// </summary>
public static class SftpServerResourceExtensions
{
    /// <summary>
    /// Adds an SFTPGo server
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="name">Optional: Defaults to `SftpServer`.</param>
    /// <param name="httpPort">Optional: Used to overide the HTTP port where the UI is presented.</param>
    /// <param name="adminUser">Optional: 'admin' will be used when missing.</param>
    /// <param name="adminPassword">Optional: If provided, the default admin will be created.</param>
    /// <param name="version">Optional: Use to specify which version of the 
    ///  <inheritdoc cref="SftpServerContainerImageTags.Image"/> container image will be used.
    ///  Defaults to <inheritdoc cref="SftpServerContainerImageTags.Tag"/>.</param>
    /// <returns>The application builder on which this method was invoked.</returns>
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
                if (adminPassword is { })
                {
                    env.EnvironmentVariables.Add("SFTPGO_DATA_PROVIDER__CREATE_DEFAULT_ADMIN", "1");
                    if (adminUser is { })
                    {
                        env.EnvironmentVariables.Add("SFTPGO_DEFAULT_ADMIN_USERNAME", adminUser.Resource);
                    }
                    else
                    {
                        env.EnvironmentVariables.Add("SFTPGO_DEFAULT_ADMIN_USERNAME", "admin");
                    }
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
