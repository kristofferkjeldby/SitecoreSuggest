namespace SitecoreSuggest.Extensions
{
    using Sitecore.Shell.Applications.ContentEditor;
    using SitecoreSuggest.Models;
    using System;
    using System.Globalization;
    using System.Linq;

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
        /// Estimates the number of tokens.
        /// </summary>
        public static int EstimateTokens(this string value, Language language)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            float tokens = value.Count(c => Constants.TokenChars.Contains(c));

            foreach(var word in value.Split(Constants.TokenSplitChars))
            {
                // Number are typically tokenized in pairs of three
                if (word.All(c => char.IsDigit(c)))
                {
                    tokens += word.Length / 3;
                    continue;
                }

                tokens += language.TokensPerWord;
            }

            return Convert.ToInt32(Math.Ceiling(tokens));
        }
    }
}