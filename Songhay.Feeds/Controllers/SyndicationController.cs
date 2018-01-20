using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Songhay.Syndication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Songhay.Feeds.Controllers
{
    [Route("api/syndication")]
    public class SyndicationController : Controller
    {
        public SyndicationController(IConfiguration configuration)
        {
            var configurationRoot = configuration as IConfigurationRoot;
            if (configurationRoot == null) throw new NullReferenceException("The expected configuration root is not here.");

            this._feeds = configurationRoot.GetSection(configSectionKey);
            if (this._feeds?.Value == null) throw new NullReferenceException("The expected configuration section is not here.");
        }

        [Route("feed/{name}")]
        public async Task<IActionResult> GetFeed(string name)
        {
            if (string.IsNullOrEmpty(name)) return this.BadRequest("The expected feed is not here.");

            var feedLocation = this._feeds[name];

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

            var feedLocation = this._feeds[feed];

            if (string.IsNullOrEmpty(feedLocation)) return this.NotFound($"The expected feed, {feed}, is not here.");
            return this.Ok(feedLocation);
        }

        const string configSectionKey = "feeds";

        IConfigurationSection _feeds;
    }
}