namespace SitecoreSuggest.Extensions
{
    using Sitecore.Data.Fields;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extensions for Sitecore fields
    /// </summary>
    public static class FieldExtensions
    {
        /// <summary>
        /// Determines whether the field is usable for summaries.
        /// </summary>
        public static bool IsSummary(this Field field)
        {
            return field.MatchConditions(Constants.SummaryFields);
        }

        /// <summary>
        /// Determines whether the field is usable for context.
        /// </summary>
        public static bool IsContextField(this Field field)
        {
            return field.MatchConditions(Constants.ContextFields);
        }

        /// <summary>
        /// Determines whether the field is usable for inserting.
        /// </summary>
        public static bool IsInsertField(this Field field)
        {
            return field.MatchConditions(Constants.InsertFields, true);
        }

        /// <summary>
        /// Determines whether a field matches conditions.
        /// </summary>
        public static bool MatchConditions(this Field field, IEnumerable<string> typeKeys, bool allowEmpty = false, bool allowSystemFields = false)
        {
            if (field == null)
                return false;

            if (field.Name.StartsWith("__") && !allowSystemFields)
                return false;

            if (!typeKeys.Contains(field.TypeKey))
                return false;

            if (allowEmpty)
                return true;

            return !string.IsNullOrEmpty(field?.GetValue(true));
        }

        /// <summary>
        /// Determines whether a field is a HTML field.
        /// </summary>
        public static bool IsHtmlField(this Field field)
        {
            return Constants.HtmlFields.Contains(field.TypeKey);
        }

        /// <summary>
        /// Determines whether a field is a HTML field.
        /// </summary>
        public static string GetValueAsString(this Field field, bool allowStandardValue)
        {
            var value = field.GetValue(allowStandardValue);
            return field.IsHtmlField() ? value.FromHtml() : value;
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