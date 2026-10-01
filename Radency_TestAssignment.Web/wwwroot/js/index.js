window.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll("input.input-validation-error").forEach(elem => elem.classList.add("is-invalid"));
        document.querySelectorAll("input.form-control, select.form-select").forEach((elem) => {
            let classWatcher = new ClassWatcher(elem, "input-validation-error",
                () => elem.classList.add("is-invalid"),
                () => elem.classList.remove("is-invalid")
            )
        }); 
    });

    class ClassWatcher {

        constructor(targetNode, classToWatch, classAddedCallback, classRemovedCallback) {
            this.targetNode = targetNode
            this.classToWatch = classToWatch
            this.classAddedCallback = classAddedCallback
            this.classRemovedCallback = classRemovedCallback
            this.observer = null
            this.lastClassState = targetNode.classList.contains(this.classToWatch)

            this.init()
        }

        init() {
            this.observer = new MutationObserver(this.mutationCallback)
            this.observe()
        }

        observe() {
            this.observer.observe(this.targetNode, { attributes: true })
        }

        disconnect() {
            this.observer.disconnect()
        }

        mutationCallback = mutationsList => {
            for (let mutation of mutationsList) {
                if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                    let currentClassState = mutation.target.classList.contains(this.classToWatch)
                    if (this.lastClassState !== currentClassState) {
                        this.lastClassState = currentClassState
                        if (currentClassState) {
                            this.classAddedCallback()
                        }
                        else {
                            this.classRemovedCallback()
                        }
                    }
                }
            }
        }
}

$(function () {
    $("form").each(function () {
        const $form = $(this);
        const $submit = $form.find("[type=submit]");
        const validator = $form.data("validator");
        if (!validator || !$submit.length) return;

        const update = () => $submit.prop("disabled", !validator.checkForm());

        $form.on("input change", update);
        update();
    });
});


