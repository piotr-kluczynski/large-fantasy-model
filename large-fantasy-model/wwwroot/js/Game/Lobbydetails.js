const lobbyConfigEl = document.getElementById('lobby-config');
const gameId = parseInt(lobbyConfigEl.getAttribute('data-game-id'));
const isDM = lobbyConfigEl.getAttribute('data-is-dm') === 'true';
const currentUserId = lobbyConfigEl.getAttribute('data-current-user-id');

const lobbyConnection = new signalR.HubConnectionBuilder()
    .withUrl("/lobbyHub")
    .build();

lobbyConnection.on("PlayerJoinedLobby", function (playerId, username) {
    const friendRow = document.getElementById(`friend-row-${playerId}`);
    if (friendRow) {
        friendRow.remove();
        const inviteList = document.getElementById("friends-invite-list");
        if (inviteList && inviteList.querySelectorAll('li').length === 0) {
            const card = document.getElementById("invite-friends-card");
            if (card) card.classList.add("d-none");
        }
    }

    const emptyMsg = document.getElementById("empty-players-msg");
    if (emptyMsg) emptyMsg.remove();

    const playersList = document.getElementById("players-list");
    if (playersList && !document.getElementById(`player-row-${playerId}`)) {
        const countElem = document.getElementById("player-count");
        if (countElem) countElem.innerText = parseInt(countElem.innerText) + 1;

        let kickButton = isDM ? `
            <form action="/Game/RemovePlayer" method="post" class="kick-form m-0">
                <input type="hidden" name="gameId" value="${gameId}" />
                <input type="hidden" name="playerId" value="${playerId}" />
                <button type="submit" class="btn btn-sm border-0 bg-transparent shadow-none p-0 ms-2 btn-kick">
                    <i class="bi bi-x"></i>
                </button>
            </form>` : "";

        let avatarSrc = profilePicturePath ? profilePicturePath : `https://ui-avatars.com/api/?name=${username}&size=45&background=${getUserColor(username)}&color=fff&length=2`;

        const html = `
            <li id="player-row-${playerId}" class="list-group-item p-3 border-top d-flex align-items-center justify-content-between border-0">
                <div class="d-flex align-items-center">
                    <img src="${avatarSrc}" class="rounded-circle me-3 border border-success border-2" style="width: 45px; height: 45px; object-fit: cover;" />
                    <div>
                        <div class="fw-bold text-dark">${username}</div>
                        <small class="text-success fw-bold">Player</small>
                    </div>
                </div>
                ${kickButton}
            </li>`;
        playersList.insertAdjacentHTML('beforeend', html);
    }
});

lobbyConnection.on("PlayerLeftLobby", function (playerId, username, profilePicturePath) {
    if (playerId.toString() === currentUserId) {
        window.location.href = '/Game/Campaigns'; 
        return;
    }
    const row = document.getElementById(`player-row-${playerId}`);
    if (row) {
        row.remove();
        const countElem = document.getElementById("player-count");
        if (countElem) countElem.innerText = parseInt(countElem.innerText) - 1;

        const playersList = document.getElementById("players-list");
        if (playersList && playersList.querySelectorAll('li').length === 0) {
            playersList.insertAdjacentHTML('beforeend', '<li id="empty-players-msg" class="list-group-item text-center py-4 text-muted small italic">No travelers have joined yet.</li>');
        }
    }

    if (username) {
        const inviteCard = document.getElementById("invite-friends-card");
        if (inviteCard) inviteCard.classList.remove("d-none");
        const inviteList = document.getElementById("friends-invite-list");

        if (inviteList && !document.getElementById(`friend-row-${playerId}`)) {
            let avatarSrc = profilePicturePath ? profilePicturePath : `https://ui-avatars.com/api/?name=${username}&size=32&background=${getUserColor(username)}&color=fff&length=2`;
            const friendHtml = `
                <li id="friend-row-${playerId}" class="list-group-item d-flex justify-content-between align-items-center py-3">
                    <div class="d-flex align-items-center">
                        <img src="${avatarSrc}" class="rounded-circle me-2" style="width: 32px; height: 32px; object-fit: cover;" />
                        <span class="fw-semibold">${username}</span>
                    </div>
                    <form action="/Game/InviteFriend" method="post" class="invite-form m-0">
                        <input type="hidden" name="__RequestVerificationToken" value="${window.getAntiForgeryToken()}" />
                        <input type="hidden" name="gameId" value="${gameId}" />
                        <input type="hidden" name="friendId" value="${playerId}" />
                        <button type="submit" class="btn btn-outline-success btn-sm rounded-pill fw-bold">Invite</button>
                    </form>
                </li>`;
            inviteList.insertAdjacentHTML('beforeend', friendHtml);
        }
    }
});

