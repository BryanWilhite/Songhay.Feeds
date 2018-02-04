using Songhay.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Songhay.Feeds.Tests")]

namespace Songhay.Feeds.Activities
{
    public class FeedsActivitiesGetter : ActivitiesGetter
    {
        public FeedsActivitiesGetter(string[] args) : base(args)
        {
            this.LoadActivities(new Dictionary<string, Lazy<IActivity>>
            {
                {
                    nameof(DownloadFeedsActivity),
                    new Lazy<IActivity>(() => new DownloadFeedsActivity())
                }
            });
        }
    }
}
