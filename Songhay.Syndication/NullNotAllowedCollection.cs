using System;
using System.Collections.ObjectModel;

namespace Songhay.Syndication
{
    internal class NullNotAllowedCollection<TCollectionItem> : Collection<TCollectionItem> where TCollectionItem : class
    {
        protected override void InsertItem(int index, TCollectionItem item)
        {
            if (item == null)
            {
                throw new NullReferenceException("The expected Collection item is not here.");
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, TCollectionItem item)
        {
            if (item == null)
            {
                throw new NullReferenceException("The expected Collection item is not here.");
            }
            base.SetItem(index, item);
        }
    }
}
