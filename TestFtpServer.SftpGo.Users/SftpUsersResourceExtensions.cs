using Aspire.Hosting.ApplicationModel;

using TestFtpServer.SftpGo.Server;
using TestFtpServer.SftpGo.Users.Aspire;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Aspire.Hosting;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A set of extensions to integrate this with .NET Aspire
/// </summary>
public static class SftpUsersResourceExtensions
{
    /// <summary>
    /// Adds users to the SFTPGo server via the `SFTPGO_DATA_PROVIDER__PRE_LOGIN_HOOK`
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="name">Optional: Name for the Aspire resource. Defaults to `SftpUsers`.</param>
    /// <param name="httpPort">Port on which the API will be exposed</param>
    /// <param name="registry">Registry for the container image. Defaults to `SftpUsersContainerImageTags.Registry`.</param>
    /// <param name="version">Optional: Use to specify which version of the 
    ///  <inheritdoc cref="SftpUsersContainerImageTags.Image"/> container image will be used.
    ///  Defaults to <inheritdoc cref="SftpUsersContainerImageTags.Tag"/>.</param>
    /// <returns></returns>
    public static IResourceBuilder<SftpServerResource> WithUserRepository(
        this IResourceBuilder<SftpServerResource> builder,
        string name = "SftpUsers",
        int? httpPort = null,
        string? registry = null,
        string? version = null
    )
    {
#if DEBUG
        var sftpUsers = builder
            .ApplicationBuilder
            .AddProject(
                "TestFtpServer.SftpGo.Users".Replace('.', '-'),
                "../TestFtpServer.SftpGo.Users/TestFtpServer.SftpGo.Users.csproj"
            )
            .WithParentRelationship(builder)
            .WithExternalHttpEndpoints()
            .WithHttpEndpoint(
                port: httpPort,
                name: SftpUsersResource.HttpEndpointName
            )
        ;

        return builder
            .WithEnvironment(
                async env =>
                    env.EnvironmentVariables.Add(
                        "SFTPGO_DATA_PROVIDER__PRE_LOGIN_HOOK",
                        sftpUsers.Resource.GetEndpoint("http").Property(EndpointProperty.Url)
                    )
            )
            .WithReference(sftpUsers)
            .WaitFor(sftpUsers)
            ;

#else
        var resource = new SftpUsersResource(name);
        var result = builder.ApplicationBuilder.AddResource(resource)
            .WithParentRelationship(builder)
            .WithImage(SftpUsersContainerImageTags.Image)
            .WithImageRegistry(registry ?? SftpUsersContainerImageTags.Registry)
            .WithImageTag(version ?? SftpUsersContainerImageTags.Tag)
            .WithHttpEndpoint(
                targetPort: 8080,
                port: httpPort,
                name: SftpUsersResource.HttpEndpointName
            )
            ;

        return builder
            .WithEnvironment(
                env =>
                    env.EnvironmentVariables.Add(
                        "SFTPGO_DATA_PROVIDER__PRE_LOGIN_HOOK",
                        result.Resource.ConnectionStringExpression
                    )
            )
            .WithReference(result)
            .WaitFor(result)
            ;
#endif

    }
}
