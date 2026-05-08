var builder = DistributedApplication.CreateBuilder(args);

var sftpAdminUser = builder.AddParameter("sftpAdminUser");
var sftpAdminPassword = builder.AddParameter("sftpAdminPassword", true);

builder
    .AddSftpServer(
        adminUser: sftpAdminUser,
        adminPassword: sftpAdminPassword,
        sftpPort: 2022,
        httpPort: 4040
    )
    .WithUserRepository()
    ;

builder.Build().Run();
