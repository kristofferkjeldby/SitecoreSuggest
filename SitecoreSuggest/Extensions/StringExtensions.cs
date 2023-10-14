namespace SitecoreSuggest.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string ToHtml(this string value)
        {
            return $"<p>{value.Replace(Environment.NewLine, "<br>")}</p>";
        }

        public static int ParseLength(this string value)
        {
            if (int.TryParse(value, out var length))
            {
                if (length > Constants.MaxWords)
                    return Constants.MaxWords;
                else if (length < Constants.MinWords)
                    return Constants.MinWords;
                else
                    return length;
            }
            else
            {
                return Constants.DefaultWords;
            }
        }
    }
}