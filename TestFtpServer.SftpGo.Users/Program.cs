using Microsoft.AspNetCore.Mvc;

using TestFtpServer.SftpGo.Users.Container;
using TestFtpServer.SftpGo.Users.Models;

namespace TestFtpServer.SftpGo.Users;

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        var app = builder.Build();
        app.MapPost(
            "/create",
            (
                [FromBody] User requestedUser,
                ILogger<Program> logger
            ) =>
            {
                var userName = TestScenario.CreateUser(requestedUser);
                logger.LogInformation(
                    "Created userName:'{userName}' for requestedUser:{@requestedUser} ",
                    userName,
                    requestedUser
                );
                return Results.Ok(new { userName });
            }
        );
        app.MapPost(
            "/",
            (
                [FromQuery] string? login_method,
                [FromQuery] string? protocol,
                [FromBody] User requestedUser,
                ILogger<Program> logger
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
                return Results.Ok(result);
            }
        );
        app.Run();
    }
}
