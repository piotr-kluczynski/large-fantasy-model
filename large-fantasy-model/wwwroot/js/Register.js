document.addEventListener('DOMContentLoaded', function () {
    const passwordInput = document.getElementById('passwordInput');
    const checklistContainer = document.getElementById('passwordChecklist');

    if (!passwordInput || !checklistContainer) return;

    const requirements = {
        length: document.getElementById('check-length'),
        upper: document.getElementById('check-upper'),
        lower: document.getElementById('check-lower'),
        number: document.getElementById('check-number')
    };

    function updateRequirement(element, isValid) {
        if (isValid) {
            element.classList.remove('text-danger');
            element.classList.add('text-success');
        } else {
            element.classList.remove('text-success');
            element.classList.add('text-danger');
        }
    }

    function checkPasswordStrength() {
        const val = passwordInput.value;

        if (val.length > 0) {
            checklistContainer.classList.remove('d-none');
        } else {
            checklistContainer.classList.add('d-none');
        }

        updateRequirement(requirements.length, val.length >= 8);
        updateRequirement(requirements.upper, /[A-Z]/.test(val));
        updateRequirement(requirements.lower, /[a-z]/.test(val));
        updateRequirement(requirements.number, /\d/.test(val));
    }

    passwordInput.addEventListener('input', checkPasswordStrength);
});