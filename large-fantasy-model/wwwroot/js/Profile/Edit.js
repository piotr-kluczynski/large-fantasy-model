document.addEventListener("DOMContentLoaded", function () {
    const profilePicInput = document.getElementById('ProfilePicture');
    if (profilePicInput) {
        profilePicInput.addEventListener('change', function (event) {
            const file = event.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const preview = document.getElementById('avatarPreview');
                    if (preview) preview.src = e.target.result;
                }
                reader.readAsDataURL(file);
            }
        });
    }

    const removeForm = document.getElementById('removeAvatarForm');
    if (removeForm) {
        removeForm.addEventListener('submit', function (e) {
            e.preventDefault();
            
            fetch(this.action, {
                method: this.method,
                body: new FormData(this)
            }).then(response => {
                if (response.ok) {
                    const preview = document.getElementById('avatarPreview');
                    const username = preview ? preview.getAttribute('data-username') : '';
                    
                    let fallbackUrl = "";
                    if (username) {
                        let hash = 0;
                        for (let i = 0; i < username.length; i++) hash += username.charCodeAt(i);
                        const colors = ["0d6efd", "198754", "dc3545", "fd7e14", "e83e8c", "6f42c1", "20c997", "0dcaf0"];
                        let color = colors[hash % colors.length];
                        fallbackUrl = `https://ui-avatars.com/api/?name=${username}&size=120&background=${color}&color=fff&length=2`;
                    }
                    
                    if (preview) preview.src = fallbackUrl;
                    
                    let removeBtn = document.getElementById('removeAvatarBtnContainer');
                    if (removeBtn) {
                        removeBtn.style.display = 'none';
                    }

                    if (profilePicInput) profilePicInput.value = '';

                    if (window.updateUserAvatars && username) {
                        window.updateUserAvatars(username, fallbackUrl);
                    }
                }
            }).catch(error => console.error("Error removing avatar:", error));
        });
    }
});
