(() => {
    const modalEl = document.getElementById('appModal');
    const content = document.getElementById('appModalContent');
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    const headers = { 'X-Requested-With': 'XMLHttpRequest' };

    function setContent(html) {
        content.innerHTML = html;
        if (window.jQuery?.validator?.unobtrusive) {
            jQuery.validator.unobtrusive.parse(content);
        }

        content.querySelectorAll("input.input-validation-error")
            .forEach(elem => elem.classList.add("is-invalid"));

        content.querySelectorAll("input.form-control, select.form-select")
            .forEach(elem => {
                new ClassWatcher(
                    elem,
                    "input-validation-error",
                    () => elem.classList.add("is-invalid"),
                    () => elem.classList.remove("is-invalid")
                );
            });
    }

    function handleError(message = "Something went wrong. Please try again.") {
        document.activeElement.blur();
        modal.hide();
        content.innerHTML = '';
        showAlert(message, "danger");
    }

    document.addEventListener('click', async e => {
        const trigger = e.target.closest('[data-modal-url]');
        if (!trigger) return;
        e.preventDefault();

        try {
            const res = await fetch(trigger.dataset.modalUrl, { headers });
            if (!res.ok) throw new Error(res.status);
            setContent(await res.text());
            modal.show();
        } catch {
            handleError();
        }
    });

    content.addEventListener('submit', async e => {
        const form = e.target.closest('form[data-modal-form]');
        if (!form) return;
        e.preventDefault();

        try {
            const res = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form),
                headers
            });

            const isJson = res.headers.get('content-type')?.includes('application/json');

            if (res.ok && isJson) {
                document.activeElement.blur();
                modal.hide();
                document.dispatchEvent(new CustomEvent('modal:saved', {
                    detail: { source: form, result: await res.json() }
                }));
            } else if (res.ok) {
                setContent(await res.text());
            } else {
                throw new Error(res.status);
            }
        } catch {
            handleError();
        }
    });

    modalEl.addEventListener('hidden.bs.modal', () => { content.innerHTML = ''; });

    document.addEventListener('modal:saved', e => {
        const result = e.detail.result;
        if (result.message) {
            showAlert(result.message, result.type ?? "alert-success");
        }
    });
})();
