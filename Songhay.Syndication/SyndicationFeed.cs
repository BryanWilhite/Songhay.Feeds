using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Songhay.Syndication
{
    /// <summary>
    /// Represents a top-level feed object in Atom 1.0 and in RSS 2.0.
    /// </summary>
    public class SyndicationFeed
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.ServiceModel.Syndication.SyndicationFeed" /> class with the specified collection of <see cref="T:System.ServiceModel.Syndication.SyndicationItem" /> objects.</summary>
        /// <param name="items">A collection of <see cref="T:System.ServiceModel.Syndication.SyndicationItem" /> objects.</param>
        public SyndicationFeed(IEnumerable<SyndicationItem> items) : this(null, null, null, items)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.ServiceModel.Syndication.SyndicationFeed" /> class with the specified title, description, URI, and collection of <see cref="T:System.ServiceModel.Syndication.SyndicationItem" /> objects.</summary>
        /// <param name="title">The title of the feed.</param>
        /// <param name="description">The description of the feed.</param>
        /// <param name="feedAlternateLink">The URI for the feed.</param>
        /// <param name="items">A collection of <see cref="T:System.ServiceModel.Syndication.SyndicationItem" /> objects.</param>
        public SyndicationFeed(string title, string description, Uri feedAlternateLink, IEnumerable<SyndicationItem> items) : this(title, description, feedAlternateLink, null, DateTimeOffset.MinValue, items)
        {
        }

        /// <summary>Creates a new instance of the <see cref="T:System.ServiceModel.Syndication.SyndicationFeed" /> class. </summary>
        /// <param name="title">The syndication feed title.</param>
        /// <param name="description">The syndication feed description.</param>
        /// <param name="feedAlternateLink">The alternate URI for the syndication feed.</param>
        /// <param name="id">The ID of the syndication feed.</param>
        /// <param name="lastUpdatedTime">The <see cref="T:System.DateTimeOffset" /> that contains the last time the syndication feed was updated.</param>
        /// <param name="items">A collection of <see cref="T:System.ServiceModel.Syndication.SyndicationItem" /> objects.</param>
        public SyndicationFeed(string title, string description, Uri feedAlternateLink, string id, DateTimeOffset lastUpdatedTime, IEnumerable<SyndicationItem> items)
        {
            if (title != null)
            {
                this._title = new SyndicationContent(title);
            }
            if (description != null)
            {
                this._description = new SyndicationContent(description);
            }
            if (feedAlternateLink != null)
            {
                this.Links.Add(new SyndicationLink(feedAlternateLink, "alternate"));
            }

            this.id = id;
            this.lastUpdatedTime = lastUpdatedTime;
            this._items = items;
        }

        /// <summary>Gets the links associated with the feed.</summary>
        /// <returns>A collection of <see cref="T:System.ServiceModel.Syndication.SyndicationLink" /> objects.</returns>
        public Collection<SyndicationLink> Links
        {
            get
            {
                if (this._links == null)
                {
                    this._links = new NullNotAllowedCollection<SyndicationLink>();
                }
                return this._links;
            }
        }

        string id;
        DateTimeOffset lastUpdatedTime;

        Collection<SyndicationLink> _links;
        IEnumerable<SyndicationItem> _items;

        SyndicationContent _description;
        SyndicationContent _title;
    }
}
