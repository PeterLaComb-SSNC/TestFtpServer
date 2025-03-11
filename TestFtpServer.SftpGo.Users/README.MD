# TestFtpServer.SftpGo.Users - a .NET Aspire component that adds users for SFTPGo Server

This is a companion package for TestFtpServer.SftpGo.Server that
uses the `SFTPGO_DATA_PROVIDER__PRE_LOGIN_HOOK` configuration option
to automatically create users. A default set of users is included,
and additional users can be created by referencing a simple JSON file.

## Usage

***This is intended for development purposes only***
Please do not run this directly in a public-facing location - it is not secure by default.

No configuration is required to get started. The UI link from the Aspire
dashboard will allow you to create an administrator login and you can go from there.

### Default Users

Each entry in this list is the *case-sensitive* user name, with details.

- simplePassword `1234Password`
- disabled `1234Password`
  - This user cannot login, but the password is correct
- wrongPassword
  - Every password you try for this user is wrong
- keyOnly

  This user can only auth with the following SSH key:

  ```text
  public_key:
      "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIMZtYfj/7iUnf++hfSSiSPXB/WMtdMZZaXAzT7hd054C test@test.com"
  
  private_key:
      -----BEGIN OPENSSH PRIVATE KEY-----
      b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAMwAAAAtzc2gtZW
      QyNTUxOQAAACDGbWH4/+4lJ3/voX0kokj1wf1jLXTGWWlwM0+4XdOeAgAAAJDXz9qW18/a
      lgAAAAtzc2gtZWQyNTUxOQAAACDGbWH4/+4lJ3/voX0kokj1wf1jLXTGWWlwM0+4XdOeAg
      AAAEBwYmdx4iCMpV3C0E7GOGH5F52YBCj03iRr8aiVC1a6d8ZtYfj/7iUnf++hfSSiSPXB
      /WMtdMZZaXAzT7hd054CAAAADXRlc3RAdGVzdC5jb20=
      -----END OPENSSH PRIVATE KEY-----
  ```

- keyAndPassword `4321Password`

  This user requires the password and this ssh key:

  ```text
  public_key:
      "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIMZtYfj/7iUnf++hfSSiSPXB/WMtdMZZaXAzT7hd054C test@test.com"
  
  private_key:
      -----BEGIN OPENSSH PRIVATE KEY-----
      b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAMwAAAAtzc2gtZW
      QyNTUxOQAAACDGbWH4/+4lJ3/voX0kokj1wf1jLXTGWWlwM0+4XdOeAgAAAJDXz9qW18/a
      lgAAAAtzc2gtZWQyNTUxOQAAACDGbWH4/+4lJ3/voX0kokj1wf1jLXTGWWlwM0+4XdOeAg
      AAAEBwYmdx4iCMpV3C0E7GOGH5F52YBCj03iRr8aiVC1a6d8ZtYfj/7iUnf++hfSSiSPXB
      /WMtdMZZaXAzT7hd054CAAAADXRlc3RAdGVzdC5jb20=
      -----END OPENSSH PRIVATE KEY-----
  ```

### Custom Users

1. Create a JSON file (Note: All properties are case-sensitive)

    ```json
    {
        /*
        "ScenarioName aka UserName": {
            "status": (1 = Enabled, 0 = Disabled),
            "password": ("whatever" or null),
            "permissions": {
                "path": [(PermissionOption)]
                // PermissionOption:
                // * - All
                // list - 
                // download - (Read)
                // upload - (Create/Write w/o overwrite)
                // overwrite - (specifically overwrite)
                // delete - (delete any)
                // delete_files - (files only)
                // delete_dirs - (dirs only)
                // rename - (rename any)
                // rename_files - (files only)
                // rename_dirs - (dirs only)
                // create_dirs - (dirs only)
                // create_symlinks
                // chmod -
                // chown -
                // chtimes -
                // copy -
            },
            "public_keys": [
                //raw text of the public key(s) in the array.
            ]
        }
        */
        "myTest": { "status": "1", "password": "password", "permissions": { "/": ["*"]}}
    }
    ```

2. Pass a ParameterReference to the path of that file in your application

    ```csharp
    var builder = DistributedApplication.CreateBuilder(args);
    
    var sftpAdminUser = builder.AddParameter("sftpAdminUser");
    var sftpAdminPassword = builder.AddParameter("sftpAdminPassword", true);
    var scenarioFilePath = builder.AddParameter("userRepo");
    
    builder
        .AddSftpServer(
            adminUser: sftpAdminUser,
            adminPassword: sftpAdminPassword
        )
        .WithUserRepository(
            scenarioFilePath: scenarioFilePath
        )
        ;
    
    builder.Build().Run();
    
    ```

3. The supplied file will be copied to a temp directory and mounted.

## Release Notes

- v0.0.1:
  Initial Release of the SFTPGo Users component for .NET Aspire

## TODO

- Add API endpoint(s) for user CRUD
