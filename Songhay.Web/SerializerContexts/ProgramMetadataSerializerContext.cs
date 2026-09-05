using System.Text.Json.Serialization;
using Songhay.Models;

namespace Songhay.Web.SerializerContexts;


[JsonSerializable(typeof(ProgramMetadata))]
[JsonSerializable(typeof(DbmsMetadata))]
[JsonSerializable(typeof(RestApiMetadata))]
public partial class ProgramMetadataSerializerContext : JsonSerializerContext;
