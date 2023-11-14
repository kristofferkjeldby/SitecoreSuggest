namespace SitecoreSuggest.Extensions
{
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using SitecoreSuggest.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Version = Sitecore.Data.Version;

    /// <summary>
    /// Extensions for payloads
    /// </summary>
    public static class SuggestFormPayloadExtensions
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        public static Item GetItem(this SuggestFormPayload payload)
        {
            var database = Database.GetDatabase(payload.Database);
            var langauge = payload.GetLanguage();
            var itemId = ID.Parse(payload.ItemId);
            var version = Version.Parse(payload.Version);   

            return database.GetItem(itemId, langauge, version);
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        public static Sitecore.Globalization.Language GetLanguage(this SuggestFormPayload payload)
        {
            return Sitecore.Globalization.Language.Parse(payload.Language);
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        public static List<Field> GetFields(this SuggestFormPayload payload)
        {
            var item = payload.GetItem();

            var fields = new List<Field>();

            item?.Fields.ReadAll();

            foreach (var f in item?.Fields)
            {
                var field = f as Field;

                if (field.IsInsertField())
                    fields.Add(field);
            }

            return fields.OrderBy(f => f.SectionSortorder).ThenBy(f => f.SectionDisplayName).ThenBy(f => f.Sortorder).ToList();
        }

        /// <summary>
        /// Update the fields.
        /// </summary>
        public static void UpdateField(this SuggestFormPayload payload, bool append)
        {
            if (payload == null)
                return;

            if (string.IsNullOrEmpty(payload.Suggestion))
                return;

            if (string.IsNullOrEmpty(payload.FieldId))
                return;

            var item = payload.GetItem();

            if (item == null) 
                return;

            var field = item.Fields[ID.Parse(payload.FieldId)];

            if (field == null)
                return;

            var suggestion = payload.Suggestion;

            if (field.IsHtmlField())
                suggestion = suggestion.ToHtml();

            using (new EditContext(item))
            {
                if (append)
                {
                    var value = item.Fields[field.ID].GetValue(true).Trim();
                    if (!string.IsNullOrEmpty(value))
                        suggestion = string.Concat(value, System.Environment.NewLine, suggestion);
                }
                    
                item.Fields[field.ID].SetValue(suggestion, false);
            }
        }
    }
}