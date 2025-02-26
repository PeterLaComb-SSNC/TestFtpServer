using System.ComponentModel.DataAnnotations;

namespace TestFtpServer.SftpGo.Users.Container.Models;

public record class User
{
    /// <summary>
    /// Gets or Sets Id
    /// </summary>

    [JsonPropertyName("id")]
    public int? Id { get; set; } = default!;

    /// <summary>
    /// status:   * `0` user is disabled, login is not allowed   * `1` user is enabled 
    /// </summary>
    /// <value>status:   * `0` user is disabled, login is not allowed   * `1` user is enabled </value>
    public enum StatusEnum
    {
        Disabled = 0,
        Enabled = 1
    }

    /// <summary>
    /// status:   * &#x60;0&#x60; user is disabled, login is not allowed   * &#x60;1&#x60; user is enabled 
    /// </summary>
    /// <value>status:   * &#x60;0&#x60; user is disabled, login is not allowed   * &#x60;1&#x60; user is enabled </value>

    [JsonPropertyName("status")]
    public StatusEnum? Status { get; set; } = default!;

    /// <summary>
    /// username is unique
    /// </summary>
    /// <value>username is unique</value>

    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    /// <summary>
    /// Gets or Sets Email
    /// </summary>

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    /// <summary>
    /// optional description, for example the user full name
    /// </summary>
    /// <value>optional description, for example the user full name</value>

    [JsonPropertyName("description")]
    public string Description { get; set; } = default!;

    /// <summary>
    /// expiration date as unix timestamp in milliseconds. An expired account cannot login. 0 means no expiration
    /// </summary>
    /// <value>expiration date as unix timestamp in milliseconds. An expired account cannot login. 0 means no expiration</value>

    [JsonPropertyName("expiration_date")]
    public long? ExpirationDate { get; set; } = default!;

    /// <summary>
    /// If the password has no known hashing algo prefix it will be stored, by default, using bcrypt, argon2id is supported too. You can send a password hashed as bcrypt ($2a$ prefix), argon2id, pbkdf2 or unix crypt and it will be stored as is. For security reasons this field is omitted when you search/get users
    /// </summary>
    /// <value>If the password has no known hashing algo prefix it will be stored, by default, using bcrypt, argon2id is supported too. You can send a password hashed as bcrypt ($2a$ prefix), argon2id, pbkdf2 or unix crypt and it will be stored as is. For security reasons this field is omitted when you search/get users</value>

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    /// <summary>
    /// Public keys in OpenSSH format.
    /// </summary>
    /// <value>Public keys in OpenSSH format.</value>

    [JsonPropertyName("public_keys")]
    public List<string>? PublicKeys { get; set; }

    /// <summary>
    /// Indicates whether the password is set
    /// </summary>
    /// <value>Indicates whether the password is set</value>

    [JsonPropertyName("has_password")]
    public bool HasPassword => string.IsNullOrWhiteSpace(Password) is false;

    /// <summary>
    /// path to the user home directory. The user cannot upload or download files outside this directory. SFTPGo tries to automatically create this folder if missing. Must be an absolute path
    /// </summary>
    /// <value>path to the user home directory. The user cannot upload or download files outside this directory. SFTPGo tries to automatically create this folder if missing. Must be an absolute path</value>

    [JsonPropertyName("home_dir")]
    public string HomeDir { get; set; } = default!;

    /// <summary>
    /// if you run SFTPGo as root user, the created files and directories will be assigned to this uid. 0 means no change, the owner will be the user that runs SFTPGo. Ignored on windows
    /// </summary>
    /// <value>if you run SFTPGo as root user, the created files and directories will be assigned to this uid. 0 means no change, the owner will be the user that runs SFTPGo. Ignored on windows</value>

    [Range(0, 2147483647)]
    [JsonPropertyName("uid")]
    public int? Uid { get; set; } = default!;

