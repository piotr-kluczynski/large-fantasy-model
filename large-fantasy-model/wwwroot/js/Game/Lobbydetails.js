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

        const html = `
            <li id="player-row-${playerId}" class="list-group-item p-3 border-top d-flex align-items-center justify-content-between border-0">
                <div class="d-flex align-items-center">
                    <img src="https://ui-avatars.com/api/?name=${username}&size=45&background=${getUserColor(username)}&color=fff&length=2" class="rounded-circle me-3 border border-success border-2" />
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

lobbyConnection.on("PlayerLeftLobby", function (playerId, username) {
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
        if (playersList && playersList.querySelectorAll('li').length === 1) {
            playersList.insertAdjacentHTML('beforeend', '<li id="empty-players-msg" class="list-group-item text-center py-4 text-muted small italic">No travelers have joined yet.</li>');
        }
    }

    if (username) {
        const inviteCard = document.getElementById("invite-friends-card");
        if (inviteCard) inviteCard.classList.remove("d-none");
        const inviteList = document.getElementById("friends-invite-list");

        if (inviteList && !document.getElementById(`friend-row-${playerId}`)) {
            const friendHtml = `
                <li id="friend-row-${playerId}" class="list-group-item d-flex justify-content-between align-items-center py-3">
                    <div class="d-flex align-items-center">
                        <img src="https://ui-avatars.com/api/?name=${username}&size=32&background=${getUserColor(username)}&color=fff&length=2" class="rounded-circle me-2" />
                        <span class="fw-semibold">${username}</span>
                    </div>
                    <form action="/Game/InviteFriend" method="post" class="invite-form m-0">
                        <input type="hidden" name="__RequestVerificationToken" value="${getAfToken()}" />
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

lobbyConnection.start().then(() => {
    lobbyConnection.invoke("JoinLobbyGroup", gameId);
});

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
}