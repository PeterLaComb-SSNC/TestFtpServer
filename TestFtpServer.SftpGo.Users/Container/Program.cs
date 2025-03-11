using Microsoft.AspNetCore.Mvc;

using TestFtpServer.SftpGo.Users.Container;
using TestFtpServer.SftpGo.Users.Container.Models;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace TestFtpServer.SftpGo.Users;
#pragma warning restore IDE0130 // Namespace does not match folder structure

#pragma warning disable CS8892 // Method will not be used as an entry point because a synchronous entry point 'method' was found.
#if BUILDING_CONTAINER
internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        var app = builder.Build();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        var scenario = builder.Configuration.GetValue("SFTPGO_USERS_LIST", "");
        if (string.IsNullOrEmpty(scenario) is false)
        {
            logger.LogInformation("Loading {scenarioFile}", scenario);
            await TestScenario.Load(logger, scenario);
        } else 
        {
            logger.LogInformation("Using default users");
        }
        app.MapPost(
            "/",
            (
                [FromQuery] string? login_method,
                [FromQuery] string? protocol,
                [FromBody] User requestedUser,
                ILogger<User> logger
            ) =>
            {
                var result = TestScenario.GetUser(requestedUser.Username);
                logger.LogInformation(
                    "Login received: login_method='{login_method}' protocol='{protocol}' requestedUser:{@requestedUser} result:{@result}",
                    login_method,
                    protocol,
                    requestedUser,
                    result
                );
                return result;
            }
        );

        app.Run();
    }
}
#endif
#pragma warning disable CS8892 // Method will not be used as an entry point because a synchronous entry point 'method' was found.