    /// <summary>
    /// if you run SFTPGo as root user, the created files and directories will be assigned to this gid. 0 means no change, the group will be the one of the user that runs SFTPGo. Ignored on windows
    /// </summary>
    /// <value>if you run SFTPGo as root user, the created files and directories will be assigned to this gid. 0 means no change, the group will be the one of the user that runs SFTPGo. Ignored on windows</value>

    [Range(0, 2147483647)]
    [JsonPropertyName("gid")]
    public int? Gid { get; set; } = default!;

    /// <summary>
    /// Limit the sessions that a user can open. 0 means unlimited
    /// </summary>
    /// <value>Limit the sessions that a user can open. 0 means unlimited</value>

    [JsonPropertyName("max_sessions")]
    public int? MaxSessions { get; set; } = default!;

    /// <summary>
    /// Quota as size in bytes. 0 means unlimited. Please note that quota is updated if files are added/removed via SFTPGo otherwise a quota scan or a manual quota update is needed
    /// </summary>
    /// <value>Quota as size in bytes. 0 means unlimited. Please note that quota is updated if files are added/removed via SFTPGo otherwise a quota scan or a manual quota update is needed</value>

    [JsonPropertyName("quota_size")]
    public long? QuotaSize { get; set; } = default!;

    /// <summary>
    /// Quota as number of files. 0 means unlimited. Please note that quota is updated if files are added/removed via SFTPGo otherwise a quota scan or a manual quota update is needed
    /// </summary>
    /// <value>Quota as number of files. 0 means unlimited. Please note that quota is updated if files are added/removed via SFTPGo otherwise a quota scan or a manual quota update is needed</value>

    [JsonPropertyName("quota_files")]
    public int? QuotaFiles { get; set; } = default!;

    /// <summary>
    /// hash map with directory as key and an array of permissions as value. Directories must be absolute paths, permissions for root directory (\&quot;/\&quot;) are required
    /// </summary>
    /// <value>hash map with directory as key and an array of permissions as value. Directories must be absolute paths, permissions for root directory (\&quot;/\&quot;) are required</value>

    [JsonPropertyName("permissions")]
    public Dictionary<string, List<Permission>> Permissions { get; set; } = default!;

    /// <summary>
    /// Gets or Sets UsedQuotaSize
    /// </summary>

    [JsonPropertyName("used_quota_size")]
    public long? UsedQuotaSize { get; set; } = default!;

    /// <summary>
    /// Gets or Sets UsedQuotaFiles
    /// </summary>

    [JsonPropertyName("used_quota_files")]
    public int? UsedQuotaFiles { get; set; } = default!;

    /// <summary>
    /// Last quota update as unix timestamp in milliseconds
    /// </summary>
    /// <value>Last quota update as unix timestamp in milliseconds</value>

    [JsonPropertyName("last_quota_update")]
    public long? LastQuotaUpdate { get; set; } = default!;

    /// <summary>
    /// Maximum upload bandwidth as KB/s, 0 means unlimited
    /// </summary>
    /// <value>Maximum upload bandwidth as KB/s, 0 means unlimited</value>

    [JsonPropertyName("upload_bandwidth")]
    public int? UploadBandwidth { get; set; } = default!;

    /// <summary>
    /// Maximum download bandwidth as KB/s, 0 means unlimited
    /// </summary>
    /// <value>Maximum download bandwidth as KB/s, 0 means unlimited</value>

    [JsonPropertyName("download_bandwidth")]
    public int? DownloadBandwidth { get; set; } = default!;

    /// <summary>
    /// Maximum data transfer allowed for uploads as MB. 0 means no limit
    /// </summary>
    /// <value>Maximum data transfer allowed for uploads as MB. 0 means no limit</value>

    [JsonPropertyName("upload_data_transfer")]
    public int? UploadDataTransfer { get; set; } = default!;

