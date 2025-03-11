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
            ["keyOnly"] = SetupUser(enabled: true, password: null, publicKeys: ["ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIMZtYfj/7iUnf++hfSSiSPXB/WMtdMZZaXAzT7hd054C test@test.com"]),
            ["keyAndPassword"] = SetupUser(enabled: true, password: "4321Password", publicKeys: ["ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIMZtYfj/7iUnf++hfSSiSPXB/WMtdMZZaXAzT7hd054C test@test.com"]),
        };
        _default = setup.ToFrozenDictionary();
    }

    public static async Task Load(
        ILogger logger,
        string path,
        CancellationToken cancellationToken = default
    )
    {
        if (Path.Exists(path))
        {
            var dictionary = await JsonSerializer.DeserializeAsync<Dictionary<string, User>>(File.OpenRead(path), cancellationToken: cancellationToken);
            if (dictionary is { })
            {
                foreach (var (key, value) in dictionary)
                {
                    logger.LogInformation("{@userName} added with {@config}", key, value);
                }
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
