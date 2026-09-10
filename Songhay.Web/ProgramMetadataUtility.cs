using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;

using Songhay.Extensions;
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
        string? json = GetJsonForProgramMetadataFromEnvironment();

        if (string.IsNullOrWhiteSpace(json)) return null;

        return JsonSerializer
            .Deserialize<ProgramMetadata>(json, GetJsonDeserializerOptions());
    }

    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// for <see cref="ProgramMetadata"/>
    /// with explicit serializer context(s)
    /// for AOT compiled apps.
    /// </summary>
    public static JsonSerializerOptions GetJsonDeserializerOptions()
    {
        JsonSerializerOptions options = new();

        options.TypeInfoResolverChain.Add(ProgramMetadataSerializerContext.Default);
        options.TypeInfoResolverChain.Add(DbmsMetadataSerializerContext.Default);
        options.TypeInfoResolverChain.Add(ProgramMetadataSerializerContext.Default);

        return options;
    }

    /// <summary>
    /// Returns a JSON string or <c>null</c>
    /// based on the presence of one of two conventional environment variables.
    /// </summary>
    /// <remarks>
    /// Of the two expected environment variables,
    /// The one with the <c>_PATH</c> suffix should lead to JSON of the form:
    ///
    /// <code>
    /// {
    ///     "ProgramMetadata": { … }
    /// }
    /// </code>
    ///
    /// …which conforms to <see cref="IConfiguration"/> conventions.
    /// </remarks>
    public static string? GetJsonForProgramMetadataFromEnvironment()
    {
        string? json = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS");
        string? path = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS_PATH");

        if (string.IsNullOrWhiteSpace(json) && string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(json))
        {
            return json;
        }

        if (!string.IsNullOrWhiteSpace(path))
        {
            json = File.ReadAllText(path);
        }

        JsonElement jE = JsonElementUtility.ParseJson(json, logger: NullLogger.Instance);

        if (jE.IsExpectedObject(NullLogger.Instance, nameof(ProgramMetadata)))
        {
            JsonElement? actual = jE
                .GetJsonChildElementOrNull(nameof(ProgramMetadata));

            if (actual == null) return json;

            return actual.Value.GetRawText();
        }

        return json;
    }
}
