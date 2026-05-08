using System.Collections.Concurrent;
using System.Collections.Frozen;

using TestFtpServer.SftpGo.Users.Models;

namespace TestFtpServer.SftpGo.Users.Container;

internal static class TestScenario
{
    private static readonly FrozenDictionary<string, User> _default;
    private static readonly ConcurrentDictionary<string, User> _loaded = new();

    static TestScenario()
    {
        var setup = new Dictionary<string, User>()
        {
            ["simplePassword"] = SetupUser(password: "1234Password"),
            ["disabled"] = SetupUser(enabled: false, password: "1234Password"),
            ["wrongPassword"] = SetupUser(enabled: false, password: Guid.NewGuid().ToString()),
            ["keyOnly"] = SetupUser(enabled: true, password: null, publicKeys: ["ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIMZtYfj/7iUnf++hfSSiSPXB/WMtdMZZaXAzT7hd054C test@test.com"]),
            ["keyAndPassword"] = SetupUser(enabled: true, password: "4321Password", publicKeys: ["ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIMZtYfj/7iUnf++hfSSiSPXB/WMtdMZZaXAzT7hd054C test@test.com"]),
        };
        _default = setup.ToFrozenDictionary();
    }

    public static User GetUser(string username)
    {
        var result = _default.TryGetValue(username, out var user)
            ? user
            : _loaded.TryGetValue(username, out user)
                ? user
                : new User();

        result.Username = username;
        return result;
    }

    public static string CreateUser(User requestedUser)
    {
        static string GenerateUserName(string requestedName) => $"{requestedName}-{Guid.NewGuid().ToString().Replace("-", "")[0..8]}";

        var user = SetupUser(
            requestedUser.Status == User.StatusEnum.Enabled,
            requestedUser.Password,
            requestedUser.PublicKeys?.ToArray(),
            requestedUser.Permissions
        );

        var userName = GenerateUserName(requestedUser.Username);
        while (_loaded.TryAdd(userName, user) is false)
        {
            userName = GenerateUserName(requestedUser.Username);
        }
        return userName;
    }

    private static User SetupUser(
        bool enabled = true,
        string? password = null,
        string[]? publicKeys = null,
        Dictionary<string, List<Permission>>? permissions = null
    ) =>
        new()
        {
            Password = password,
            PublicKeys = publicKeys?.ToList(),
            Status = enabled ? User.StatusEnum.Enabled : User.StatusEnum.Disabled,
            Permissions = permissions ?? new() { ["/"] = [Permission.Star] }
        };
}
