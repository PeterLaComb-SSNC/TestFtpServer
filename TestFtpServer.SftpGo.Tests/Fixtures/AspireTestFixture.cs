using Aspire.Hosting;
using Aspire.Hosting.Testing;

[assembly: AssemblyFixture(typeof(TestFtpServer.SftpGo.Tests.Fixtures.AspireTestFixture))]

namespace TestFtpServer.SftpGo.Tests.Fixtures;

public sealed class AspireTestFixture : IAsyncDisposable, IDisposable
{
    private readonly CancellationTokenSource _startupTokenSource =
#if DEBUG
        new();
#else
        new(TimeSpan.FromMinutes(15));
#endif

    public CancellationToken CancellationToken => _startupTokenSource.Token;

    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private bool _initialized;

    private bool _disposed;

    private DistributedApplication? _app;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _semaphore.Dispose();
        _startupTokenSource.Cancel();
        _startupTokenSource.Dispose();
        if (_app is IDisposable disposable)
        {
            _app.StopAsync().GetAwaiter().GetResult();
            disposable.Dispose();
        }
        _disposed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _semaphore.Dispose();
        await _startupTokenSource.CancelAsync();
        _startupTokenSource.Dispose();
        if (_app is IAsyncDisposable disposable)
        {
            await _app.StopAsync();
            await disposable.DisposeAsync();
        }
        _disposed = true;
    }

    public async ValueTask<DistributedApplication> GetApp()
    {
        if (_initialized)
        {
            return _app!;
        }

        await _semaphore.WaitAsync(CancellationToken);
        try
        {
            if (_initialized)
            {
                return _app!;
            }

            Environment.SetEnvironmentVariable("ASPIRE_ALLOW_UNSECURED_TRANSPORT", "true");

            var userList = Path.GetTempFileName();
            await File.WriteAllTextAsync(
                userList,
                """
                {
                    "myTest": { "status": "1", "password": "password", "permissions": { "/": ["*"]}}
                }
                """,
                CancellationToken
            );

            var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.TestFtpServer_AppHost>(
                args:
                    [
                        // "DcpPublisher:RandomizePorts=false",
                        $"SftpServer:UserListFile={userList}"
                    ],
                configureBuilder:
                    (appOptions, hostSettings) =>
                    {
                        // Configure the builder here if needed
                        appOptions.DisableDashboard = false;
                    },
                cancellationToken: CancellationToken
            );

            _app = await builder.BuildAsync(CancellationToken).WaitAsync(CancellationToken);
            await _app.StartAsync(CancellationToken);
            await _app.ResourceNotifications.WaitForResourceHealthyAsync("SftpServer", CancellationToken);
            _initialized = true;
            return _app;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
