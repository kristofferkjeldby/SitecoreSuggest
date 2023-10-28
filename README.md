# SitecoreSuggest

![Example](https://raw.githubusercontent.com/kristofferkjeldby/SitecoreSuggest/main/readme.png)

## Introduction

SitecoreSuggest is an integration between Sitecore XM 10.3 and the GPT language models offered by Open AI. It allows the Content Editor to use generative AI to generate summaries and texts directly in Sitecore and use these texts as content.

## Getting Started

To get started, first you need to download or clone the solution and open it in Visual Studio.

### Configuration Sitecore

As part of the solution a Sitecore package is provided ([SitecoreSuggest.zip](SitecoreSuggest.zip)). The package should be installed on your (local) Sitecore CM server and will add three items in the core database:

```
/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Proofing/Suggest
/sitecore/content/Applications/Dialogs/Suggest
/sitecore/layout/Layouts/Dialogs/Suggest
```

These items will add the needed configuration items to add the menu item *Suggest* under the *Review* ribbon in the Content Editor (see screenshot above).

### Configuration Open AI

Next you will need to obtain a valid API key from Open AI. You can obtain a API key by creating an account on https://platform.openai.com/ and select your profile in the top right corner and click *View API keys*. Create a new API key and copy it.

Note that Open AI offers both free and paid access but impose limitations on free access. I recommend that you pay some money into your account (like 10$) and turn off *Auto recharge*. This will give you plenty of credit to play around with SitecoreSuggest without risking being changed unforeseen money.

### Configuring and publish the solution

In Visual Studio open the file [App_Config\Include\SitecoreSuggest.config](SitecoreSuggest/App_Config/Include/SitecoreSuggest.config) and uncomment the `SitecoreSuggest.ApiKey` setting and insert your API key. 

Setup a publish profile to publish the solution into your local Sitecore CM web root and publish the solution. Please note that the solution is setup for Sitecore 10.3. If you wish to use the solution in other version of Sitecore, make sure to adjust the NuGet packages (`Sitecore.Kernel` and `Newtonsoft.Json`) to match the expected versions.

You can of cause also include the code files in you own solution.

When this is done, you should be able to login to Sitecore, select an content item, open the *Review* ribbon and click *Suggest* to start generating content for the selected item based on a prompt (and question).

## Supported models

SitecoreSuggest supports two different Open AI endpoints - the _completions_ and the _chat_ endpoint as well and a number of models exposed by each of these endpoints. 

The `Endpoint` ("completions" or "chat") and the `Model` settings are configured in the file [App_Config\Include\SitecoreSuggest.config](SitecoreSuggest/App_Config/Include/SitecoreSuggest.config):

```
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/" xmlns:set="http://www.sitecore.net/xmlconfig/set/" xmlns:role="http://www.sitecore.net/xmlconfig/role/">
  <sitecore role:require="ContentManagement or Standalone">
    <commands>
      <command name="custom:SitecoreSuggest" type="SitecoreSuggest.Commands.SuggestCommand, SitecoreSuggest"/>
    </commands>
    <settings>
      <!--<setting name="SitecoreSuggest.ApiKey" value=""/>-->
      <setting name="SitecoreSuggest.BaseUrl" value="https://api.openai.com/v1"/>
      <!-- Please make sure to match the endpoint ("completions" or "chat") to the selected model as the model exposed by the endpoints are not the same -->
      <setting name="SitecoreSuggest.Endpoint" value="completions"/>
      <setting name="SitecoreSuggest.Model" value="text-davinci-003"/>
      <setting name="SitecoreSuggest.MaxTokens" value="4097"/>
    </settings>
  </sitecore>
</configuration>
```

Be aware that the models supported are _not_ the same for the two endpoints: The models exposed by the chat endpoint have been optimized for multi-turn chats with a chat context (previous prompts and replies) whereas the completion endpoint simply support as single prompt and a reply. 

Hence it is important to match the `Endpoint` (whether to call the completions or chat endpoint) and `Model` settings to avoid calling e.g., a chat model via the completions endpoint (which will result in an error). Also, different models allow different maximum tokens sizes (the combined size of the prompt, reply and chat context) - this is set using the `MaxTokens` setting. As number of models for both endpoints changes often, this list is probably already outdated, but SitecoreSuggest have been tested with the following combination of `Endpoint`, `Model` and `MaxTokens` settings:

|Endpoint|Model|MaxTokens|
|---|---|---|
|completions|text-davinci-003|4097|
|completions|text-davinci-002|4097|
|completions|text-davinci-001|2049|
|completions|text-curie-001|2049|
|completions|text-babbage-001|2049|
|completions|text-ada-001|2049|
|chat|gpt-4|8193|
|chat|gpt-3.5-turbo|4097|

Some of the completions models are really old. The default model is set to `text-davinci-003` which is a completions model comparable to the `gpt-3.5-turbo` model (commonly referred to as ChatGPT), but without is multi-turn capability (that is back-and-forth chatting). 

## Language support

When sending a prompt to Open AI, SitecoreSuggest does some "prompt engineering" to steer the model in the right direction. In the screenshot above you can see that I have generated a summary of "Sitecore Experience Platform" (which is the title of the selected item) using a *Medium* word count. Behind the scenes, this is formatted into the following prompt: 

```
Write summary of "Sitecore Experience Platform". Use about 100 words.
```

With a chat model such instructions could also be put into the chat context as special "System" message, but to support the completions models SitecoreSuggest put it into a single prompt.

The language of the prompt directs the GPT model to reply in the same language. This means that if support for other languages is needed, we need to add prompts for generation of summaries and restrict the word count. This is done in the [Languages.cs](SitecoreSuggest/Languages.cs) file. Out of the box SitecoreSuggest supports two languages (`en` and `da`). In other languages the *Suggest* button will be grayed out unless prompts are added for that language in the [Languages.cs](SitecoreSuggest/Languages.cs) file. 

## Advanced settings

### Text length

The UI supports three different text lengths:

- Short: 20 words
- Medium: 100 words
- Long: 700 words

If needed this can be adjusted in the file [sitecore\shell\Applications\Dialogs\Suggest\SuggestForm.xml](SitecoreSuggest/sitecore/shell/Applications/Dialogs/Suggest/SuggestForm.xml) line 44-46. Very long texts might result in a unresponsive UI and might also give problems with the maximum token length depending on the model.

### Creativity

The UI supports three levels of *creativity* - techically called temperature:

- Low: 0.2
- Medium: 0.5
- High: 0.8

Most GPT models support temperatures up to 2, indicating the amount of randomness thrown into the token generation. Temperatures higher that 1 tend to produce text close to gibberish. 

If needed the available temperatures can be adjusted in [sitecore\shell\Applications\Dialogs\Suggest\SuggestForm.xml](SitecoreSuggest/sitecore/shell/Applications/Dialogs/Suggest/SuggestForm.xml) line 54-56.

### Summary fields

SitecoreSuggest allow the content editor to input a custom prompt or to use the value of one of the existing fields of an item to generate summaries.

The use case, as illustrated in the screenshot above is to create a content item, enter a title (e.g. "Sitecore Experience Platform") and then generate a summary of the title to use in the main text fields of a content item.

The fields available for summary generating summaries are configured in the [Constants.cs](SitecoreSuggest/Constants.cs) file using the `SummaryFields` array and is per default set to allow summaries from only single-line text fields. 

Notice that the summary dropdown will only display the first 70 chars of the field, but the content of the entire field will be used when generating summaries. In no summary fields exist (or they are empty), the summary dropdown is greyed out.

### Supported field

When SitecoreSuggest has generated a suggestion, is it possible to either append or insert the suggestion into a field on the selected item. 

The fields where appending and inserting are supported are configured in the [Constants.cs](SitecoreSuggest/Constants.cs) filed in the `SupportedFields` array. The default configuration is `single-line text`, `multi-line text` and `rich text` fields. The field types that expect HTML (rich text) also need to be added to the `HtmlFields` array to allow SitecoreSuggest to format the suggestions using HTML. 

## Read more

You can read more about SitecoreSuggest on my blog: https://www.kjeldby.dk/2023/10/integrating-sitecore-with-generative-ai-and-chatgpt/

