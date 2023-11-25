namespace SitecoreSuggest.Extensions
{
    using Microsoft.ML.Tokenizers;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Extensions for strings
    /// </summary>
    public static class StringExtensions
    {
        private static Tokenizer tokenizer = new Tokenizer(
            new Bpe(
                HttpContext.Current.Server.MapPath(Constants.VocabularyFile),
                HttpContext.Current.Server.MapPath(Constants.MergesFile)
            )
        );

        /// <summary>
        /// Converts to html.
        /// </summary>
        public static string ToHtml(this string value)
        {
            var replacement = $"</p>{Environment.NewLine}<p>";

            return string.Concat(
                "<p>",
                value.Replace("\n\n", replacement).Replace("\r\n\r\n", replacement),
                "</p>"
            );
        }

        /// <summary>
        /// Converts from html.
        /// </summary>
        public static string FromHtml(this string value)
        {
            return Sitecore.StringUtil.RemoveTags(value);
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

        /// <summary>
        /// Counts the number of tokens.
        /// </summary>
        public static int CountTokens(this string value)
        {
            return tokenizer.Encode(value).Tokens.Count();
        }
    }
}