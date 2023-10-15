namespace SitecoreSuggest.Extensions
{
    using Sitecore.Data.Fields;
    using System.Linq;

    /// <summary>
    /// Extensions for Sitecore fields
    /// </summary>
    public static class FieldExtensions
    {
        /// <summary>
        /// Determines whether usable for summaries.
        /// </summary>
        public static bool IsSummaryField(this Field field)
        {
            if (!Constants.SummaryFields.Contains(field.TypeKey))
                return false;

            return !string.IsNullOrEmpty(field?.GetValue(true));
        }

        /// <summary>
        /// Determines whether a field is supported.
        /// </summary>
        public static bool IsSupported(this Field field)
        {
            if (field == null)
                return false;

            if (field.Name.StartsWith("__"))
                return false;

            return Constants.SupportedTypeKeys.Contains(field.TypeKey);
        }

        /// <summary>
        /// Determines whether a field is a HTML field.
        /// </summary>
        public static bool IsHtml(this Field field)
        {
            return Constants.HtmlTypeKeys.Contains(field.TypeKey);
        }

        /// <summary>
        /// Generates a short value from a field
        /// </summary>
        public static string ShortValue(this Field field)
        {
            var value = field?.GetValue(true).Trim();

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length < Constants.ShortValueLength)
                return value;

            return value.Substring(0, Constants.ShortValueLength) + " ...";                      
        }
    }
}