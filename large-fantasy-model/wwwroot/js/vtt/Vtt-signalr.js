window.connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

window.connection.on("UserStatusChanged", (userId, isOnline, username) => {
    const avatar = document.getElementById("player-" + userId);
    if (avatar) { if (isOnline) avatar.classList.add("online"); else avatar.classList.remove("online"); }
});

window.connection.on("UserAvatarChanged", function (username, newAvatarPath) {
    if (window.updateUserAvatars) {
        window.updateUserAvatars(username, newAvatarPath);
    }
});

window.connection.on("RequestStatusSync", () => { window.connection.invoke("ReportStatus", gameId, currentUserId).catch(console.error); });

window.connection.on("TokenMoved", (tokenId, x, y) => {
    const token = document.getElementById(`token-${tokenId}`);
    if (token && !token.classList.contains('dragging')) {
        token.style.transition = "left 0.2s ease-out, top 0.2s ease-out";
        token.style.left = `${x}px`; token.style.top = `${y}px`;
        setTimeout(() => { token.style.transition = "transform 0.1s, box-shadow 0.1s"; }, 200);
    }
});

window.connection.on("TokenSpawned", (tokenId, name, maxHp, color, x, y, ownerId) => {
    const tokenDiv = document.createElement('div');
    tokenDiv.className = "game-token d-flex justify-content-center align-items-center fw-bold";
    tokenDiv.id = `token-${tokenId}`;
    tokenDiv.setAttribute('data-token-id', tokenId); tokenDiv.setAttribute('data-owner-id', ownerId);
    tokenDiv.setAttribute('data-token-name', name); tokenDiv.setAttribute('data-current-hp', maxHp); tokenDiv.setAttribute('data-max-hp', maxHp);
    tokenDiv.style.left = `${x}px`; tokenDiv.style.top = `${y}px`; tokenDiv.style.backgroundColor = color;
    tokenDiv.innerHTML = `<div class="token-hp-container"><div class="token-hp-fill" style="width: 100%; background-color: #28a745;"></div></div>${name.substring(0, 2).toUpperCase()}`;
    document.getElementById('map-container').appendChild(tokenDiv);
    if (typeof window.bindTokenEvents === 'function') window.bindTokenEvents(tokenDiv);
});

const chatInput = document.querySelector('#content-chat input');
const sendBtn = document.querySelector('#content-chat button');
const chatBox = document.querySelector('.chat-messages');

window.connection.on("ReceiveGameMessage", (username, message, time) => {
    const isMe = username === myUsername;
    const isDice = message.indexOf("[DICE]") === 0;
    const isSystem = message.indexOf("[SYSTEM]") === 0;
    const cleanText = message.replace("[DICE]", "").replace("[SYSTEM]", "");

    const msgWrapper = document.createElement('div');
    msgWrapper.className = "d-flex w-100 mb-2";

    if (isSystem) {
        msgWrapper.style.justifyContent = "center";
        msgWrapper.innerHTML = `<div class="w-100 text-center py-2 px-3 rounded shadow-sm" style="background-color: #242424; border: 1px solid #444; max-width: 95%;"><div class="text-muted mb-1" style="font-size: 0.65rem;">${time}</div><div class="fst-italic text-light opacity-75" style="font-size: 0.85rem;">${cleanText}</div></div>`;
    } else if (isDice) {
        msgWrapper.style.justifyContent = "center";
        msgWrapper.innerHTML = `<div class="w-100 border border-warning text-center p-2 rounded-3 bg-dark text-warning" style="max-width: 90%; font-size: 0.9rem; box-shadow: 0 0 10px rgba(255,193,7,0.3); animation: pulse 1s ease-out;"><div class="text-muted mb-1" style="font-size: 0.7rem;">${time} - <strong style="color: #ffc107;">${isMe ? "You" : username}</strong> rolled:</div><div class="fw-bold fs-5">${cleanText}</div></div>`;
    } else {
        msgWrapper.style.justifyContent = isMe ? "flex-end" : "flex-start";
        const msgBubble = document.createElement('div');
        msgBubble.style.maxWidth = "80%"; msgBubble.style.padding = "8px 12px"; msgBubble.style.borderRadius = "10px"; msgBubble.style.wordWrap = "break-word";
        if (isMe) {
            msgBubble.style.backgroundColor = "#0d6efd"; msgBubble.style.color = "white";
            msgBubble.innerHTML = `<div class="d-flex justify-content-between align-items-baseline mb-1"><strong class="small me-3">Me</strong> <span class="text-light opacity-75" style="font-size: 0.65rem;">${time}</span></div><div>${cleanText}</div>`;
        } else {
            msgBubble.style.backgroundColor = "#333333"; msgBubble.style.color = "white";
            msgBubble.innerHTML = `<div class="d-flex justify-content-between align-items-baseline mb-1"><strong class="text-warning small me-3">${username}</strong> <span class="text-muted" style="font-size: 0.65rem;">${time}</span></div><div>${cleanText}</div>`;
        }
        msgWrapper.appendChild(msgBubble);
    }
    chatBox.appendChild(msgWrapper); chatBox.scrollTop = chatBox.scrollHeight;
});

