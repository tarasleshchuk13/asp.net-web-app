const inputAll = document.querySelector('#all');
const userInputs = document.querySelectorAll('#user');
const buttonsWrapper = document.querySelector('#buttons')

function isAllUserInputsChecked() {
    for (const userInput of userInputs) {
        if (!userInput.checked) {
            return false;
        }
    }
    return true;
}

function setDisplayForButtonsWrapper() {
    for (const userInput of userInputs) {
        if (userInput.checked) {
            return buttonsWrapper.style.display = 'block';
        }
    }
    buttonsWrapper.style.display = 'none';
}
setDisplayForButtonsWrapper();

inputAll?.addEventListener('change', (event) => {
    const flag = isAllUserInputsChecked();
    userInputs.forEach(i => i.checked = flag ? false : true);
    setDisplayForButtonsWrapper();
})

userInputs.forEach(input => {
    input.addEventListener('change', (event) => {
        inputAll.checked = isAllUserInputsChecked();
        setDisplayForButtonsWrapper();
    })
})