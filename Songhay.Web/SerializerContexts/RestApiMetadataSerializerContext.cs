using System.Text.Json.Serialization;
using Songhay.Models;

namespace Songhay.Web.SerializerContexts;


[JsonSerializable(typeof(RestApiMetadata))]
public partial class RestApiMetadataSerializerContext : JsonSerializerContext;
