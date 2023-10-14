namespace SitecoreSuggest.Extensions
{
    using Sitecore.Data.Items;

    public static class ItemExtensions
    {
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
    }
}