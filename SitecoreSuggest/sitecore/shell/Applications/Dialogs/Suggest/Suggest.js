window.addEventListener('load', sitecoreSuggest_onLoad);

function sitecoreSuggest_onLoad(event) {

    let summaryFieldIdCombobox = document.getElementById("SummaryFieldIdCombobox");
    let promptEdit = document.getElementById("PromptEdit");
    let generateButton = document.getElementById("GenerateButton");
    let generateMoreButton = document.getElementById("GenerateMoreButton");
    let suggestionMemo = document.getElementById("SuggestionMemo");
    let fieldIdComboBox = document.getElementById("FieldIdCombobox");
    let copyButton = document.getElementById("CopyButton"); 
    let appendButton = document.getElementById("AppendButton");
    let replaceButton = document.getElementById("ReplaceButton");

    function init() {
        initGenerate();
        initSuggestion(false);
        initAppendReplace();
    }

    // Enable generate button is prompt field or prompt is entered
    function initGenerate() {
        const disabled = summaryFieldIdCombobox.value.length == 0 && promptEdit.value.length == 0;
        generateButton.disabled = disabled;
        promptEdit.disabled = summaryFieldIdCombobox.value.length > 0;
        summaryFieldIdCombobox.disabled = promptEdit.value.length > 0;
    }

    // Enable append and replace buttons if field is selected
    function initAppendReplace() {
        const disabled = fieldIdComboBox.value.length == 0;
        appendButton.disabled = disabled;
        replaceButton.disabled = disabled;
    }

    // Enable suggestion is suggestion is filled out
    function initSuggestion(forceEnable) {
        const disabled = forceEnable ? false : suggestionMemo.value.length == 0;
        generateMoreButton.disabled = disabled;
        suggestionMemo.disabled = disabled;
        fieldIdComboBox.disabled = disabled;
        copyButton.disabled = disabled;

        if (disabled) {
            suggestionMemo.classList.add("sitecoreSuggest_Disabled");
            generateButton.innerText = "Generate";
        }
        else {
            suggestionMemo.classList.remove("sitecoreSuggest_Disabled");
            generateButton.innerText = "Regenerate";
        }
    }

    function showLoader() {
        suggestionMemo.value = "Please wait ...";
    }

    // Setup changes to UI
    summaryFieldIdCombobox.addEventListener("input", initGenerate);
    promptEdit.addEventListener("input", initGenerate);
    fieldIdComboBox.addEventListener("input", initAppendReplace);
    generateButton.addEventListener("click", () => initSuggestion(true));

    // Setup loaders
    generateButton.addEventListener("mousedown", showLoader);

    copyButton.addEventListener("click", async () => {
        try {
            await navigator.clipboard.writeText(suggestionMemo.value);
        } catch (err) {
            alert("Could not access clipboard: " + err);
        }
    });

    init();
}