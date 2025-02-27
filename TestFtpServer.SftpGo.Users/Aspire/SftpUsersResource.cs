using Aspire.Hosting.ApplicationModel;

namespace TestFtpServer.SftpGo.Users.Aspire;

/// <summary>
/// Represents SFTPGo Server Users
/// </summary>
/// <param name="name"></param>
public sealed class SftpUsersResource(
    string name
) : ContainerResource(name), IResourceWithConnectionString
{
    internal const string HttpEndpointName = "http";

    // An EndpointReference is a core .NET Aspire type used for keeping
    // track of endpoint details in expressions. Simple literal values cannot
    // be used because endpoints are not known until containers are launched.
    private EndpointReference? _httpEndpointReference;

    /// <summary>
    /// The HTTP Endpoint for the application
    /// </summary>
    public EndpointReference HttpEndpoint =>
        _httpEndpointReference ??= new(this, HttpEndpointName);

    /// <inheritdoc />
    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create(
            $"{HttpEndpointName}://{HttpEndpoint.Property(EndpointProperty.Host)}:{HttpEndpoint.Property(EndpointProperty.Port)}"
        );
}
