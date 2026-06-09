window.activeToken = null;
let tokenOffsetX = 0; let tokenOffsetY = 0;
const ctxMenu = document.getElementById('token-context-menu');
const ctxClose = document.getElementById('ctx-close');
const inputCurrHp = document.getElementById('ctx-current-hp');
const inputMaxHp = document.getElementById('ctx-max-hp');
const inputMod = document.getElementById('ctx-hp-mod');
let contextTokenId = null;

window.isPlacingMonster = false;
window.pendingMonsterData = null;

window.bindTokenEvents = function (token) {
    token.addEventListener('mousedown', (e) => {
        if (!isDM && token.getAttribute('data-owner-id') !== currentUserId) return;
        e.stopPropagation();
        window.activeToken = token; window.activeToken.classList.add('dragging'); window.activeToken.style.transition = "none";
        const rect = window.activeToken.getBoundingClientRect();
        tokenOffsetX = (e.clientX - rect.left) / window.mapScale;
        tokenOffsetY = (e.clientY - rect.top) / window.mapScale;
    });

    token.addEventListener('contextmenu', (e) => {
        if (!isDM && token.getAttribute('data-owner-id') !== currentUserId) return;
        e.preventDefault();
        contextTokenId = parseInt(token.getAttribute('data-token-id'));
        inputCurrHp.value = parseInt(token.getAttribute('data-current-hp')) || 0;
        inputMaxHp.value = parseInt(token.getAttribute('data-max-hp')) || 0;
        inputMod.value = "";
        document.getElementById('ctx-token-name').innerText = token.getAttribute('data-token-name') || "Unknown Token";
        ctxMenu.style.display = 'block'; ctxMenu.style.left = `${e.pageX}px`; ctxMenu.style.top = `${e.pageY}px`;
    });
};

document.querySelectorAll('.game-token').forEach(window.bindTokenEvents);

window.addEventListener('mousemove', (e) => {
    if (!window.activeToken) return;
    const mapRect = document.getElementById('map-container').getBoundingClientRect();
    window.activeToken.style.transition = "none";
    window.activeToken.style.left = `${(e.clientX - mapRect.left) / window.mapScale - tokenOffsetX}px`;
    window.activeToken.style.top = `${(e.clientY - mapRect.top) / window.mapScale - tokenOffsetY}px`;
});

window.addEventListener('mouseup', () => {
    if (!window.activeToken) return;
    let snappedX = Math.round(parseFloat(window.activeToken.style.left) / 50) * 50;
    let snappedY = Math.round(parseFloat(window.activeToken.style.top) / 50) * 50;

    window.activeToken.style.transition = "left 0.1s ease-out, top 0.1s ease-out";
    window.activeToken.style.left = `${snappedX}px`; window.activeToken.style.top = `${snappedY}px`;
    window.activeToken.classList.remove('dragging');

    const tokenId = parseInt(window.activeToken.getAttribute('data-token-id'));
    window.connection.invoke("UpdateTokenPosition", gameId, tokenId, snappedX, snappedY).catch(console.error);

    const currentToken = window.activeToken;
    setTimeout(() => { currentToken.style.transition = "transform 0.1s, box-shadow 0.1s"; }, 100);
    window.activeToken = null;
});

document.addEventListener('click', (e) => { if (ctxMenu && !ctxMenu.contains(e.target)) ctxMenu.style.display = 'none'; });
ctxClose?.addEventListener('click', () => ctxMenu.style.display = 'none');

function applyHpChange(newCurrentHp, newMaxHp) {
    if (!contextTokenId) return;
    newMaxHp = Math.max(1, newMaxHp); newCurrentHp = Math.max(0, Math.min(newCurrentHp, newMaxHp));
    window.connection.invoke("UpdateTokenHp", gameId, contextTokenId, newCurrentHp, newMaxHp).catch(console.error);
    ctxMenu.style.display = 'none';
}

document.getElementById('btn-dmg')?.addEventListener('click', () => {
    let curr = parseInt(inputCurrHp.value); let max = parseInt(inputMaxHp.value); let mod = parseInt(inputMod.value) || 0;
    let newHp = Math.max(0, Math.min(curr - mod, max)); applyHpChange(newHp, max);
    window.connection.invoke("SendMessageToGame", gameId, "System", `[SYSTEM] ${document.getElementById('ctx-token-name').innerText} takes ${mod} damage! (${newHp}/${max} HP)`).catch(console.error);
});

document.getElementById('btn-heal')?.addEventListener('click', () => {
    let curr = parseInt(inputCurrHp.value); let max = parseInt(inputMaxHp.value); let mod = parseInt(inputMod.value) || 0;
    let newHp = Math.max(0, Math.min(curr + mod, max)); applyHpChange(newHp, max);
    window.connection.invoke("SendMessageToGame", gameId, "System", `[SYSTEM] ${document.getElementById('ctx-token-name').innerText} is healed for ${mod} HP! (${newHp}/${max} HP)`).catch(console.error);
});

