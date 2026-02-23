// Initialize Lucide icons
document.addEventListener('DOMContentLoaded', function() {
  lucide.createIcons();
  
  // Check if already logged in
  const isLoggedIn = localStorage.getItem('adminLoggedIn');
  if (isLoggedIn === 'true') {
    window.location.href = 'admin-dashboard.html';
    return;
  }
  
  const loginForm = document.getElementById('login-form');
  const togglePasswordBtn = document.getElementById('toggle-password');
  const passwordInput = document.getElementById('password');
  const alertMessage = document.getElementById('alert-message');
  
  // Toggle password visibility
  togglePasswordBtn.addEventListener('click', function() {
    const type = passwordInput.getAttribute('type');
    
    if (type === 'password') {
      passwordInput.setAttribute('type', 'text');
      this.innerHTML = '<i data-lucide="eye-off" class="w-5 h-5"></i>';
    } else {
      passwordInput.setAttribute('type', 'password');
      this.innerHTML = '<i data-lucide="eye" class="w-5 h-5"></i>';
    }
    
    lucide.createIcons();
  });
  
  // Show alert message
  function showAlert(message, type = 'error') {
    alertMessage.textContent = message;
    alertMessage.className = `p-4 rounded-lg ${
      type === 'error' 
        ? 'bg-red-50 text-red-800 border border-red-200' 
        : 'bg-green-50 text-green-800 border border-green-200'
    }`;
    alertMessage.classList.remove('hidden');
    
    setTimeout(() => {
      alertMessage.classList.add('hidden');
    }, 5000);
  }
  
  // Handle login
  loginForm.addEventListener('submit', function(e) {
    e.preventDefault();
    
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const remember = document.getElementById('remember').checked;
    
    // Simple authentication (in production, this should be server-side)
    if (username === 'admin' && password === 'admin123') {
      // Store login state
      localStorage.setItem('adminLoggedIn', 'true');
      localStorage.setItem('adminUsername', username);
      
      if (remember) {
        localStorage.setItem('rememberMe', 'true');
      }
      
      showAlert('Login realizado com sucesso! Redirecionando...', 'success');
      
      // Redirect to dashboard
      setTimeout(() => {
        window.location.href = 'admin-dashboard.html';
      }, 1000);
    } else {
      showAlert('Usuário ou senha incorretos. Tente novamente.');
      passwordInput.value = '';
      passwordInput.focus();
    }
  });
  
  // Add enter key support
  document.querySelectorAll('input').forEach(input => {
    input.addEventListener('keypress', function(e) {
      if (e.key === 'Enter') {
        loginForm.dispatchEvent(new Event('submit'));
      }
    });
  });
});
