let hideTimer;
let displayTimer;

const getAlertEls = () => ({
    container: document.getElementById("liveAlertPlaceholder"),
    box: document.getElementById("alertBox"),
    text: document.getElementById("alertMessage")
});

function showAlert(message, type = "success") {
    const { container, box, text } = getAlertEls();
    if (!container) return;

    if (message !== undefined) {
        text.textContent = message;
        box.className = `alert alert-dismissible alert-${type}`;
    }

    clearTimeout(hideTimer);
    clearTimeout(displayTimer);

    container.style.display = "block";
    container.classList.remove("alert-hide");
    container.classList.add("alert-show");

    hideTimer = setTimeout(hideAlert, 2000);
}

function hideAlert() {
    const { container } = getAlertEls();
    if (!container) return;

    clearTimeout(hideTimer);
    clearTimeout(displayTimer);

    container.classList.remove("alert-show");
    container.classList.add("alert-hide");

    displayTimer = setTimeout(() => { container.style.display = "none"; }, 500);
}

function showAlertAfterReload(message, type) {
    sessionStorage.setItem("pendingAlert", JSON.stringify({ message, type }));
}

document.addEventListener("DOMContentLoaded", () => {
    const pending = sessionStorage.getItem("pendingAlert");
    if (pending) {
        sessionStorage.removeItem("pendingAlert");
        const { message, type } = JSON.parse(pending);
        showAlert(message, type);
        return;
    }

    const { text } = getAlertEls();
    if (text && text.textContent.trim() !== "") showAlert();
});