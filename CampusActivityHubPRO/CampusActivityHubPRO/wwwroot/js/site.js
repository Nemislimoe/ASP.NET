// Simple site JS: AJAX helpers for tags and registration
async function tagAutocomplete(q) {
    const res = await fetch('/api/tags/autocomplete?q=' + encodeURIComponent(q));
    if (!res.ok) return [];
    return await res.json();
}

async function registerEvent(eventId) {
    const res = await fetch('/api/events/' + eventId + '/register', { method: 'POST' });
    return res;
}
