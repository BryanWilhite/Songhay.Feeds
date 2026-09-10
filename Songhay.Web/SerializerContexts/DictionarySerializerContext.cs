using System.Text.Json.Serialization;

namespace Songhay.Web.SerializerContexts;

[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class DictionarySerializerContext : JsonSerializerContext;
