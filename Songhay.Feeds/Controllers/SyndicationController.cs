using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Songhay.Feeds.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Songhay.Feeds.Controllers
{
    [Route("api/syndication")]
    public class SyndicationController : Controller
    {
        public SyndicationController(IOptions<FeedsControllerMetadata> meta)
        {
            this._meta = meta.Value;
        }

        [Route("feed/{name}")]
        public async Task<IActionResult> GetFeed(string name)
        {
            if (string.IsNullOrEmpty(name)) return this.BadRequest("The expected feed is not here.");

            var feedLocation = this._meta.Feeds[name];

            if (string.IsNullOrEmpty(feedLocation)) return this.NotFound($"The expected feed, {name}, is not here.");

            var items = new List<SyndicationItem>();

            using (var xmlReader = XmlReader.Create(feedLocation, new XmlReaderSettings { Async = true }))
            {
                var feedReader = new RssFeedReader(xmlReader);

                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        case SyndicationElementType.Item:
                            ISyndicationItem item = await feedReader.ReadItem();
                            items.Add(new SyndicationItem(item));
                            break;
                        default:
                            break;
                    }
                }
            }

            var feed = new SyndicationFeed(items);
            return this.Ok(feed);
        }

        [Route("info/{feed}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult GetFeedInfo(string feed)
        {
            if (string.IsNullOrEmpty(feed)) return this.BadRequest("The expected feed is not here.");

            var feedLocation = this._meta.Feeds[feed];

            if (string.IsNullOrEmpty(feedLocation)) return this.NotFound($"The expected feed, {feed}, is not here.");
            return this.Ok(feedLocation);
        }

        FeedsControllerMetadata _meta;
    }
}