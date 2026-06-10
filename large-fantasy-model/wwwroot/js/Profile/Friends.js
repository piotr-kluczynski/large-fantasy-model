const friendsConfigEl = document.getElementById('friends-config');
window.myUserId = parseInt(friendsConfigEl.getAttribute('data-my-user-id'));

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/privateMessageHub")
    .build();


window.getUserColor = function (username) {
    if (!username) return "0d6efd";
    const colors = ["0d6efd", "198754", "dc3545", "fd7e14", "e83e8c", "6f42c1", "20c997", "0dcaf0"];
    let hash = 0;
    for (let i = 0; i < username.length; i++) hash += username.charCodeAt(i);
    return colors[hash % colors.length];
};

connection.on("UserAvatarChanged", function (username, newAvatarPath) {
    if (window.updateUserAvatars) {
        window.updateUserAvatars(username, newAvatarPath);
    }
});

connection.on("ReceiveFriendRequest", function (senderId, senderName, senderAvatar) {
    let list = document.getElementById("friend-requests-list");
    let card = document.getElementById("received-requests-card");
    let color = getUserColor(senderName);

    let avatarSrc = senderAvatar ? senderAvatar : `https://ui-avatars.com/api/?name=${senderName}&size=40&background=${color}&color=fff&length=2`;

    if (list) {
        if (card) card.style.display = "block";

        let newRequestHtml = `
            <li class="list-group-item d-flex justify-content-between align-items-center py-3 bg-light">
                <div class="d-flex align-items-center">
                    <img src="${avatarSrc}" class="rounded-circle me-3" style="width: 40px; height: 40px; object-fit: cover;" />
                    <span class="fw-semibold">${senderName}</span>
                </div>
                <div class="d-flex gap-2">
                    <form action="/Profile/AcceptFriendInvite" method="post">

                    </form>
                    <form action="/Profile/AddFriend" method="post" style="display:inline;">
                        <input type="hidden" name="friendId" value="${senderId}" />
                        <input type="hidden" name="__RequestVerificationToken" value="${window.getAntiForgeryToken()}" />
                        <button type="submit" class="btn btn-success btn-sm rounded-pill fw-bold">Accept</button>
                    </form>
                    <form action="/Profile/RemoveFriend" method="post" style="display:inline;">
                        <input type="hidden" name="friendId" value="${senderId}" />
                        <input type="hidden" name="__RequestVerificationToken" value="${window.getAntiForgeryToken()}" />
                        <button type="submit" class="btn btn-outline-danger btn-sm rounded-pill fw-bold">Decline</button>
                    </form>
                </div>
            </li>`;
        list.insertAdjacentHTML('afterbegin', newRequestHtml);
    }
});

connection.on("ReceiveFriendAccept", function (senderId, senderName, senderAvatar) {
    const friendsList = document.getElementById("mutual-friends-list");
    const noFriendsMsg = document.getElementById("no-friends-msg");

    if (noFriendsMsg) noFriendsMsg.style.display = "none";

    if (friendsList) {
        friendsList.style.display = "block";

        if (!document.querySelector(`#mutual-friends-list input[name="friendId"][value="${senderId}"]`)) {
            const color = window.getUserColor(senderName);
            let avatarSrc = senderAvatar ? senderAvatar : `https://ui-avatars.com/api/?name=${senderName}&size=40&background=${color}&color=fff&length=2`;

            const newFriendHtml = `
                <li class="list-group-item d-flex justify-content-between align-items-center py-3 unread-pop" style="background-color: #f8fff9;">
                    <div class="d-flex align-items-center">
                        <img src="${avatarSrc}" class="rounded-circle me-3 border border-success" style="width: 40px; height: 40px; object-fit: cover;" />
                        <span class="fw-bold fs-5">${senderName}</span>
                    </div>
                    <div class="d-flex gap-2">
                        <form action="/PrivateMessage/StartPrivateChat" method="post">
                            <input type="hidden" name="friendId" value="${senderId}" />
                            <input type="hidden" name="__RequestVerificationToken" value="${window.getAntiForgeryToken()}" />
                            <button type="submit" class="btn btn-primary btn-sm rounded-pill fw-bold px-3">Message</button>
                        </form>
                        <form action="/Profile/RemoveFriend" method="post">
                            <input type="hidden" name="friendId" value="${senderId}" />
                            <input type="hidden" name="__RequestVerificationToken" value="${window.getAntiForgeryToken()}" />
                            <button type="submit" class="btn btn-outline-danger btn-sm rounded-pill fw-bold">Remove</button>
                        </form>
                    </div>
                </li>`;
            friendsList.insertAdjacentHTML('afterbegin', newFriendHtml);
            
        }
    }


    const sentInputs = document.querySelectorAll(`#sent-requests-list form input[name="friendId"][value="${senderId}"]`);
    sentInputs.forEach(input => {
        const liToRemove = input.closest('li');
        if (liToRemove) {
            const ulContainer = liToRemove.closest('ul');
            const cardContainer = liToRemove.closest('.card');

            liToRemove.remove(); 

            console.log("Usunięto znajomego z oczekujących.");


            if (ulContainer && ulContainer.querySelectorAll('li').length === 0 && cardContainer) {
                cardContainer.style.display = "none";
            }
        }
    });
});

connection.on("ReceiveFriendRemove", function (senderId) {
    const removeFormInput = document.querySelector(`#mutual-friends-list form[action="/Profile/RemoveFriend"] input[name="friendId"][value="${senderId}"]`);
    if (removeFormInput) {
        const liToRemove = removeFormInput.closest('li');
        if (liToRemove) liToRemove.remove();

        const friendsList = document.getElementById("mutual-friends-list");
        const noFriendsMsg = document.getElementById("no-friends-msg");
        if (friendsList && friendsList.querySelectorAll('li').length === 0) {
            friendsList.style.display = "none";
            if (noFriendsMsg) noFriendsMsg.style.display = "block";
        }
    }
});

connection.start().then(function () {
    connection.invoke("JoinMyUserGroup", window.myUserId.toString());
});