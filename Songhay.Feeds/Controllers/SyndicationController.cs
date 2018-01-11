using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Songhay.Feeds.Controllers
{
    [Route("api/syndication")]
    public class SyndicationController : Controller
    {
        Dictionary<string, string> _feeds;
    }
}