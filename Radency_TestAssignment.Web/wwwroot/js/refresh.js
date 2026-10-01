(() => {
    const headers = { "X-Requested-With": "XMLHttpRequest" };

    async function loadBlock(block, url) {
        const res = await fetch(url, { headers });
        if (!res.ok) throw new Error(res.status);

        block.innerHTML = await res.text();
        block.dataset.refreshUrl = url;
    }

    async function refreshAll() {
        const blocks = document.querySelectorAll("[data-refresh-url]");
        await Promise.all([...blocks].map(b => loadBlock(b, b.dataset.refreshUrl)));
        return blocks.length;
    }

    document.addEventListener("click", async e => {
        const link = e.target.closest("[data-page-url]");
        if (!link) return;
        e.preventDefault();

        const block = link.closest("[data-refresh-url]");
        if (!block) return;

        try {
            await loadBlock(block, link.dataset.pageUrl);
        } catch {
            showAlert("Could not load the list. Please try again.", "danger");
        }
    });

    window.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll("[data-load-on-init]").forEach(b =>
            loadBlock(b, b.dataset.refreshUrl).catch(() => { }));
    });

    window.BlockRefresh = { refreshAll, loadBlock };
})();