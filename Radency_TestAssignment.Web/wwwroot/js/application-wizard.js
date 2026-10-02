document.addEventListener('submit', async function (e) {
    const form = e.target.closest('[data-wizard-form]');
    if (!form) return;

    e.preventDefault();

    const host = form.closest('[data-wizard-host]');
    const response = await fetch(form.action, {
        method: 'POST',
        body: new FormData(form, e.submitter),
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    });

    if (response.status === 404 || response.status === 409) {
        location.reload();
        return;
    }

    if (!response.ok) {
        showAlert('Something went wrong. Please try again.', 'danger');
        return;
    }

    const contentType = response.headers.get('content-type') ?? '';
    if (contentType.includes('application/json')) {
        const data = await response.json();

        if (data.redirectUrl) {
            location.href = data.redirectUrl;
            return;
        }

        if (data.message) {
            showAlert(data.message, data.type);
        }

        return;
    }

    host.innerHTML = await response.text();

    if (window.jQuery && jQuery.validator && jQuery.validator.unobtrusive) {
        jQuery.validator.unobtrusive.parse(host);
    }
});