# TestFtpServer

This project is a set of .NET Aspire resources for testing sFTP server connections. It makes use of [SFTPGo](https://sftpgo.com/) as the server.

## SFTPGo

- [Documentation](https://docs.sftpgo.com/latest/)
- [GitHub](https://github.com/drakkan/sftpgo)
- [DockerHub](https://hub.docker.com/r/drakkan/sftpgo)
- [GitHub Container Registry](https://github.com/drakkan/sftpgo/pkgs/container/sftpgo)

## Default Users

Each entry in this list is the _case-sensitive_ user name, with details.

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

## Building

Tasks in the [`.vscode`](.vscode/) folder are configured to work with [podman desktop](https://podman-desktop.io/). With that installed, you should be able to launch with the default task.
