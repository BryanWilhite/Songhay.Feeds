using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Songhay.Feeds.Extensions;
using Songhay.Feeds.Models;
using Songhay.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Songhay.Feeds.OutputFormatters
{
    public class AtomOutputFormatter : TextOutputFormatter
    {
        public AtomOutputFormatter()
        {
            this.SupportedMediaTypes.Add(MimeTypes.ApplicationAtomXml);
            this.SupportedMediaTypes.Add(MimeTypes.ApplicationRssXml);

            this.SupportedEncodings.Add(Encoding.UTF8);
            this.SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var builder = new StringBuilder();
            var feed = context.Object as SyndicationFeed;
            builder.BuildAtom(feed);
            return context.HttpContext.Response.WriteAsync(builder.ToString());
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(SyndicationFeed).IsAssignableFrom(type)) return base.CanWriteType(type);

            return false;
        }
    }
}
