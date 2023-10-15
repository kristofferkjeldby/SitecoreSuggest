window.addEventListener('load', sitecoreSuggest_onLoad);

function sitecoreSuggest_onLoad(event) {

    let promptFieldIdComboBox = document.getElementById("PromptFieldIdComboBox");
    let promptEdit = document.getElementById("PromptEdit");
    let generateButton = document.getElementById("GenerateButton");
    let generateMoreButton = document.getElementById("GenerateMoreButton");
    let appendButton = document.getElementById("AppendButton");
    let insertButton = document.getElementById("InsertButton");

    function enable() {
        generateMoreButton.disabled = false;
        appendButton.disabled = false;
        insertButton.disabled = false;
    }

    function disable() {
        generateMoreButton.disabled = true;
        appendButton.disabled = true;
        insertButton.disabled = true;
    }

    promptEdit.addEventListener("input", function () {
        promptFieldIdComboBox.disabled = promptEdit.value.length > 0;
    });

    generateButton.addEventListener("click", function () {
        console.log("GenerateButton click");
        enable();
    });

    generateMoreButton.addEventListener("click", function () {
        console.log("GenerateMoreButton click");
    });

    disable();
}