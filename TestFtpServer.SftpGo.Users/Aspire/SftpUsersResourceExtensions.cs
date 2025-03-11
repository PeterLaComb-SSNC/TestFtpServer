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
    private const string MountFileName = "SftpGoUsers.json";
    private const string MountDirectory = "/tmp/SftpGoUsers";
    private const string MountPath = $"{MountDirectory}/{MountFileName}";

    /// <summary>
    /// Adds users to the SFTPGo server via the `SFTPGO_DATA_PROVIDER__PRE_LOGIN_HOOK`
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="name">Optional: Name for the Aspire resource. Defaults to `SftpUsers`.</param>
    /// <param name="scenarioFilePath">Optional: Path to a JSON file with the users to load.</param>
    /// <param name="httpPort">Port on which the API will be exposed</param>
    /// <param name="version">Optional: Use to specify which version of the 
    ///  <inheritdoc cref="SftpUsersContainerImageTags.Image"/> container image will be used.
    ///  Defaults to <inheritdoc cref="SftpUsersContainerImageTags.Tag"/>.</param>
    /// <returns></returns>
    public static IResourceBuilder<SftpServerResource> WithUserRepository(
        this IResourceBuilder<SftpServerResource> builder,
        string name = "SftpUsers",
        IResourceBuilder<ParameterResource>? scenarioFilePath = null,
        int? httpPort = null,
        string? version = null
    )
    {
        var resource = new SftpUsersResource(name);

        var pathToScenario = GetScenarioPath(scenarioFilePath);
        var loadCustomScenario = IsCustomScenario(pathToScenario);

        var result = builder.ApplicationBuilder.AddResource(resource)
            .WithParentRelationship(builder)
            .WithImage(SftpUsersContainerImageTags.Image)
            .WithImageRegistry(SftpUsersContainerImageTags.Registry)
            .WithImageTag(version ?? SftpUsersContainerImageTags.Tag)
            .WithEnvironment(
                env =>
                {
                    if (loadCustomScenario)
                    {
                        env.EnvironmentVariables["SFTPGO_USERS_LIST"] = MountPath;
                    }
                }
            )
            .WithHttpEndpoint(
                targetPort: 8080,
                port: httpPort,
                name: SftpUsersResource.HttpEndpointName
            )
            ;

        if (loadCustomScenario)
        {
            var tempPath = GetTempPath(name, pathToScenario!);
            result = result.WithBindMount(Path.GetDirectoryName(tempPath)!, MountDirectory, true);
        }

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

        static string? GetScenarioPath(IResourceBuilder<ParameterResource>? pathToScenarioFile) =>
            pathToScenarioFile?.Resource.Value;

        static bool IsCustomScenario(string? pathToScenarioFile) =>
            string.IsNullOrWhiteSpace(pathToScenarioFile) is false && File.Exists(pathToScenarioFile);

        static string GetTempPath(
            string instanceName,
            string pathToScenarioFile
        )
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "SftpGoUsers", instanceName, Guid.NewGuid().ToString());
            var result = Path.Combine(tempFolder, MountFileName);
            Directory.CreateDirectory(tempFolder);
            File.Copy(pathToScenarioFile, result, true);
            return result;
        }
    }
}
