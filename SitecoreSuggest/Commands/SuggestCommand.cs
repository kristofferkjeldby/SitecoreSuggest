namespace SitecoreSuggest.Commands
{
    using Newtonsoft.Json;
    using Sitecore;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Text;
    using Sitecore.Web.UI.Sheer;
    using SitecoreSuggest.Extensions;
    using SitecoreSuggest.Models;
    using System.Collections.Specialized;
    using System.Linq;

    public class SuggestCommand : Command
    {
        public override CommandState QueryState(CommandContext context)
        {
            var item = context.Items.FirstOrDefault();

            if (!item.IsSupported())
                return CommandState.Disabled;

            return CommandState.Enabled;
        }

        public override void Execute(CommandContext context)
        {
            var item = context.Items.FirstOrDefault();

            if (item == null)
                return;

            var payload = new SuggestFormPayload()
            {
                ItemId = item.ID.ToString(),
                Language = item.Language.ToString(),
                Database = item.Database.ToString(),
                Version = item.Version.Number
            };

            var parameters = new NameValueCollection
            {
                { "payload", JsonConvert.SerializeObject(payload) },
            };

            Context.ClientPage.Start(this, nameof(Suggest), parameters);
        }

        public void Suggest(ClientPipelineArgs args)
        {
            if (!args.IsPostBack)
            {
                UrlString urlString = new UrlString(UIUtil.GetUri("control:SuggestForm"));
                urlString.Append("payload", args.Parameters["payload"]);
                SheerResponse.ShowModalDialog(urlString.ToString(), "600px", "400px", string.Empty, true);
                args.WaitForPostBack();
            }
            else
            {
                var payload = JsonConvert.DeserializeObject<SuggestFormPayload>(args.Result);

                if (payload?.Action == SitecoreSuggest.Constants.Insert)
                    payload.UpdateField(false);
                if (payload?.Action == SitecoreSuggest.Constants.Append)
                    payload.UpdateField(true);

            }
        }
    }
}