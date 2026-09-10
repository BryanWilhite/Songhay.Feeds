using System.Text.Json.Serialization;
using Songhay.Models;

namespace Songhay.Web.SerializerContexts;


[JsonSerializable(typeof(ProgramMetadata))]
[JsonSerializable(typeof(DbmsMetadata))]
[JsonSerializable(typeof(RestApiMetadata))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<string, DbmsMetadata>))]
[JsonSerializable(typeof(Dictionary<string, RestApiMetadata>))]
public partial class ProgramMetadataSerializerContext : JsonSerializerContext;