    /// <summary>
    /// Maximum data transfer allowed for downloads as MB. 0 means no limit
    /// </summary>
    /// <value>Maximum data transfer allowed for downloads as MB. 0 means no limit</value>

    [JsonPropertyName("download_data_transfer")]
    public int? DownloadDataTransfer { get; set; } = default!;

    /// <summary>
    /// Maximum total data transfer as MB. 0 means unlimited. You can set a total data transfer instead of the individual values for uploads and downloads
    /// </summary>
    /// <value>Maximum total data transfer as MB. 0 means unlimited. You can set a total data transfer instead of the individual values for uploads and downloads</value>

    [JsonPropertyName("total_data_transfer")]
    public int? TotalDataTransfer { get; set; } = default!;

    /// <summary>
    /// Uploaded size, as bytes, since the last reset
    /// </summary>
    /// <value>Uploaded size, as bytes, since the last reset</value>

    [JsonPropertyName("used_upload_data_transfer")]
    public int? UsedUploadDataTransfer { get; set; } = default!;

    /// <summary>
    /// Downloaded size, as bytes, since the last reset
    /// </summary>
    /// <value>Downloaded size, as bytes, since the last reset</value>

    [JsonPropertyName("used_download_data_transfer")]
    public int? UsedDownloadDataTransfer { get; set; } = default!;

    /// <summary>
    /// creation time as unix timestamp in milliseconds. It will be 0 for users created before v2.2.0
    /// </summary>
    /// <value>creation time as unix timestamp in milliseconds. It will be 0 for users created before v2.2.0</value>

    [JsonPropertyName("created_at")]
    public long? CreatedAt { get; set; } = default!;

    /// <summary>
    /// last update time as unix timestamp in milliseconds
    /// </summary>
    /// <value>last update time as unix timestamp in milliseconds</value>

    [JsonPropertyName("updated_at")]
    public long? UpdatedAt { get; set; } = default!;

    /// <summary>
    /// Last user login as unix timestamp in milliseconds. It is saved at most once every 10 minutes
    /// </summary>
    /// <value>Last user login as unix timestamp in milliseconds. It is saved at most once every 10 minutes</value>

    [JsonPropertyName("last_login")]
    public long? LastLogin { get; set; } = default!;

    /// <summary>
    /// first download time as unix timestamp in milliseconds
    /// </summary>
    /// <value>first download time as unix timestamp in milliseconds</value>

    [JsonPropertyName("first_download")]
    public long? FirstDownload { get; set; } = default!;

    /// <summary>
    /// first upload time as unix timestamp in milliseconds
    /// </summary>
    /// <value>first upload time as unix timestamp in milliseconds</value>

    [JsonPropertyName("first_upload")]
    public long? FirstUpload { get; set; } = default!;

    /// <summary>
    /// last password change time as unix timestamp in milliseconds
    /// </summary>
    /// <value>last password change time as unix timestamp in milliseconds</value>

    [JsonPropertyName("last_password_change")]
    public long? LastPasswordChange { get; set; } = default!;

    /// <summary>
    /// Free form text field for external systems
    /// </summary>
    /// <value>Free form text field for external systems</value>

    [JsonPropertyName("additional_info")]
    public string AdditionalInfo { get; set; } = default!;

    /// <summary>
    /// This field is passed to the pre-login hook if custom OIDC token fields have been configured. Field values can be of any type (this is a free form object) and depend on the type of the configured OIDC token fields
    /// </summary>
    /// <value>This field is passed to the pre-login hook if custom OIDC token fields have been configured. Field values can be of any type (this is a free form object) and depend on the type of the configured OIDC token fields</value>

    [JsonPropertyName("oidc_custom_fields")]
    public Dictionary<string, object> OidcCustomFields { get; set; } = default!;

    /// <summary>
    /// Gets or Sets Role
    /// </summary>

    [JsonPropertyName("role")]
    public string Role { get; set; } = default!;
}
