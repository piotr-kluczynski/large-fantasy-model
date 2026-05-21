document.addEventListener("DOMContentLoaded", function () {
    const isPublicSwitch = document.getElementById('isPublicSwitch');
    const passwordField = document.getElementById('passwordField');

    if (!isPublicSwitch || !passwordField) {
        console.error("Nie znaleziono przełącznika lub pola hasła!");
        return;
    }

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