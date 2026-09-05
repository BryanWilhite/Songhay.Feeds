using System.Text.Json;

using Songhay.Models;
using Songhay.Web.SerializerContexts;

namespace Songhay.Web;

/// <summary>
/// Shared routines for <see cref="ProgramMetadata"/>
/// </summary>
public static class ProgramMetadataUtility
{
    /// <summary>
    /// Returns an instance of <see cref="ProgramMetadata"/>
    /// based on the presence of one of two conventional environment variables.
    /// </summary>
    public static ProgramMetadata? GetProgramMetadataFromEnvironment()
    {
        string? json = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS");

        if (!string.IsNullOrWhiteSpace(json))
        {
            return JsonSerializer
                .Deserialize<ProgramMetadata>(json, GetJsonDeserializerOptions());
        }

        string? path = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS_PATH");

        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        json = File.ReadAllText(path);

        return JsonSerializer
            .Deserialize<ProgramMetadata>(json, GetJsonDeserializerOptions());
    }

    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// for <see cref="ProgramMetadata"/>
    /// with explicit serializer context(s)
    /// for AOT compiled apps.
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerOptions GetJsonDeserializerOptions()
    {
        JsonSerializerOptions options = new();

        options.TypeInfoResolverChain.Add(ProgramMetadataSerializerContext.Default);

        return options;
    }
}
