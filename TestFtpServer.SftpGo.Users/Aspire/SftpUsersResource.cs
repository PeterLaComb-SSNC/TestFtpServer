using Aspire.Hosting.ApplicationModel;

namespace TestFtpServer.SftpGo.Users.Aspire;

public sealed class SftpUsersResource(
    string name
) : ContainerResource(name), IResourceWithConnectionString
{
    internal const string HttpEndpointName = "http";

    // An EndpointReference is a core .NET Aspire type used for keeping
    // track of endpoint details in expressions. Simple literal values cannot
    // be used because endpoints are not known until containers are launched.
    private EndpointReference? _httpEndpointReference;

    public EndpointReference HttpEndpoint =>
        _httpEndpointReference ??= new(this, HttpEndpointName);

    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create(
            $"{HttpEndpointName}://{HttpEndpoint.Property(EndpointProperty.Host)}:{HttpEndpoint.Property(EndpointProperty.Port)}"
        );
}
