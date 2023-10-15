namespace SitecoreSuggest.Extensions
{
    using System;

    /// <summary>
    /// Extensions for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts to html.
        /// </summary>
        public static string ToHtml(this string value)
        {
            return $"<p>{value.Replace(Environment.NewLine, "<br>")}</p>";
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        public static int ParseInt(this string value, int defaultValue)
        {
            if (int.TryParse(value, out var length))
                return length;

            return Constants.DefaultWords;
        }

        /// <summary>
        /// Append text.
        /// </summary>
        public static string Append(this string value, string text)
        {
            return string.Concat(value, Environment.NewLine, Environment.NewLine, text);
        }
    }
}