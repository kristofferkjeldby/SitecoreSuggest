# SitecoreSuggest

![Example](https://raw.githubusercontent.com/kristofferkjeldby/SitecoreSuggest/main/readme.png)

## Introduction

SitecoreSuggest is a integration between Sitecore XM 10.3 and the GPT language models offered by Open AI. It allow the Content Editor to use generative AI to generate summaries and texts directly in Sitecore and use these texts are content.

## Getting Started

To get started, first you need to download or clone the solution and open it in Visual Studio.

### Configuration Sitecore

As part of the solution a Sitecore package is provided (`SitecoreSuggest.zip`). The package should be installed in Sitecore and will add three items in the core database:

```
/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Proofing/Suggest
/sitecore/content/Applications/Dialogs/Suggest
/sitecore/layout/Layouts/Dialogs/Suggest
```

These items will add the needed configuration items to add the menu item *Suggest* under the *Review* ribbon in the Content Editor (see screenshot above).

### Configuration Open AI

Next you will need to obtain a valid API key from Open AI. You can obtain a API key by creating an account on https://platform.openai.com/, and select the profile in the top right corner and click *View API keys*. Create a new API key and copy it.

Note that Open AI offers both free and paid access but impose limitations on free access. I recommend that you pay some money into your account (like 10$) and turn off *Auto recharge*. This will give you plenty of credit to play around with SitecoreSuggest without risking being changed unforseen money.

### Configuring and publish the solution

In Visual Studio open the file `App_Config\Include\SitecoreSuggest.config` and uncomment the SitecoreSuggest.ApiKey setting and insert your API key. 

Setup a publish profile to publish the solution into your local Sitecore CM web root and publish the solution. Please note that the solution is setup for Sitecore 10.3. If you wish to use the solution in other version of Sitecore, make sure to adjust the NuGet packages (`Sitecore.Kernel` and `Newtonsoft.Json`) to match the expected versions.

You can of cause also include the code files in you own solution.

When this is done, you should be able to login to Sitecore, select an content item, open the *Review* ribbon and click *Suggest* to start generating content. 

# Supported models

SitecoreSuggest supports a number of the models offered by Open AI, both the chat and completions models. The model to use is configured in the `App_Config\Include\SitecoreSuggest.config` and is per default set to `text-davinci-003` which is a completion model comparable to the `gpt-3.5-turbo` model (commonly referred to as ChatGPT), but without is multi-turn capability (that is back-and-forth chatting). 

Hence it is a bit faster and provides less "chatty" results than the chat models. I suggest you start by experimenting with these two models, but other models that should work - at least to some extend - are:

_Chat models_

- gpt-4
- gpt-4-0613
- gpt-4-32k
- gpt-4-32k-0613
- gpt-3.5-turbo
- gpt-3.5-turbo-0613
- gpt-3.5-turbo-16k
- gpt-3.5-turbo-16k-0613

_Completion models_

- davinci-002 
- babbage-002 
- text-davinci-003 
- text-davinci-002 
- text-davinci-001 
- text-curie-001 
- text-babbage-001 
- text-ada-001 
- davinci 
- curie 
- babbage 
- ada

Please be aware that some of these models are really experimental and/or old. Also different models allow different max tokens sizes (result size). The max value and what that means in turn of words differs from model to model. The default setting is `4096` tokens with is a reasonable limit for both the `text-davinci-003` and `gpt-3.5-turbo` models, but this can be adjusted using the `SitecoreSuggest.MaxTokens` setting if you run into problems.

## Language support

When sending a request (a prompt) to Open AI, SitecoreSuggest does some "prompt engineering" to steer the GPT model in the right direction. In the screenshot above you can see that I have generated a summary of "Sitecore Experience Platform" (which is the title of the selected item) using a *Medium* word count. Behind the scenes, this is formatted into the following prompt: 

```
Write summary of "Sitecore Experience Platform". Use about 100 words.
```

The language of the prompt directs the GPT model to reply in the same language. This means that if support for other languages are needed, we need to add prompts for generation of summaries and restrict the word count. This is done in the `Languages.cs` file. Out of the box SitecoreSuggest supports two languages (`en` and `da`). In other languages the *Suggest* button will be grayed out unless prompts are added for that language in the `Languages.cs` file. 

## Advanced settings

*Text length*

The UI supports three different text lengths:

- Short: 20 words
- Medium: 100 words
- Long: 700 words

If needed this can be adjusted in the file `SitecoreSuggest\SitecoreSuggest\sitecore\shell\Applications\Dialogs\Suggest\SuggestForm.xml` line 44-46. Very long texts might result in a unresponsive UI and might also give problems with the maximum token length depending on the model.

*Creativity*

The UI supports three levels of *creativity* - techically called temperature:

- Low: 0.2
- Medium: 0.5
- High: 0.8

Most GPT models support temperatures up to 2, indicating the amount of randomness thrown into the token generation. Temperatures higher that 1 tend to produce text borderlining gibberish. 

If needed the available temperatures can be adjusted in `SitecoreSuggest\SitecoreSuggest\sitecore\shell\Applications\Dialogs\Suggest\SuggestForm.xml` line 54-56.

*Summary fields*

SitecoreSuggest allow the content editor to input a custom prompt of use the value of one of the existing field values off an item to generate summaries.

The use case, as illustrated in the screenshot above is to create a content item, enter a title (e.g. "Sitecore Experience Platform") and then generate a summary of the title to use in the main text fields of the content item.

The fields available for summary generating summaries are configured in the Constants.cs file using the SummaryFields array and is per default set to allow summaries from only single-line text fields. 

Notice that the summary dropdown will only display the first 70 chars of the field, but the content of the entire field will be used when generating summaries.


