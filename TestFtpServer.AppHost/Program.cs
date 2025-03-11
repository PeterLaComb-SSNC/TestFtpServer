var builder = DistributedApplication.CreateBuilder(args);

var sftpAdminUser = builder.AddParameter("sftpAdminUser");
var sftpAdminPassword = builder.AddParameter("sftpAdminPassword", true);
var scenarioFilePath = builder.AddParameter("userRepo");

builder
    .AddSftpServer(
        adminUser: sftpAdminUser,
        adminPassword: sftpAdminPassword,
        sftpPort: 2022,
        httpPort: 4040
    )
    .WithUserRepository(
        scenarioFilePath: scenarioFilePath
    )
    ;

builder.Build().Run();
