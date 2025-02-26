using System.Runtime.Serialization;
using System.Collections.Frozen;
using System.Reflection;

namespace TestFtpServer.SftpGo.Users.Container.Models;

public class EnumMemberConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private static readonly FrozenDictionary<TEnum, string> _toString;
    private static readonly FrozenDictionary<string, TEnum> _fromString;

    static EnumMemberConverter()
    {
        var toString = new Dictionary<TEnum, string>();
        var fromString = new Dictionary<string, TEnum>();
        var validValues = Enum.GetNames<TEnum>();
        var type = typeof(TEnum);
        var members = type.GetMembers();
        foreach (MemberInfo mi in members)
        {
            if (validValues.Contains(mi.Name))
            {
                var attr = mi.GetCustomAttribute<EnumMemberAttribute>();
                var stringValue = attr?.Value ?? mi.Name;
                var enumMember = Enum.Parse<TEnum>(mi.Name);
                toString[enumMember] = stringValue;
                fromString[stringValue] = enumMember;
            }
        }
        _toString = toString.ToFrozenDictionary();
        _fromString = fromString.ToFrozenDictionary();
    }

    public override TEnum Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        string? stringValue = reader.GetString();
        return string.IsNullOrWhiteSpace(stringValue)
            ? default
            : _fromString.TryGetValue(stringValue, out var @enum)
                ? @enum
                : default;
    }

    public override void Write(
        Utf8JsonWriter writer,
        TEnum value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(_toString[value]);
    }
}