document.getElementById('btn-save-hp')?.addEventListener('click', () => {
    let newHp = parseInt(inputCurrHp.value); let max = parseInt(inputMaxHp.value); applyHpChange(newHp, max);
    window.connection.invoke("SendMessageToGame", gameId, "System", `[SYSTEM] ${document.getElementById('ctx-token-name').innerText}'s HP is set to ${newHp}/${max}.`).catch(console.error);
});

document.getElementById('btn-long-rest')?.addEventListener('click', () => {
    document.querySelectorAll('.game-token').forEach(token => {
        let max = parseInt(token.getAttribute('data-max-hp')) || 100;
        window.connection.invoke("UpdateTokenHp", gameId, parseInt(token.getAttribute('data-token-id')), max, max).catch(console.error);
    });
    window.connection.invoke("SendMessageToGame", gameId, "System", "[SYSTEM] The party takes a Long Rest. Everyone is fully healed!").catch(console.error);
});

document.getElementById('btn-global-hp')?.addEventListener('click', () => {
    let input = prompt("Enter new Max HP for ALL tokens on the board:", "1000");
    if (input === null || input.trim() === "") return;
    let newMaxHp = parseInt(input);
    if (isNaN(newMaxHp) || newMaxHp <= 0) { alert("Please enter a valid number greater than 0."); return; }
    document.querySelectorAll('.game-token').forEach(token => {
        window.connection.invoke("UpdateTokenHp", gameId, parseInt(token.getAttribute('data-token-id')), newMaxHp, newMaxHp).catch(console.error);
    });
    window.connection.invoke("SendMessageToGame", gameId, "System", `[SYSTEM] ⚙️ The Dungeon Master has shifted reality! Everyone's Max HP is now ${newMaxHp}!`).catch(console.error);
});

let spawnModal;
const spawnModalElement = document.getElementById('spawnMonsterModal');
if (spawnModalElement) spawnModal = new bootstrap.Modal(spawnModalElement);

document.getElementById('btn-spawn-monster')?.addEventListener('click', () => { spawnModal.show(); });

document.getElementById('btn-confirm-spawn')?.addEventListener('click', () => {
    const hp = parseInt(document.getElementById('monster-hp').value);
    if (isNaN(hp) || hp <= 0) { alert("Please enter a valid HP."); return; }
    window.pendingMonsterData = { name: document.getElementById('monster-name').value, hp, color: document.getElementById('monster-color').value };
    window.isPlacingMonster = true; document.getElementById('viewport').style.cursor = "crosshair"; spawnModal.hide();
    const msgWrapper = document.createElement('div'); msgWrapper.className = "d-flex w-100 mb-2 justify-content-center";
    msgWrapper.innerHTML = `<div class="w-100 text-center py-2 px-3 rounded shadow-sm border border-danger" style="background-color: #4a0000; max-width: 95%;"><div class="fst-italic text-light fw-bold" style="font-size: 0.85rem;">Click anywhere on the map to place the monster! (Press ESC to cancel)</div></div>`;
    const chatBox = document.querySelector('.chat-messages');
    chatBox.appendChild(msgWrapper); chatBox.scrollTop = chatBox.scrollHeight;
});

document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && window.isPlacingMonster) { window.isPlacingMonster = false; window.pendingMonsterData = null; document.getElementById('viewport').style.cursor = "grab"; }
});

document.querySelectorAll('.btn-open-sheet').forEach(btn => {
    btn.addEventListener('click', () => {
        ctxMenu.style.display = 'none'; new bootstrap.Modal(document.getElementById('characterSheetModal')).show();
    });
});

document.getElementById('btn-open-notes')?.addEventListener('click', () => {
    const savedNotes = localStorage.getItem(`vtt_notes_game_${gameId}_user_${currentUserId}`);
    document.getElementById('personal-notes-area').value = savedNotes ? savedNotes : "";
    new bootstrap.Modal(document.getElementById('notesModal')).show();
});

document.getElementById('btn-save-notes')?.addEventListener('click', () => {
    localStorage.setItem(`vtt_notes_game_${gameId}_user_${currentUserId}`, document.getElementById('personal-notes-area').value);
    const ns = document.getElementById('notes-save-status'); ns.style.opacity = "1"; setTimeout(() => { ns.style.opacity = "0"; }, 2000);
});

document.getElementById('btn-delete-token')?.addEventListener('click', () => {
    if (!contextTokenId) return;
    if (confirm("Are you sure you want to permanently delete this token?")) {
        window.connection.invoke("DeleteToken", gameId, contextTokenId).catch(console.error);
        ctxMenu.style.display = 'none';
    }
});