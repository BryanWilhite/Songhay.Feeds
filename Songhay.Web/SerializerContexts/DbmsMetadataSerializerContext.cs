using System.Text.Json.Serialization;
using Songhay.Models;

namespace Songhay.Web.SerializerContexts;

[JsonSerializable(typeof(DbmsMetadata))]
public partial class DbmsMetadataSerializerContext : JsonSerializerContext;
