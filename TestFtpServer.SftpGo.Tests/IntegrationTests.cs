using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Aspire.Hosting.Testing;

using Renci.SshNet;

using TestFtpServer.SftpGo.Tests.Fixtures;

namespace TestFtpServer.SftpGo.Tests;

public sealed class IntegrationTests(
    AspireTestFixture testFixture
)
{
    private readonly AspireTestFixture _testFixture = testFixture;

    private record class NewUserResponse(
        [property: JsonPropertyName("userName")] string UserName
    );

    [Fact]
    public async Task DefaultUser_SimplePassword_CanLogin()
    {
        var app = await _testFixture.GetApp();

        var sftpServerEndpoint = app.GetEndpoint("SftpServer", "sftp");

        var connectionInfo = new ConnectionInfo(
            sftpServerEndpoint.Host,
            sftpServerEndpoint.Port,
            "simplePassword",
            new PasswordAuthenticationMethod("simplePassword", "1234Password")
        );
        using var sftpClient = new SftpClient(connectionInfo);
        await sftpClient.ConnectAsync(_testFixture.CancellationToken);
        Assert.True(sftpClient.IsConnected);
    }

    [Fact]
    public async Task CreateUser_WithSimplePassword_CanLogin()
    {
        var app = await _testFixture.GetApp();
        using var httpClient = app.CreateHttpClient("TestFtpServer-SftpGo-Users", "http");

        var newUser = new
        {
            username = "newUser",
            password = Guid.NewGuid().ToString(),
            status = 1,
        };
        using var content = new StringContent(
            JsonSerializer.Serialize(newUser),
            Encoding.UTF8,
            "application/json"
        );
        using var createUserRequest = new HttpRequestMessage(HttpMethod.Post, "/create")
        {
            Content = content
        };
        using var createUserResponse = await httpClient.SendAsync(createUserRequest, _testFixture.CancellationToken);
        createUserResponse.EnsureSuccessStatusCode();
        var createUserResponseContent = await createUserResponse.Content.ReadAsStringAsync(_testFixture.CancellationToken);
        var createUserResult = JsonSerializer.Deserialize<NewUserResponse>(createUserResponseContent);
        Assert.NotNull(createUserResult);

        var sftpServerEndpoint = app.GetEndpoint("SftpServer", "sftp");

        var connectionInfo = new ConnectionInfo(
            sftpServerEndpoint.Host,
            sftpServerEndpoint.Port,
            createUserResult.UserName,
            new PasswordAuthenticationMethod(createUserResult.UserName, newUser.password)
        );
        using var sftpClient = new SftpClient(connectionInfo);
        await sftpClient.ConnectAsync(_testFixture.CancellationToken);
        Assert.True(sftpClient.IsConnected);
    }
}
