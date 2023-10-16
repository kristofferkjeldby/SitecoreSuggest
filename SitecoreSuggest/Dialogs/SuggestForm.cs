namespace SitecoreSuggest.Dialogs
{
    using Newtonsoft.Json;
    using Sitecore;
    using Sitecore.Common;
    using Sitecore.Data;
    using Sitecore.Data.Events;
    using Sitecore.Data.Fields;
    using Sitecore.Diagnostics;
    using Sitecore.Web;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.Pages;
    using SitecoreSuggest.Extensions;
    using SitecoreSuggest.Models;
    using SitecoreSuggest.Service;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The suggest form shown in Sitecore
    /// </summary>
    public class SuggestForm : DialogForm
    {
        protected Image IconImage;
        protected Literal NameLiteral;
        protected Literal ModelLiteral;
        protected Combobox SummaryFieldIdCombobox;
        protected Edit PromptEdit;
        protected Combobox WordsCombobox;
        protected Combobox FieldIdCombobox;
        protected Memo SuggestionMemo;
        protected SuggestService SuggestService = new SuggestService();

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestForm"/> class.
        /// </summary>
        public SuggestForm() : base()
        {

        }

        /// <summary>
        /// Triggered on load.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, nameof(e));

            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;

            var payload = GetPayload();

            var item = payload.GetItem();
            this.IconImage.Src = item.GetLargeIconUrl();
            this.NameLiteral.Text = item.Name;
            this.ModelLiteral.Text = SuggestService.Model;

            BindFields(payload);
        }

        /// <summary>
        /// Generates suggestions.
        /// </summary>
        protected void Generate(bool append)
        {
            var words = WordsCombobox.SelectedItem.Value.ParseInt(SitecoreSuggest.Constants.DefaultWords);

            var prompt = PromptEdit.Value;

            // If no prompt is entered, generate one from the summary field
            if (string.IsNullOrEmpty(prompt))
            {
                var summaryFieldId = SummaryFieldIdCombobox.SelectedItem.Value;
                var payload = GetPayload();
                prompt = GenerateSummaryFieldPrompt(payload, summaryFieldId);
            }

            if (string.IsNullOrEmpty(prompt))
                return;

            var suggestion = SuggestService.GenerateSuggestion(prompt, words);

            if (append && !string.IsNullOrEmpty(SuggestionMemo.Value))
                SuggestionMemo.Value = SuggestionMemo.Value.Append(suggestion);
            else
                SuggestionMemo.Value = suggestion;
        }

        /// <summary>
        /// Resolves the summary field value into a prompt.
        /// </summary>
        private static string GenerateSummaryFieldPrompt(SuggestFormPayload payload, string summaryFieldId)
        {
            if (string.IsNullOrEmpty(summaryFieldId))
                return null;

            var item = payload.GetItem();

            if (ID.TryParse(summaryFieldId, out var id))
            {
                var summaryField = item?.Fields[id];
                var summaryFieldValue = summaryField?.GetValue(true)?.Trim();
                
                if (string.IsNullOrEmpty(summaryFieldValue))
                    return null;

                if (SitecoreSuggest.Constants.SummaryPrompts.TryGetValue(payload.Language, out var summaryQuery))
                    return string.Format(summaryQuery, summaryFieldValue);
            }

            return null;
        }

        /// <summary>
        /// Closes the form with the specified action.
        /// </summary>
        protected void Close(string action = null)
        {
            if (action == null) {
                Context.ClientPage.ClientResponse.CloseWindow();
                return;
            }

            var payload = GetPayload();
            payload.Action = action;
            payload.Suggestion = SuggestionMemo.Value;
            payload.FieldId = FieldIdCombobox.SelectedItem.Value;

            Context.ClientPage.ClientResponse.SetDialogValue(JsonConvert.SerializeObject(payload));
            Context.ClientPage.ClientResponse.CloseWindow();
        }

        /// <summary>
        /// Gets the payload from the url
        /// </summary>
        private static SuggestFormPayload GetPayload()
        {
            var payloadJson = WebUtil.GetQueryString("payload");
            var payload = JsonConvert.DeserializeObject<SuggestFormPayload>(payloadJson);
            return payload;
        }

        /// <summary>
        /// Binds the comboboxes.
        /// </summary>
        private void BindFields(SuggestFormPayload payload)
        {
            var fields = payload.GetFields();
            BindSummaryFields(SummaryFieldIdCombobox, fields.Where(field => field.IsSummaryField()));
            BindFields(FieldIdCombobox, fields);

        }

        /// <summary>
        /// Binds the summary fields to a combobox
        /// </summary>
        private static void BindSummaryFields(Combobox combobox, IEnumerable<Field> fields)
        {
            foreach (var field in fields)
            {
                var shortValue = field.ShortValue();
                if (string.IsNullOrEmpty(shortValue))
                    continue;
                combobox.Controls.Add(new ListItem() { Header = shortValue, Value = field.ID.ToString() });
            }
        }


        /// <summary>
        /// Binds the fields to a combobox
        /// </summary>
        private static void BindFields(Combobox combobox, IEnumerable<Field> fields)
        {
            var fieldGroups = fields.GroupBy(f => f.SectionDisplayName);

            fieldGroups.ForEach(
                fieldGroup =>
                {
                    combobox.Controls.Add(new ListItem() { Header = fieldGroup.Key, Value = string.Empty, Disabled = true });
                    fieldGroup.ForEach(field =>
                    {
                        combobox.Controls.Add(new ListItem() { Header = field.Name, Value = field.ID.ToString() });
                    });
                }
            );
        }

        #region Click events

        /// <summary>
        /// Triggered when generate is clicked.
        /// </summary>
        protected void GenerateClick()
        {
            Generate(false);
        }

        /// <summary>
        /// Triggered when generate more is clicked.
        /// </summary>
        protected void GenerateMoreClick()
        {
            Generate(true);
        }

        /// <summary>
        /// Triggered when replace is clicked.
        /// </summary>
        protected void ReplaceClick()
        {
            Close(SitecoreSuggest.Constants.Replace);
        }

        /// <summary>
        /// Triggered when append is clicked.
        /// </summary>
        protected void AppendClick()
        {
            Close(SitecoreSuggest.Constants.Append);
        }

        /// <summary>
        /// Triggered when closed is clicked.
        /// </summary>
        protected void CloseClick()
        {
            Close();
        }

        #endregion
    }
}