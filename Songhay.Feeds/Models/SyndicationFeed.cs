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
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeed"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public SyndicationFeed(IEnumerable<ISyndicationItem> items) : this(null, null, null, items)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeed"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="feedAlternateLink">The feed alternate link.</param>
        /// <param name="items">The items.</param>
        public SyndicationFeed(string title, string description, Uri feedAlternateLink, IEnumerable<ISyndicationItem> items) : this(title, description, feedAlternateLink, null, DateTimeOffset.MinValue, items)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationFeed"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="feedAlternateLink">The feed alternate link.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="lastUpdatedTime">The last updated time.</param>
        /// <param name="items">The items.</param>
        public SyndicationFeed(string title, string description, Uri feedAlternateLink, string id, DateTimeOffset lastUpdatedTime, IEnumerable<ISyndicationItem> items)
        {
            this._authors = new Collection<ISyndicationPerson>();
            this._links = new Collection<ISyndicationLink>();

            this.Categories = new Collection<ISyndicationCategory>();

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
        /// Gets the authors.
        /// </summary>
        /// <value>
        /// The authors.
        /// </value>
        public Collection<ISyndicationPerson> Authors
        {
            get
            {
                if (this._authors == null)
                {
                    this._authors = new NullNotAllowedCollection<ISyndicationPerson>();
                }
                return this._authors;
            }
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public Collection<ISyndicationCategory> Categories { get; private set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public ISyndicationContent Description { get => _description; set => _description = value; }

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
        public IEnumerable<ISyndicationItem> Items { get => _items; set => _items = value; }

        /// <summary>
        /// Gets or sets the last updated time.
        /// </summary>
        /// <value>
        /// The last updated time.
        /// </value>
        public DateTimeOffset LastUpdatedTime { get => lastUpdatedTime; set => lastUpdatedTime = value; }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public Collection<ISyndicationLink> Links
        {
            get
            {
                if (this._links == null)
                {
                    this._links = new NullNotAllowedCollection<ISyndicationLink>();
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
        public ISyndicationContent Title { get => _title; set => _title = value; }

        ISyndicationContent _description;

        IEnumerable<ISyndicationItem> _items;

        Collection<ISyndicationPerson> _authors;
        Collection<ISyndicationLink> _links;

        ISyndicationContent _title;

        string id;

        DateTimeOffset lastUpdatedTime;
    }
}
