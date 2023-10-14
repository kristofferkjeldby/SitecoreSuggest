namespace SitecoreSuggest.Dialogs
{
    using Newtonsoft.Json;
    using Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Web;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.Pages;
    using SitecoreSuggest.Extensions;
    using SitecoreSuggest.Models;
    using SitecoreSuggest.Service;
    using System;

    public class SuggestForm : DialogForm
    {
        protected Literal ItemLiteral;
        protected Combobox PromptFieldIdComboBox;
        protected Edit PromptEdit;
        protected Combobox WordsCombobox;
        protected Combobox FieldIdCombobox;
        protected Memo SuggestionMemo;
        protected SuggestService SuggestService = new SuggestService();

        protected void CancelClick()
        {
            Context.ClientPage.ClientResponse.CloseWindow();
        }

        protected void GenerateClick()
        {
            var payload = GetPayload();

            payload.PromptFieldId = PromptFieldIdComboBox.SelectedItem.Value;
            payload.Prompt = PromptEdit.Value;
            payload.Words = WordsCombobox.SelectedItem.Value.ParseLength();

            SuggestionMemo.Value = SuggestService.GenerateSuggestion(payload);
        }

        protected void UseClick()
        {
            var payload = GetPayload();

            payload.Action = "use";
            payload.Suggestion = SuggestionMemo.Value;
            payload.FieldId = FieldIdCombobox.SelectedItem.Value;

            Context.ClientPage.ClientResponse.SetDialogValue(JsonConvert.SerializeObject(payload));
            Context.ClientPage.ClientResponse.CloseWindow();
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, nameof(e));

            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;

            var payload = GetPayload();

            var item = payload.GetItem();
            this.ItemLiteral.Text = item.Name;
            BindFields(payload);
        }

        private static SuggestFormPayload GetPayload()
        {
            var payloadJson = WebUtil.GetQueryString("payload");
            var payload = JsonConvert.DeserializeObject<SuggestFormPayload>(payloadJson);
            return payload;
        }

        private void BindFields(SuggestFormPayload payload)
        {
            FieldIdCombobox.Controls.Clear();

            foreach (var field in payload.GetFields())
            {
                FieldIdCombobox.Controls.Add(new ListItem() { Header = field.Name, Value = field.ID.ToString() });
                if (field.IsSummaryField())
                {
                    var shortValue = field.ShortValue();
                    PromptFieldIdComboBox.Controls.Add(new ListItem() { Header = shortValue, Value = field.ID.ToString() });
                }
            }
        }

        public SuggestForm() : base()
        {

        }
    }
}