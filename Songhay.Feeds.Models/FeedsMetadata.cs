using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Songhay.Feeds.Models
{
    public class FeedsMetadata
    {
        public Dictionary<string, string> Feeds { get; set; }

        public string FeedsDirectory { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if ((this.Feeds != null) && this.Feeds.Any()) sb.AppendLine($"{nameof(this.Feeds)}:");
            foreach (var feed in this.Feeds) sb.AppendLine($"    {feed.Key}: {feed.Value}");

            if (!string.IsNullOrEmpty(this.FeedsDirectory)) sb.AppendLine($"{nameof(this.FeedsDirectory)}: {this.FeedsDirectory}");

            return (sb.Length > 0) ? sb.ToString() : base.ToString();
        }
    }
}
