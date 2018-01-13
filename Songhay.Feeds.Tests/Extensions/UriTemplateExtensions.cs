using System;
using System.Linq;
using Tavis.UriTemplates;

namespace Songhay.Feeds.Tests.Extensions
{
    /// <summary>
    /// Extensions of <see cref="UriTemplate"/>
    /// </summary>
    public static class UriTemplateExtensions
    {
        public static string BindByPosition(this UriTemplate template, params string[] values)
        {
            if (template == null) throw new ArgumentNullException("template", "The expected URI template is not here.");

            var keys = template.GetParameterNames();
            for (int i = 0; i < keys.Count(); i++)
            {
                template.AddParameter(keys.ElementAt(i), values.ElementAtOrDefault(i));
            }

            return template.Resolve();
        }
    }
}
