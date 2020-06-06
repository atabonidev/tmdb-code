function focusElement (id) {
    const element = document.getElementById(id);
    element.classList.add("focus");
}

function focusOutElement(id) {
    const element = document.getElementById(id);
    element.classList.remove("focus");
}