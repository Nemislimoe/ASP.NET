document.addEventListener('DOMContentLoaded', function () {
    const tagsInput = document.getElementById('tags-input');
    if (!tagsInput) return;

    let timeout;
    tagsInput.addEventListener('input', function () {
        clearTimeout(timeout);
        timeout = setTimeout(async () => {
            const q = tagsInput.value;
            if (!q) return;
            const res = await fetch(`/api/tags/autocomplete?q=${encodeURIComponent(q)}`);
            const items = await res.json();
            let dl = document.getElementById('tags-datalist');
            if (!dl) {
                dl = document.createElement('datalist');
                dl.id = 'tags-datalist';
                document.body.appendChild(dl);
                tagsInput.setAttribute('list', 'tags-datalist');
            }
            dl.innerHTML = '';
            items.forEach(i => {
                const opt = document.createElement('option');
                opt.value = i;
                dl.appendChild(opt);
            });
        }, 300);
    });
});

async function registerEvent(eventId) {
    const res = await fetch(`/events/${eventId}/register`, { method: 'POST', credentials: 'include' });
    if (res.ok) {
        alert('Ви успішно зареєстровані.');
        location.reload();
    } else {
        const txt = await res.text();
        alert('Помилка: ' + txt);
    }
}
