namespace SitecoreSuggest.Extensions
{
    using Sitecore.Data.Items;

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

            return true;
        }

        /// <summary>
        /// Gets a large icon for an item
        /// </summary>
        public static string GetLargeIconUrl(this Item item)
        {
            return Sitecore.Resources.Images.GetThemedImageSource(item.Appearance.Icon.Replace("16x16", "32x32"));
        }
    }
}