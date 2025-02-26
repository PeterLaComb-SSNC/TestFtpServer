using System.Runtime.Serialization;

namespace TestFtpServer.SftpGo.Users.Container.Models;

[JsonConverter(typeof(EnumMemberConverter<Permission>))]
public enum Permission
{
    /// <summary>
    /// Enum Star for *
    /// </summary>
    [EnumMember(Value = "*")]
    Star = 0,

    /// <summary>
    /// Enum ListEnum for list
    /// </summary>
    [EnumMember(Value = "list")]
    ListEnum = 1,

    /// <summary>
    /// Enum DownloadEnum for download
    /// </summary>
    [EnumMember(Value = "download")]
    DownloadEnum = 2,

    /// <summary>
    /// Enum UploadEnum for upload
    /// </summary>
    [EnumMember(Value = "upload")]
    UploadEnum = 3,

    /// <summary>
    /// Enum OverwriteEnum for overwrite
    /// </summary>
    [EnumMember(Value = "overwrite")]
    OverwriteEnum = 4,

    /// <summary>
    /// Enum DeleteEnum for delete
    /// </summary>
    [EnumMember(Value = "delete")]
    DeleteEnum = 5,

    /// <summary>
    /// Enum DeleteFilesEnum for delete_files
    /// </summary>
    [EnumMember(Value = "delete_files")]
    DeleteFilesEnum = 6,

    /// <summary>
    /// Enum DeleteDirsEnum for delete_dirs
    /// </summary>
    [EnumMember(Value = "delete_dirs")]
    DeleteDirsEnum = 7,

    /// <summary>
    /// Enum RenameEnum for rename
    /// </summary>
    [EnumMember(Value = "rename")]
    RenameEnum = 8,

    /// <summary>
    /// Enum RenameFilesEnum for rename_files
    /// </summary>
    [EnumMember(Value = "rename_files")]
    RenameFilesEnum = 9,

    /// <summary>
    /// Enum RenameDirsEnum for rename_dirs
    /// </summary>
    [EnumMember(Value = "rename_dirs")]
    RenameDirsEnum = 10,

    /// <summary>
    /// Enum CreateDirsEnum for create_dirs
    /// </summary>
    [EnumMember(Value = "create_dirs")]
    CreateDirsEnum = 11,

    /// <summary>
    /// Enum CreateSymlinksEnum for create_symlinks
    /// </summary>
    [EnumMember(Value = "create_symlinks")]
    CreateSymlinksEnum = 12,

    /// <summary>
    /// Enum ChmodEnum for chmod
    /// </summary>
    [EnumMember(Value = "chmod")]
    ChmodEnum = 13,

    /// <summary>
    /// Enum ChownEnum for chown
    /// </summary>
    [EnumMember(Value = "chown")]
    ChownEnum = 14,

    /// <summary>
    /// Enum ChtimesEnum for chtimes
    /// </summary>
    [EnumMember(Value = "chtimes")]
    ChtimesEnum = 15,

    /// <summary>
    /// Enum CopyEnum for copy
    /// </summary>
    [EnumMember(Value = "copy")]
    CopyEnum = 16
}
