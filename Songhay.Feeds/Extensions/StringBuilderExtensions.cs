using Songhay.Feeds.Models;
using System.Text;

namespace Songhay.Feeds.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void BuildAtom(this StringBuilder builder, SyndicationFeed feed)
        {
            if (builder == null) return;
            if (feed == null) return;
        }

        public static void BuildRss(this StringBuilder builder, SyndicationFeed feed)
        {
            if (builder == null) return;
            if (feed == null) return;
        }
    }
}
