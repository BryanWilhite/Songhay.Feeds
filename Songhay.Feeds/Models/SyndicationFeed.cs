using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Songhay.Feeds.Models
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
                this.Title = new SyndicationContent(title);
            }
            if (description != null)
            {
                this.Description = new SyndicationContent(description);
            }
            if (feedAlternateLink != null)
            {
                this.Links.Add(new SyndicationLink(feedAlternateLink, "alternate"));
            }

            this.Id = id;
            this.LastUpdatedTime = lastUpdatedTime;
            this.Items = items;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public SyndicationContent Description { get => _description; set => _description = value; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get => id; set => id = value; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IEnumerable<SyndicationItem> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Gets or sets the last updated time.
        /// </summary>
        /// <value>
        /// The last updated time.
        /// </value>
        public DateTimeOffset LastUpdatedTime { get => lastUpdatedTime; set => lastUpdatedTime = value; }

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

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public SyndicationContent Title { get => _title; set => _title = value; }

        SyndicationContent _description;

        IEnumerable<SyndicationItem> _items;

        Collection<SyndicationLink> _links;

        SyndicationContent _title;

        string id;

        DateTimeOffset lastUpdatedTime;
    }
}
