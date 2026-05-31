
const aiModalElement = document.getElementById('aiModal');
const aiModal = new bootstrap.Modal(aiModalElement);

document.getElementById('btn-open-ai').addEventListener('click', () => {
    aiModal.show();
});

const chatHistory = document.getElementById('ai-chat-history');
const promptInput = document.getElementById('ai-prompt');
const btnGenerate = document.getElementById('btn-generate-ai');


function scrollToBottom() {
    chatHistory.scrollTop = chatHistory.scrollHeight;
}


function appendMessage(sender, text, isError = false) {
    const msgWrapper = document.createElement('div');
    const isUser = sender === 'User';

    msgWrapper.className = `d-flex w-100 mb-3 ${isUser ? 'justify-content-end' : 'justify-content-start'}`;

    const bgColor = isUser ? '#0d6efd' : '#2b2b2b';
    const borderColor = isUser ? '#0d6efd' : '#444';
    const titleColor = isUser ? 'text-light' : (isError ? 'text-danger' : 'text-warning');
    const icon = isUser ? '<i class="bi bi-person-fill"></i> Ty' : '<i class="bi bi-robot"></i> Gemini';
    const textColor = isError ? 'text-danger fw-bold' : 'text-white';

    msgWrapper.innerHTML = `
        <div class="p-2 rounded shadow-sm" style="background-color: ${bgColor}; border: 1px solid ${borderColor}; max-width: 85%;">
            <div class="${titleColor} small fw-bold mb-1">${icon}</div>
            <div class="${textColor}" style="white-space: pre-wrap; word-wrap: break-word;">${text}</div>
        </div>
    `;

    chatHistory.appendChild(msgWrapper);
    scrollToBottom();
}

btnGenerate.addEventListener('click', async () => {
    const prompt = promptInput.value.trim();
    if (!prompt) return;

    promptInput.value = '';
    appendMessage('User', prompt);

    btnGenerate.disabled = true;

    const loadingId = 'loading-' + Date.now();
    const loadingWrapper = document.createElement('div');
    loadingWrapper.id = loadingId;
    loadingWrapper.className = "d-flex w-100 mb-3 justify-content-start";
    loadingWrapper.innerHTML = `
        <div class="p-2 rounded shadow-sm" style="background-color: #2b2b2b; border: 1px solid #444; max-width: 85%;">
            <div class="text-warning small fw-bold mb-1"><i class="bi bi-robot"></i> Gemini</div>
            <div class="text-muted fst-italic">
                <span class="spinner-border spinner-border-sm me-2 text-warning" role="status"></span>Conjuring lore...
            </div>
        </div>
    `;
    chatHistory.appendChild(loadingWrapper);
    scrollToBottom();

    try {
        const gameId = parseInt(document.body.getAttribute('data-game-id'));
        const payload = {
            prompt: prompt,
            gameId: gameId,
            isInGameAssistant: true
        };

        const res = await fetch('/api/Ai/generate', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        document.getElementById(loadingId).remove();

        if (!res.ok) {
            const errText = await res.text();
            appendMessage('AI', "Request rejected" + errText, true);
        } else {
            const data = await res.json();
            appendMessage('AI', data.response);
        }

    } catch (error) {
        document.getElementById(loadingId)?.remove();
        appendMessage('AI', "Error connecting to your server C#: " + error.message, true);
    } finally {
        btnGenerate.disabled = false;
        promptInput.focus();
    }
});


promptInput.addEventListener('keydown', function (e) {
    if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault(); 
        btnGenerate.click();
    }
});