function sendMessage() {
    if (chatInput.value.trim() !== "") {
        window.connection.invoke("SendMessageToGame", gameId, myUsername, chatInput.value).catch(console.error);
        chatInput.value = "";
    }
}
sendBtn?.addEventListener("click", sendMessage);
chatInput?.addEventListener("keypress", (e) => { if (e.key === 'Enter') sendMessage(); });

window.connection.start().then(() => {
    window.connection.invoke("JoinGameSession", gameId, myUsername);
    const myAvatar = document.getElementById("player-" + currentUserId);
    if (myAvatar) myAvatar.classList.add("online");
}).catch(console.error);

document.getElementById("exit-vtt")?.addEventListener("click", () => { window.connection.invoke("LeaveGameSession", gameId); });

window.connection.on("MapChanged", (mapUrl) => {
    if (typeof window.setMapBackground === 'function') window.setMapBackground(mapUrl);
    const systemMsg = mapUrl ? "🗺️ The Dungeon Master has changed the battlemap!" : "🗺️ The battlemap has been cleared.";
    const timeNow = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    const msgWrapper = document.createElement('div'); msgWrapper.className = "d-flex w-100 mb-2 justify-content-center";
    msgWrapper.innerHTML = `<div class="w-100 text-center py-2 px-3 rounded shadow-sm" style="background-color: #242424; border: 1px solid #444; max-width: 95%;"><div class="text-muted mb-1" style="font-size: 0.65rem;">${timeNow}</div><div class="fst-italic text-light opacity-75" style="font-size: 0.85rem;">${systemMsg}</div></div>`;
    chatBox.appendChild(msgWrapper); chatBox.scrollTop = chatBox.scrollHeight;
});

window.connection.on("TokenHpUpdated", (tokenId, currentHp, maxHp) => {
    const token = document.getElementById(`token-${tokenId}`);
    if (token) {
        token.setAttribute('data-current-hp', currentHp); token.setAttribute('data-max-hp', maxHp);
        let hpPercent = maxHp > 0 ? (currentHp * 100) / maxHp : 0;
        let fillBar = token.querySelector('.token-hp-fill');
        if (fillBar) { fillBar.style.width = `${hpPercent}%`; fillBar.style.backgroundColor = hpPercent > 50 ? "#28a745" : (hpPercent > 20 ? "#ffc107" : "#dc3545"); }
    }
});

window.connection.on("TokenDeleted", (tokenId) => {
    const tokenElement = document.getElementById(`token-${tokenId}`);
    if (tokenElement) {
        tokenElement.style.transition = "transform 0.3s, opacity 0.3s"; tokenElement.style.transform = "scale(0)"; tokenElement.style.opacity = "0";
        setTimeout(() => { tokenElement.remove(); }, 300);
    }
    if (window.activeToken && window.activeToken.getAttribute('data-token-id') == tokenId) window.activeToken = null;
});

document.querySelectorAll('.dice-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        const sides = parseInt(this.getAttribute('data-sides'));
        const result = Math.floor(Math.random() * sides) + 1;
        let diceText = `[DICE] 🎲 D${sides} result: ${result}`;
        if (sides === 20) { if (result === 20) diceText += "(CRITICAL SUCCESS!)"; else if (result === 1) diceText += "(CRITICAL FAILURE!)"; }
        window.connection.invoke("SendMessageToGame", gameId, myUsername, diceText).catch(console.error);
    });
});

window.connection.on("InitiativeUpdated", (jsonStr) => {
    if (typeof window.updateInitiativeData === 'function') window.updateInitiativeData(jsonStr);
});

window.connection.on("ActiveTurnChanged", (tokenId) => {
    if (typeof window.updateActiveTurn === 'function') window.updateActiveTurn(tokenId);
});