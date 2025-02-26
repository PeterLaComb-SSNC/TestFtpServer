using System.Collections.Frozen;

using TestFtpServer.SftpGo.Users.Container.Models;

namespace TestFtpServer.SftpGo.Users.Container;

public static class TestScenario
{
    private static readonly FrozenDictionary<string, User> _default;
    private static FrozenDictionary<string, User>? _loaded;

    static TestScenario()
    {
        var setup = new Dictionary<string, User>()
        {
            ["simplePassword"] = SetupUser(password: "1234Password"),
            ["disabled"] = SetupUser(enabled: false, password: "1234Password"),
            ["wrongPassword"] = SetupUser(enabled: false, password: Guid.NewGuid().ToString()),
        };
        _default = setup.ToFrozenDictionary();
    }

    public static async Task Load(
        string path,
        CancellationToken cancellationToken = default
    )
    {
        if (Path.Exists(path))
        {
            var dictionary = await JsonSerializer.DeserializeAsync<Dictionary<string, User>>(File.OpenRead(path), cancellationToken: cancellationToken);
            if (dictionary is { })
            {
                _loaded = dictionary.ToFrozenDictionary();
            }
        }
    }

    public static User GetUser(string username)
    {
        var result = (_loaded ?? _default).TryGetValue(username, out var user)
            ? user
            : new();

        result.Username = username;
        return result;
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