lobbyConnection.on("GameInviteDeclined", function (declinerId) {
    console.log("Gracz odrzucił zaproszenie do gry:", declinerId);
    const friendRow = document.getElementById(`friend-row-${declinerId}`);
    if (friendRow) {
        const btn = friendRow.querySelector('button');
        if (btn) {
            btn.disabled = false;
            btn.className = 'btn btn-outline-success btn-sm rounded-pill fw-bold';
            btn.innerText = 'Invite';
        }
    }
});

lobbyConnection.on("PlayerSelectedCharacter", function (playerId, characterName) {
    const playerRow = document.getElementById(`player-row-${playerId}`);
    if (playerRow) {
        const nameDiv = playerRow.querySelector('.fw-bold.text-dark');
        if (nameDiv) {
            let span = nameDiv.querySelector('span.text-muted');
            if (!span) {
                span = document.createElement('span');
                span.className = 'text-muted fw-normal ms-1';
                nameDiv.appendChild(span);
            }
            span.innerText = `(${characterName})`;
        }
    }
});

lobbyConnection.on("PlayerClearedCharacter", function (playerId) {
    const playerRow = document.getElementById(`player-row-${playerId}`);
    if (playerRow) {
        const span = playerRow.querySelector('.fw-bold.text-dark span.text-muted');
        if (span) {
            span.remove();
        }
    }
});

lobbyConnection.start().then(() => {
    lobbyConnection.invoke("JoinLobbyGroup", gameId);
});


const characterSelect = document.getElementById('character-select');
if (characterSelect) {
    characterSelect.addEventListener('change', async function () {
        const form = document.getElementById('select-character-form');
        const formData = new FormData(form);

        try {
            const res = await fetch(form.action, {
                method: 'POST',
                body: formData,
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });

            if (res.ok) {
                const data = await res.json();
                const enterVttBtn = document.getElementById('enter-vtt-btn');
                const statusContainer = document.getElementById('character-status-container');

                if (data.hasCharacter) {
                    if (enterVttBtn) enterVttBtn.setAttribute('data-has-character', 'true');
                    if (statusContainer) {
                        statusContainer.innerHTML = '<p class="text-success small fw-bold mb-0"><i class="bi bi-check-circle-fill me-1"></i> Character selected and ready for adventure!</p>';
                    }
                } else {
                    if (enterVttBtn) enterVttBtn.setAttribute('data-has-character', 'false');
                    if (statusContainer) {
                        statusContainer.innerHTML = '<p class="text-danger small mb-0"><i class="bi bi-exclamation-circle-fill me-1"></i> You must select a character to enter the VTT.</p>';
                    }
                }
            }
        } catch (e) {
            console.error('Error selecting character:', e);
        }
    });
}

document.addEventListener('submit', function (e) {
    if (e.target.classList.contains('invite-form')) {
        e.preventDefault();
        const btn = e.target.querySelector('button');
        btn.disabled = true;
        btn.className = 'btn btn-secondary btn-sm rounded-pill fw-bold btn-invited';
        btn.innerText = 'Invited';
        fetch(e.target.action, { method: 'POST', body: new FormData(e.target), headers: { 'RequestVerificationToken': getAfToken() } });
    }
    if (e.target.classList.contains('kick-form')) {
        e.preventDefault();
        if (confirm('Kick this player?')) {
            fetch(e.target.action, { method: 'POST', body: new FormData(e.target), headers: { 'RequestVerificationToken': getAfToken() } });
        }
    }
});

function getUserColor(username) {
    const colors = ["0d6efd", "198754", "dc3545", "fd7e14", "e83e8c", "6f42c1", "20c997", "0dcaf0"];
    let hash = 0;
    for (let i = 0; i < username.length; i++) hash += username.charCodeAt(i);
    return colors[hash % colors.length];
};

