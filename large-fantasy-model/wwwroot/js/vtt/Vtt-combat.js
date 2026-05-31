let initiativeData = [];
let activeTurnTokenId = null;
const combatListEl = document.getElementById('initiative-list');

window.renderInitiativeList = function () {
    if (!combatListEl) return;
    combatListEl.innerHTML = '';
    if (initiativeData.length === 0) {
        combatListEl.innerHTML = '<p class="text-muted small text-center mt-4">No active combat. DM can load tokens.</p>';
        return;
    }

    initiativeData.forEach((item) => {
        const isMyTurn = item.tokenId === activeTurnTokenId;
        const borderClass = isMyTurn ? 'border-warning shadow' : 'border-secondary';
        const bgClass = isMyTurn ? 'bg-dark' : 'bg-transparent';
        const textClass = isMyTurn ? 'text-warning fw-bold' : 'text-light';
        const inputDisabled = isDM ? '' : 'disabled';

        const deleteBtn = isDM ? `<button class="btn btn-sm btn-outline-danger ms-2 remove-init-btn" data-token-id="${item.tokenId}" title="Remove from combat"><i class="bi bi-x-lg"></i></button>` : '';

        const row = document.createElement('div');
        row.className = `d-flex align-items-center justify-content-between p-2 mb-2 border rounded ${borderClass} ${bgClass}`;
        row.innerHTML = `
            <div class="${textClass} text-truncate" style="max-width: 130px;"><i class="bi ${isMyTurn ? 'bi-caret-right-fill' : 'bi-person'}"></i> ${item.name}</div>
            <div class="d-flex align-items-center">
                <span class="small text-muted me-2">Init:</span>
                <input type="number" class="form-control form-control-sm text-center bg-secondary text-white init-val" style="width: 50px;" value="${item.value}" data-token-id="${item.tokenId}" ${inputDisabled}>
                ${deleteBtn}
            </div>
        `;
        combatListEl.appendChild(row);
    });

    if (isDM) {
        document.querySelectorAll('.init-val').forEach(input => {
            input.addEventListener('change', (e) => {
                const tId = parseInt(e.target.getAttribute('data-token-id'));
                const item = initiativeData.find(i => i.tokenId === tId);
                if (item) item.value = parseInt(e.target.value) || 0;
                window.connection.invoke("UpdateInitiative", gameId, JSON.stringify(initiativeData)).catch(console.error);
            });
        });

        document.querySelectorAll('.remove-init-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const tId = parseInt(e.currentTarget.getAttribute('data-token-id'));
                initiativeData = initiativeData.filter(i => i.tokenId !== tId);
                if (tId === activeTurnTokenId) {
                    activeTurnTokenId = null;
                    window.connection.invoke("SetActiveTurn", gameId, null).catch(console.error);
                }
                window.connection.invoke("UpdateInitiative", gameId, JSON.stringify(initiativeData)).catch(console.error);
            });
        });
    }
};

window.updateInitiativeData = function (jsonStr) {
    initiativeData = JSON.parse(jsonStr);
    window.renderInitiativeList();
};

window.updateActiveTurn = function (tokenId) {
    activeTurnTokenId = tokenId;
    window.renderInitiativeList();

    document.querySelectorAll('.game-token').forEach(t => {
        if (parseInt(t.getAttribute('data-token-id')) === tokenId) t.classList.add('active-turn');
        else t.classList.remove('active-turn');
    });

    const activeItem = initiativeData.find(i => i.tokenId === tokenId);
    if (activeItem) {
        const msgWrapper = document.createElement('div');
        msgWrapper.className = "d-flex w-100 mb-2 justify-content-center";
        msgWrapper.innerHTML = `<div class="w-100 text-center py-2 px-3 rounded shadow-sm border border-warning" style="background-color: #332b00; max-width: 95%;"><div class="fst-italic text-warning fw-bold" style="font-size: 0.85rem;">⚔️ It is now ${activeItem.name}'s turn!</div></div>`;
        const chatBox = document.querySelector('.chat-messages');
        chatBox.appendChild(msgWrapper); chatBox.scrollTop = chatBox.scrollHeight;
    }
};

if (isDM) {
    document.getElementById('btn-refresh-combat')?.addEventListener('click', () => {
        initiativeData = [];
        document.querySelectorAll('.game-token').forEach(t => {
            initiativeData.push({ tokenId: parseInt(t.getAttribute('data-token-id')), name: t.getAttribute('data-token-name') || "Unknown", value: 0 });
        });
        window.connection.invoke("UpdateInitiative", gameId, JSON.stringify(initiativeData)).catch(console.error);
    });

    document.getElementById('btn-sort-combat')?.addEventListener('click', () => {
        initiativeData.sort((a, b) => b.value - a.value);
        window.connection.invoke("UpdateInitiative", gameId, JSON.stringify(initiativeData)).catch(console.error);
    });

    document.getElementById('btn-next-turn')?.addEventListener('click', () => {
        if (initiativeData.length === 0) return;
        let currentIndex = initiativeData.findIndex(i => i.tokenId === activeTurnTokenId);
        let nextIndex = (currentIndex + 1 >= initiativeData.length) ? 0 : currentIndex + 1;
        activeTurnTokenId = initiativeData[nextIndex].tokenId;
        window.connection.invoke("SetActiveTurn", gameId, activeTurnTokenId).catch(console.error);
    });

    document.querySelectorAll('.autoroll-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            if (initiativeData.length === 0) { alert("Please load tokens first!"); return; }
            const sides = parseInt(this.getAttribute('data-sides'));
            let rolledCount = 0;
            initiativeData.forEach(item => {
                const tokenEl = document.getElementById(`token-${item.tokenId}`);
                if (tokenEl && tokenEl.getAttribute('data-owner-id') === currentUserId && tokenEl.getAttribute('data-token-name') !== myUsername) {
                    item.value = Math.floor(Math.random() * sides) + 1;
                    rolledCount++;
                }
            });
            if (rolledCount > 0) {
                initiativeData.sort((a, b) => b.value - a.value);
                window.connection.invoke("UpdateInitiative", gameId, JSON.stringify(initiativeData)).catch(console.error);
                window.connection.invoke("SendMessageToGame", gameId, "System", `[SYSTEM] 🎲 The Dungeon Master auto-rolled D${sides} initiative for all (${rolledCount}) monsters!`).catch(console.error);
            } else { alert("No monsters found on the initiative list to roll for."); }
        });
    });

    document.getElementById('btn-clear-combat')?.addEventListener('click', () => {
        if (confirm("Are you sure you want to clear the entire initiative tracker?")) {
            initiativeData = []; activeTurnTokenId = null;
            window.connection.invoke("UpdateInitiative", gameId, JSON.stringify([])).catch(console.error);
            window.connection.invoke("SetActiveTurn", gameId, null).catch(console.error);
            window.connection.invoke("SendMessageToGame", gameId, "System", "[SYSTEM] 🕊️ Combat has ended. The initiative tracker has been cleared.").catch(console.error);
        }
    });
}