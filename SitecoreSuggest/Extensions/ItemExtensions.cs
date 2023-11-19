namespace SitecoreSuggest.Extensions
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Layouts;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for Sitecore items
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// Determines whether an item is supported.
        /// </summary>
        public static bool IsSupported(this Item item)
        {
            if (item == null)
                return false;

            if (item.Locking.IsLocked())
                return false;

            if (item.Appearance.ReadOnly)
                return false;

            if (item.Versions.Count == 0)
                return false;

            if (!Languages.SupportedLanguages.ContainsKey(item.Language.ToString()))
                return false;

            return true;
        }

        /// <summary>
        /// Gets a large icon for an item
        /// </summary>
        public static string GetLargeIconUrl(this Item item)
        {
            return Sitecore.Resources.Images.GetThemedImageSource(item.Appearance.Icon.Replace("16x16", "32x32"));
        }

        /// <summary>
        /// Gets the rendering references.
        /// </summary>
        public static RenderingReference[] GetRenderingReferences(this Item item)
        {
            return item?.Visualization.GetRenderings(Sitecore.Context.Device, false) ?? new RenderingReference[0];
        }

        /// <summary>
        /// Gets the data source items.
        /// </summary>>
        public static List<Item> GetDataSourceItems(this Item item)
        {
            var dataSourceItems = new List<Item>();

            foreach (RenderingReference reference in item.GetRenderingReferences())
            {
                if (!ID.TryParse(reference.Settings.DataSource, out var dataSourceId))
                    continue;

                var dataSourceItem = item.Database.GetItem(dataSourceId);

                if (dataSourceItem != null && dataSourceItem.Versions.Count > 0)
                {
                    dataSourceItems.Add(dataSourceItem);
                }
            }

            return dataSourceItems;
        }
    }
}