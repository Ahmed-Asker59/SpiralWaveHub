const PassBtn = document.querySelector('#passBtn');
PassBtn.addEventListener('click', () => {
    const input = document.querySelector('#passInput');
    input.getAttribute('type') === 'password' ? input.setAttribute('type', 'text') : input.setAttribute('type', 'password');

    if (input.getAttribute('type') === 'text') {
        PassBtn.innerHTML = '<i class="fa fa-eye-slash"></i>';
    } else {
        PassBtn.innerHTML = '<i class="fa fa-eye fa-fw" aria-hidden="true"></i>';
    }

});