lobbyConnection.on("UserAvatarChanged", function (username, newAvatarPath) {
    if (window.updateUserAvatars) {
        window.updateUserAvatars(username, newAvatarPath);
    }
});
document.addEventListener('DOMContentLoaded', function () {
    const btnToggleAi = document.getElementById('btn-toggle-ai-lore');
    const aiPanel = document.getElementById('ai-lore-panel');
    const btnGenerateLore = document.getElementById('btn-generate-ai-lore');

    if (btnToggleAi && aiPanel) {
        btnToggleAi.addEventListener('click', (e) => {
            e.preventDefault();

            if (aiPanel.style.display === 'none' || aiPanel.style.display === '') {
                aiPanel.style.display = 'block';
                btnToggleAi.classList.replace('btn-outline-success', 'btn-success');
                btnToggleAi.classList.add('text-white');
            } else {
                aiPanel.style.display = 'none';
                btnToggleAi.classList.replace('btn-success', 'btn-outline-success');
                btnToggleAi.classList.remove('text-white');
            }
        });
    }

    if (btnGenerateLore) {
        btnGenerateLore.addEventListener('click', async (e) => {
            e.preventDefault();

            const promptInput = document.getElementById('ai-lore-prompt');
            const includeCurrent = document.getElementById('ai-include-current').checked;
            const loreTextarea = document.getElementById('lore-textarea');
            const statusDiv = document.getElementById('ai-lore-status');

            let promptText = promptInput.value.trim();
            let currentLore = loreTextarea.value.trim();

            if (!promptText && (!includeCurrent || !currentLore)) {
                alert("Please write a prompt for the AI or ensure you have text in the main editor!");
                return;
            }

            if (includeCurrent && currentLore) {
                promptText += "\n\n--- CURRENT TEXT TO MODIFY/INCLUDE ---\n" + currentLore;
            }

            statusDiv.style.display = 'block';
            statusDiv.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Scribing lore...';
            btnGenerateLore.disabled = true;

            try {
                const payload = {
                    prompt: promptText,
                    gameId: gameId,
                    isInGameAssistant: false
                };

                const res = await fetch('/api/Ai/generate', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload)
                });

                if (!res.ok) {
                    const errText = await res.text();
                    statusDiv.innerHTML = `<span class="text-danger">❌ Error: ${errText}</span>`;
                    return;
                }

                const data = await res.json();

                if (includeCurrent && currentLore) {
                    loreTextarea.value = data.response;
                } else {
                    if (currentLore !== "") {
                        loreTextarea.value += "\n\n" + data.response;
                    } else {
                        loreTextarea.value = data.response;
                    }
                }

                statusDiv.innerHTML = '<span class="text-success"><i class="bi bi-check-circle-fill me-1"></i>Done!</span>';
                setTimeout(() => { statusDiv.style.display = 'none'; }, 2000);
                promptInput.value = '';

            } catch (error) {
                console.error(error);
                statusDiv.innerHTML = `<span class="text-danger">❌ Connection error: ${error.message}</span>`;
            } finally {
                btnGenerateLore.disabled = false;
            }
        });
    }
});

document.addEventListener('DOMContentLoaded', function () {
    const rawLore = document.getElementById('raw-lore');
    const renderedLore = document.getElementById('rendered-lore');
    if (rawLore && renderedLore) {
        renderedLore.innerHTML = marked.parse(rawLore.value);
    }

    const dmRawLore = document.getElementById('dm-raw-lore');
    const dmRenderedLore = document.getElementById('dm-rendered-lore');
    if (dmRawLore && dmRenderedLore) {
        dmRenderedLore.innerHTML = marked.parse(dmRawLore.value);
    }

    const btnEditLore = document.getElementById('btn-edit-lore');
    const btnCancelEdit = document.getElementById('btn-cancel-edit');
    const dmViewMode = document.getElementById('dm-view-mode');
    const loreForm = document.getElementById('lore-form');

    if (btnEditLore && dmViewMode && loreForm) {
        btnEditLore.addEventListener('click', function() {
            dmViewMode.style.display = 'none';
            loreForm.style.display = 'block';
        });
    }

    if (btnCancelEdit && dmViewMode && loreForm) {
        btnCancelEdit.addEventListener('click', function() {
            dmViewMode.style.display = 'block';
            loreForm.style.display = 'none';
        });
    }


    const mainTab = document.getElementById('main-tab');
    const loreTab = document.getElementById('lore-tab');
    
    if (mainTab) {
        mainTab.addEventListener('shown.bs.tab', function () {
            const url = new URL(window.location);
            url.searchParams.delete('tab');
            window.history.replaceState({}, '', url);
        });
    }
    
    if (loreTab) {
        loreTab.addEventListener('shown.bs.tab', function () {
            const url = new URL(window.location);
            url.searchParams.set('tab', 'lore');
            window.history.replaceState({}, '', url);
        });
    }

    const enterVttBtn = document.getElementById('enter-vtt-btn');
    if (enterVttBtn) {
        enterVttBtn.addEventListener('click', function(e) {
            const isDM = this.getAttribute('data-is-dm') === 'true';
            const hasCharacter = this.getAttribute('data-has-character') === 'true';

            if (!isDM && !hasCharacter) {
                e.preventDefault();
                if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        title: 'Hold on, adventurer!',
                        text: 'You must select a character from the panel on the right before entering the virtual tabletop.',
                        icon: 'warning',
                        confirmButtonText: 'I will do that',
                        confirmButtonColor: '#198754'
                    });
                } else {
                    alert('Hold on, adventurer! You must select a character before entering the virtual tabletop.');
                }
            } else {
                document.getElementById('enter-vtt-form').submit();
            }
        });
    }
});
