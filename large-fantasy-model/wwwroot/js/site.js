// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



window.getAntiForgeryToken = function () {
    const input = document.querySelector('input[name="__RequestVerificationToken"]');
    return input ? input.value : '';
};

window.getUserColor = function (username) {
    if (!username) return "0d6efd";
    const colors = ["0d6efd", "198754", "dc3545", "fd7e14", "e83e8c", "6f42c1", "20c997", "0dcaf0"];
    let hash = 0;
    for (let i = 0; i < username.length; i++) hash += username.charCodeAt(i);
    return colors[hash % colors.length];
};

window.updateUserAvatars = function(username, newAvatarPath) {
    if (!newAvatarPath) {
        let hash = 0;
        for (let i = 0; i < username.length; i++) hash += username.charCodeAt(i);
        const colors = ["0d6efd", "198754", "dc3545", "fd7e14", "e83e8c", "6f42c1", "20c997", "0dcaf0"];
        let color = colors[hash % colors.length];
        newAvatarPath = `https://ui-avatars.com/api/?name=${username}&size=120&background=${color}&color=fff&length=2`;
    }


    document.querySelectorAll("span.fw-semibold, span.fw-bold, div.fw-bold, h5.fw-bold, div.text-dark, a.dropdown-toggle").forEach(el => {
        if (el.innerText.trim() === username || el.innerText.trim() === username + " " || el.innerText.trim().includes(username)) {
            let img;
            if (el.tagName.toLowerCase() === 'a') {
                img = el.querySelector("img.rounded-circle");
            } else {
                let container = el.closest(".d-flex") || el.closest("li") || el.closest("a");
                if (container) {
                    img = container.querySelector("img.rounded-circle");
                }
            }
            if (img && img.src !== newAvatarPath) {
                img.src = newAvatarPath;
            }
        }
    });


    document.querySelectorAll(`img[data-username="${username}"]`).forEach(img => {
        if (img.src !== newAvatarPath) {
            img.src = newAvatarPath;
        }
    });

    let navAvatar = document.getElementById("nav-user-avatar");
    if (navAvatar && navAvatar.parentElement.innerText.includes(username)) {
        navAvatar.src = newAvatarPath;
    }

 
    document.querySelectorAll(".player-avatar").forEach(el => {
        if (el.title === username || el.title === "DM: " + username) {
            el.style.backgroundImage = `url('${newAvatarPath}')`;
            el.innerHTML = '<div class="status-indicator"></div>';
        }
    });
};
