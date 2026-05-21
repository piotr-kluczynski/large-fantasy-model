
window.getUserColor = function (username) {
    if (!username) return "0d6efd";
    const colors = ["0d6efd", "198754", "dc3545", "fd7e14", "e83e8c", "6f42c1", "20c997", "0dcaf0"];
    let hash = 0;
    for (let i = 0; i < username.length; i++) hash += username.charCodeAt(i);
    return colors[hash % colors.length];
};

const globalHubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/privateMessageHub")
    .build();

window.updateAllCounts = function () {
    fetch('/Profile/GetNotificationCounts')
        .then(response => response.json())
        .then(data => {
            const friendBadge = document.getElementById('friend-requests-badge');
            if (friendBadge) {
                if (data.friendRequestsCount > 0) {
                    friendBadge.innerText = data.friendRequestsCount;
                    friendBadge.style.display = 'inline-block';
                } else {
                    friendBadge.style.display = 'none';
                }
            }

            const msgBadge = document.getElementById('unread-messages-badge');
            if (msgBadge) {
                if (data.unreadMessagesCount > 0) {
                    msgBadge.innerText = data.unreadMessagesCount;
                    msgBadge.style.display = 'inline-block';
                } else {
                    msgBadge.style.display = 'none';
                }
            }
        })
        .catch(err => console.error("Błąd aktualizacji liczników:", err));
};

globalHubConnection.on("UpdateNotifications", function () {
    fetch('/Game/GetNotificationBell')
        .then(response => response.text())
        .then(html => {
            const bellContainer = document.getElementById('notification-bell-container');
            if (bellContainer) bellContainer.innerHTML = html;
        })
        .catch(err => console.error("Błąd ładowania dzwoneczka:", err));

    window.updateAllCounts();
});

globalHubConnection.on("ReceiveMessage", function (conversationId, content, timeString, senderId) {
    window.updateAllCounts();
});

globalHubConnection.on("FriendRequestDeclined", function (declinerId) {
    console.log("🔥 SignalR: Odrzucono zaproszenie od ID:", declinerId);
    const sentRequestRow = document.getElementById(`sent-request-${declinerId}`);

    if (sentRequestRow) {
        const parentList = document.getElementById("sent-requests-list");
        sentRequestRow.remove();
        console.log("✅ Usunięto wiersz zaproszenia z listy.");

        if (parentList) {
            const remaining = parentList.querySelectorAll('li').length;
            if (remaining === 0) {
                const section = document.getElementById("sent-requests-section");
                if (section) section.classList.add("d-none");
            }
        }
    }
});

globalHubConnection.start().then(() => {
    if (window.globalUserId) {
        globalHubConnection.invoke("JoinMyUserGroup", window.globalUserId);
    }
}).catch(err => console.error(err));

document.addEventListener('click', function (e) {
    const bellToggle = e.target.closest('#notificationsDropdown') || e.target.closest('#notification-bell-container');
    if (bellToggle) {
        const badge = bellToggle.querySelector('.badge');
        if (badge) badge.style.display = 'none';

        fetch('/Profile/MarkNotificationsAsRead', { method: 'POST' })
            .catch(err => console.error("Błąd oznaczania powiadomień:", err));
    }
});