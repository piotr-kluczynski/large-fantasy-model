document.addEventListener("DOMContentLoaded", function () {
    const isPublicSwitch = document.getElementById('isPublicSwitch');
    const passwordField = document.getElementById('passwordField');


    function togglePassword() {
        if (isPublicSwitch.checked) {
            passwordField.classList.add('d-none');
        } else {
            passwordField.classList.remove('d-none');
        }
    }


    togglePassword();


    isPublicSwitch.addEventListener('change', togglePassword);
});