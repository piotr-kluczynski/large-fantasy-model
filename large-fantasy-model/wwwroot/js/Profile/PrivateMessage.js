const pmConfigEl = document.getElementById('pm-config');
window.myUserId = parseInt(pmConfigEl.getAttribute('data-my-user-id'));
window.activeConversationId = pmConfigEl.getAttribute('data-active-conversation-id');

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/privateMessageHub")
    .build();

connection.on("ReceiveMessage", function (convId, content, time, senderId) {
    const isMine = (senderId === window.myUserId);

    if (convId.toString() === window.activeConversationId) {
        const chatBox = document.getElementById("chat-messages-box");
        if (chatBox) {
            const messageHtml = `
                <div class="mb-3 d-flex ${isMine ? 'justify-content-end' : 'justify-content-start'}">
                    <div class="px-3 py-2 rounded-4 shadow-sm ${isMine ? 'bg-primary text-white' : 'bg-white text-dark'}" style="max-width: 75%;">
                        <div>${content}</div>
                        <small class="${isMine ? 'text-light' : 'text-muted'}" style="font-size: 0.7rem;">${time}</small>
                    </div>
                </div>`;
            chatBox.innerHTML += messageHtml; 
            chatBox.scrollTop = chatBox.scrollHeight;
        }

        if (!isMine) {
            fetch(`/PrivateMessage/MarkConversationAsRead?conversationId=${convId}`, { method: 'POST' })
                .then(() => { if (typeof updateAllCounts === 'function') updateAllCounts(); })
                .catch(err => console.error("Błąd auto-odczytu:", err));
        }
    }
    else if (!isMine) {
        const friendBtn = document.getElementById("friend-btn-" + senderId);
        const badge = document.getElementById("unread-badge-friend-" + senderId);

        if (friendBtn && badge) {
            friendBtn.classList.add("border-warning", "fw-bold", "unread-pop");
            friendBtn.classList.remove("border-primary");
            badge.classList.remove("d-none");
            let currentCount = parseInt(badge.innerText) || 0;
            badge.innerText = currentCount + 1;
        }
    }
});

connection.on("UserAvatarChanged", function (username, newAvatarPath) {
    if (window.updateUserAvatars) {
        window.updateUserAvatars(username, newAvatarPath);
    }
});

connection.start().then(function () {
    connection.invoke("JoinMyUserGroup", window.myUserId.toString());
    const chatBox = document.getElementById("chat-messages-box");
    if (chatBox) chatBox.scrollTop = chatBox.scrollHeight;
}).catch(function (err) {
    console.error(err.toString());
});

document.getElementById('friend-search')?.addEventListener('input', function () {
    const searchTerm = this.value.toLowerCase();
    document.querySelectorAll('.friend-item').forEach(item => {
        const name = item.getAttribute('data-name');
        item.style.display = name.includes(searchTerm) ? 'block' : 'none';
    });
});

const chatForm = document.getElementById("chat-form");
if (chatForm) {
    chatForm.addEventListener("submit", function (e) {
        e.preventDefault();
        const input = document.getElementById("chat-input");
        const content = input.value;
        if (!content.trim()) return;

        const formData = new FormData(chatForm);
        fetch(chatForm.action, { method: 'POST', body: formData })
            .then(response => { if (response.ok) input.value = ""; })
            .catch(error => console.error('Error:', error));
    });
}

document.addEventListener('DOMContentLoaded', function () {
    const chatInput = document.getElementById("chat-input");
    if (chatInput) {
        chatInput.addEventListener('focus', function () {
            const convIdInput = document.querySelector('input[name="conversationId"]');
            if (convIdInput) {
                const convId = convIdInput.value;
                fetch(`/PrivateMessage/MarkConversationAsRead?conversationId=${convId}`, { method: 'POST' })
                    .then(response => {
                        if (response.ok) {
                            if (typeof updateAllCounts === 'function') updateAllCounts();
                            const activeBtns = document.querySelectorAll('.friend-item button');
                            activeBtns.forEach(btn => {
                                if (btn.classList.contains('border-primary')) {
                                    const badge = btn.querySelector('.badge');
                                    if (badge) badge.classList.add('d-none');
                                }
                            });
                        }
                    })
                    .catch(err => console.error("Błąd oznaczania jako przeczytane:", err));
            }
        });
    }
});