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

    public static class SuggestFormPayloadExtensions
    {
        public static Item GetItem(this SuggestFormPayload payload)
        {
            var database = Database.GetDatabase(payload.Database);
            var langauge = payload.GetLanguage();
            var itemId = ID.Parse(payload.ItemId);
            var version = Version.Parse(payload.Version);   

            return database.GetItem(itemId, langauge, version);
        }

        public static Language GetLanguage(this SuggestFormPayload payload)
        {
            return Language.Parse(payload.Language);
        }

        public static string GetPromptFieldValue(this SuggestFormPayload payload)
        {
            if (string.IsNullOrEmpty(payload.PromptFieldId))
                return null;

            var item = payload.GetItem();

            var promptFieldId = ID.Parse(payload.PromptFieldId);

            var promptField = item?.Fields[promptFieldId];

            return promptField?.GetValue(true)?.Trim();
        }

        public static List<Field> GetFields(this SuggestFormPayload payload)
        {
            var item = payload.GetItem();

            var fields = new List<Field>();

            item?.Fields.ReadAll();

            foreach (var f in item?.Fields)
            {
                var field = f as Field;

                if (field.IsSupported())
                    fields.Add(field);
            }

            return fields.OrderBy(f => f.SectionSortorder).ThenBy(f => f.SectionDisplayName).ThenBy(f => f.Sortorder).ToList();
        }

        public static void UpdateField(this SuggestFormPayload payload)
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

            if (field.IsHtml())
                suggestion = suggestion.ToHtml();

            using (new EditContext(item))
            {
                item.Fields[field.ID].SetValue(suggestion, false);
            }
        }

        public static string GetPrompt(this SuggestFormPayload payload)
        {
            var prompt = payload.Prompt;

            if (string.IsNullOrEmpty(prompt))
                prompt = payload.GetPromptFieldValue();

            if (string.IsNullOrEmpty(prompt))
                return string.Empty;

            return string.Format(Constants.SummaryStrings[payload.Language], prompt);
        }
    }
}