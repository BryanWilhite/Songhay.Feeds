using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Songhay.Feeds.Controllers
{
    [Route("api/syndication")]
    public class SyndicationController : Controller
    {
        public SyndicationController(IConfiguration configuration)
        {
            var configurationRoot = configuration as IConfigurationRoot;
            if (configurationRoot == null) throw new NullReferenceException("The expected configuration root is not here.");

            this._feeds = configurationRoot.GetSection("feeds");
            if (this._feeds == null) throw new NullReferenceException("The expected configuration section is not here.");
        }

        [Route("info/{feed}")]
        public IActionResult GetFeed(string feed)
        {
            if (string.IsNullOrEmpty(feed)) return this.BadRequest("The expected feed is not here.");

            var feedLocation = this._feeds[feed];

            if (string.IsNullOrEmpty(feedLocation)) return this.NotFound($"The expected feed, {feed}, is not here.");
            return this.Ok(feedLocation);
        }

        IConfigurationSection _feeds;
    }
}