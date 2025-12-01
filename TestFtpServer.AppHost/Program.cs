var builder = DistributedApplication.CreateBuilder(args);

var sftpAdminUser = builder.AddParameter("sftpAdminUser");
var sftpAdminPassword = builder.AddParameter("sftpAdminPassword", true);
var scenarioFilePath = builder.AddParameter("userRepo");

var sftpGoServer =
    builder
        .AddSftpServer(
            adminUser: sftpAdminUser,
            adminPassword: sftpAdminPassword,
            sftpPort: 2022,
            httpPort: 4040
        );

sftpGoServer = await sftpGoServer
    .WithUserRepository(
        scenarioFilePath: scenarioFilePath
    )
    ;

builder.Build().Run();
