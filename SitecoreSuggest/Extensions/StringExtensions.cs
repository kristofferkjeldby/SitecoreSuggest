namespace SitecoreSuggest.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string ToHtml(this string value)
        {
            return $"<p>{value.Replace(Environment.NewLine, "<br>")}</p>";
        }

        public static int ParseWords(this string value)
        {
            if (int.TryParse(value, out var length))
                return length;

            return Constants.DefaultWords;
        }
    }
}