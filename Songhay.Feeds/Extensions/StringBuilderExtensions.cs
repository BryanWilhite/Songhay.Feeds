using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
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

            var formatter = new AtomFormatter();
        }

        public static void BuildRss(this StringBuilder builder, SyndicationFeed feed)
        {
            if (builder == null) return;
            if (feed == null) return;

            var formatter = new RssFormatter();
        }
    }
}
