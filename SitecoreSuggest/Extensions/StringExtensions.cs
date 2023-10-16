namespace SitecoreSuggest.Extensions
{
    using System;
    using System.Globalization;

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
            var replacement = $"</p>{Environment.NewLine}</p>";

            return string.Concat(
                "<p>",
                value.Replace("\n\n", replacement).Replace("\r\n\r\n", replacement),
                "</p>"
            );
        }

        /// <summary>
        /// Parses to int.
        /// </summary>
        public static int ParseInt(this string text, int defaultValue)
        {
            if (int.TryParse(text, out var value))
                return value;

            return defaultValue;
        }

        /// <summary>
        /// Parses to float.
        /// </summary>
        public static float ParseFloat(this string text, float defaultValue)
        {
            if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                return value;

            return defaultValue;
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