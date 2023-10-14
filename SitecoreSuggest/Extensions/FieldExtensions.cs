namespace SitecoreSuggest.Extensions
{
    using Sitecore.Data.Fields;
    using System.Linq;

    public static class FieldExtensions
    {
        public static bool IsSummaryField(this Field field)
        {
            if (!Constants.SummaryFields.Contains(field.TypeKey))
                return false;

            return !string.IsNullOrEmpty(field?.GetValue(true));
        }

        public static bool IsSupported(this Field field)
        {
            if (field == null)
                return false;

            if (field.Name.StartsWith("__"))
                return false;

            return Constants.SupportedTypeKeys.Contains(field.TypeKey);
        }

        public static bool IsHtml(this Field field)
        {
            return Constants.HtmlTypeKeys.Contains(field.TypeKey);
        }

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