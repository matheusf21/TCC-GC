// Check authentication
document.addEventListener('DOMContentLoaded', function() {
  const isLoggedIn = localStorage.getItem('adminLoggedIn');
  
  if (isLoggedIn !== 'true') {
    window.location.href = 'admin-login.html';
    return;
  }
  
  // Initialize
  lucide.createIcons();
  initializeAdmin();
  loadProducts();
  loadMessages();
  loadTestimonials();
});

// Initialize admin panel
function initializeAdmin() {
  const adminName = localStorage.getItem('adminUsername');
  document.getElementById('admin-name').textContent = adminName || 'Administrador';
  
  // Sidebar navigation
  const navLinks = document.querySelectorAll('.nav-link');
  const sections = document.querySelectorAll('.content-section');
  
  navLinks.forEach(link => {
    link.addEventListener('click', function(e) {
      e.preventDefault();
      
      // Remove active class from all links
      navLinks.forEach(l => l.classList.remove('active', 'bg-orange-50', 'text-orange-600'));
      
      // Add active class to clicked link
      this.classList.add('active', 'bg-orange-50', 'text-orange-600');
      
      // Hide all sections
      sections.forEach(s => s.classList.add('hidden'));
      
      // Show selected section
      const sectionId = 'section-' + this.dataset.section;
      document.getElementById(sectionId).classList.remove('hidden');
      
      // Close mobile sidebar
      document.getElementById('sidebar').classList.add('-translate-x-full');
      document.getElementById('sidebar').classList.remove('translate-x-0');
      
      lucide.createIcons();
    });
  });
  
  // Sidebar toggle for mobile
  const sidebarToggle = document.getElementById('sidebar-toggle');
  const sidebar = document.getElementById('sidebar');
  
  sidebarToggle.addEventListener('click', function() {
    sidebar.classList.toggle('-translate-x-full');
    sidebar.classList.toggle('translate-x-0');
  });
  
  // User menu toggle
  const userMenuBtn = document.getElementById('user-menu-btn');
  const userMenu = document.getElementById('user-menu');
  
  userMenuBtn.addEventListener('click', function(e) {
    e.stopPropagation();
    userMenu.classList.toggle('hidden');
    lucide.createIcons();
  });
  
  // Close user menu when clicking outside
  document.addEventListener('click', function(e) {
    if (!userMenu.contains(e.target) && !userMenuBtn.contains(e.target)) {
      userMenu.classList.add('hidden');
    }
  });
  
  // Logout
  document.getElementById('logout-btn').addEventListener('click', function(e) {
    e.preventDefault();
    
    if (confirm('Tem certeza que deseja sair?')) {
      localStorage.removeItem('adminLoggedIn');
      localStorage.removeItem('adminUsername');
      window.location.href = 'admin-login.html';
    }
  });
  
  // Product modal handlers
  const addProductBtn = document.getElementById('add-product-btn');
  const productModal = document.getElementById('product-modal');
  const closeProductModal = document.getElementById('close-product-modal');
  const cancelProductBtn = document.getElementById('cancel-product-btn');
  const productForm = document.getElementById('product-form');
  
  addProductBtn.addEventListener('click', function() {
    document.getElementById('product-modal-title').textContent = 'Adicionar Produto';
    productForm.reset();
    productModal.classList.remove('hidden');
    lucide.createIcons();
  });
  
  closeProductModal.addEventListener('click', function() {
    productModal.classList.add('hidden');
  });
  
  cancelProductBtn.addEventListener('click', function() {
    productModal.classList.add('hidden');
  });
  
  productForm.addEventListener('submit', function(e) {
    e.preventDefault();
    
    const product = {
      id: Date.now(),
      name: document.getElementById('product-name').value,
      category: document.getElementById('product-category').value,
      stock: document.getElementById('product-stock').value,
      price: document.getElementById('product-price').value,
      description: document.getElementById('product-description').value,
      status: 'active'
    };
    
    // Get existing products
    let products = JSON.parse(localStorage.getItem('products') || '[]');
    products.push(product);
    localStorage.setItem('products', JSON.stringify(products));
    
    // Reload products
    loadProducts();
    
    // Close modal
    productModal.classList.add('hidden');
    
    alert('Produto adicionado com sucesso!');
  });
  
  // Message modal handlers
  const messageModal = document.getElementById('message-modal');
  const closeMessageModal = document.getElementById('close-message-modal');
  
  closeMessageModal.addEventListener('click', function() {
    messageModal.classList.add('hidden');
  });
  
  // Search products
  const searchProducts = document.getElementById('search-products');
  if (searchProducts) {
    searchProducts.addEventListener('input', function() {
      loadProducts(this.value.toLowerCase());
    });
  }
}

// Load Products
function loadProducts(searchTerm = '') {
  let products = JSON.parse(localStorage.getItem('products') || '[]');
  
  // Add demo products if empty
  if (products.length === 0) {
    products = [
      { id: 1, name: 'Cimento 50kg', category: 'Materiais Básicos', stock: 150, price: 32.90, status: 'active', description: 'Cimento de alta qualidade' },
      { id: 2, name: 'Tijolo Cerâmico', category: 'Materiais Básicos', stock: 2500, price: 0.85, status: 'active', description: 'Tijolo cerâmico tradicional' },
      { id: 3, name: 'Tinta Branca 18L', category: 'Acabamento', stock: 80, price: 180.00, status: 'active', description: 'Tinta acrílica branca' },
      { id: 4, name: 'Furadeira', category: 'Ferramentas', stock: 25, price: 350.00, status: 'active', description: 'Furadeira elétrica profissional' },
      { id: 5, name: 'Cabo Flexível 2.5mm', category: 'Elétrica e Hidráulica', stock: 500, price: 2.50, status: 'active', description: 'Cabo elétrico flexível' },
      { id: 6, name: 'Areia Lavada m³', category: 'Materiais Básicos', stock: 50, price: 80.00, status: 'low-stock', description: 'Areia lavada para construção' }
    ];
    localStorage.setItem('products', JSON.stringify(products));
  }
  
  // Filter by search term
  if (searchTerm) {
    products = products.filter(p => 
      p.name.toLowerCase().includes(searchTerm) ||
      p.category.toLowerCase().includes(searchTerm)
    );
  }
  
  const tbody = document.getElementById('products-table-body');
  tbody.innerHTML = '';
  
  products.forEach(product => {
    const statusClass = product.status === 'active' ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800';
    const statusText = product.status === 'active' ? 'Ativo' : 'Estoque Baixo';
    
    const row = `
      <tr>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="flex items-center">
            <div class="w-10 h-10 bg-gray-200 rounded flex-shrink-0"></div>
            <div class="ml-4">
              <div class="text-sm font-medium text-gray-900">${product.name}</div>
              <div class="text-sm text-gray-500">${product.description}</div>
            </div>
          </div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <span class="text-sm text-gray-900">${product.category}</span>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <span class="text-sm text-gray-900">${product.stock} un.</span>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <span class="text-sm text-gray-900">R$ ${parseFloat(product.price).toFixed(2)}</span>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <span class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${statusClass}">
            ${statusText}
          </span>
        </td>
        <td class="px-6 py-4 whitespace-nowrap text-sm">
          <button onclick="editProduct(${product.id})" class="text-blue-600 hover:text-blue-900 mr-3">
            <i data-lucide="edit" class="w-4 h-4 inline"></i>
          </button>
          <button onclick="deleteProduct(${product.id})" class="text-red-600 hover:text-red-900">
            <i data-lucide="trash-2" class="w-4 h-4 inline"></i>
          </button>
        </td>
      </tr>
    `;
    
    tbody.innerHTML += row;
  });
  
  lucide.createIcons();
}

// Edit Product
function editProduct(id) {
  const products = JSON.parse(localStorage.getItem('products') || '[]');
  const product = products.find(p => p.id === id);
  
  if (product) {
    document.getElementById('product-modal-title').textContent = 'Editar Produto';
    document.getElementById('product-name').value = product.name;
    document.getElementById('product-category').value = product.category;
    document.getElementById('product-stock').value = product.stock;
    document.getElementById('product-price').value = product.price;
    document.getElementById('product-description').value = product.description;
    
    document.getElementById('product-modal').classList.remove('hidden');
    lucide.createIcons();
  }
}

// Delete Product
function deleteProduct(id) {
  if (confirm('Tem certeza que deseja excluir este produto?')) {
    let products = JSON.parse(localStorage.getItem('products') || '[]');
    products = products.filter(p => p.id !== id);
    localStorage.setItem('products', JSON.stringify(products));
    loadProducts();
    alert('Produto excluído com sucesso!');
  }
}

// Load Messages
function loadMessages() {
  let messages = JSON.parse(localStorage.getItem('messages') || '[]');
  
  // Add demo messages if empty
  if (messages.length === 0) {
    messages = [
      { id: 1, name: 'João Silva', email: 'joao@email.com', phone: '(11) 98888-7777', message: 'Gostaria de um orçamento para reforma completa.', date: '2026-02-23', status: 'new' },
      { id: 2, name: 'Maria Santos', email: 'maria@email.com', phone: '(11) 97777-6666', message: 'Preciso de materiais para construção de muro.', date: '2026-02-23', status: 'new' },
      { id: 3, name: 'Carlos Lima', email: 'carlos@email.com', phone: '(11) 96666-5555', message: 'Vocês entregam em Guarulhos?', date: '2026-02-22', status: 'read' },
      { id: 4, name: 'Ana Paula', email: 'ana@email.com', phone: '(11) 95555-4444', message: 'Qual o preço do cimento 50kg?', date: '2026-02-22', status: 'replied' },
      { id: 5, name: 'Pedro Costa', email: 'pedro@email.com', phone: '(11) 94444-3333', message: 'Gostaria de fazer um pedido grande de tijolos.', date: '2026-02-21', status: 'new' }
    ];
    localStorage.setItem('messages', JSON.stringify(messages));
  }
  
  const tbody = document.getElementById('messages-table-body');
  tbody.innerHTML = '';
  
  // Update messages count
  const newMessages = messages.filter(m => m.status === 'new').length;
  document.getElementById('messages-count').textContent = newMessages;
  
  messages.forEach(message => {
    let statusClass, statusText;
    
    switch(message.status) {
      case 'new':
        statusClass = 'bg-blue-100 text-blue-800';
        statusText = 'Nova';
        break;
      case 'read':
        statusClass = 'bg-gray-100 text-gray-800';
        statusText = 'Lida';
        break;
      case 'replied':
        statusClass = 'bg-green-100 text-green-800';
        statusText = 'Respondida';
        break;
    }
    
    const row = `
      <tr class="${message.status === 'new' ? 'bg-blue-50' : ''}">
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm font-medium text-gray-900">${message.name}</div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">${message.email}</div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">${message.phone}</div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <div class="text-sm text-gray-900">${message.date}</div>
        </td>
        <td class="px-6 py-4 whitespace-nowrap">
          <span class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full ${statusClass}">
            ${statusText}
          </span>
        </td>
        <td class="px-6 py-4 whitespace-nowrap text-sm">
          <button onclick="viewMessage(${message.id})" class="text-blue-600 hover:text-blue-900 mr-3">
            <i data-lucide="eye" class="w-4 h-4 inline"></i>
          </button>
          <button onclick="deleteMessage(${message.id})" class="text-red-600 hover:text-red-900">
            <i data-lucide="trash-2" class="w-4 h-4 inline"></i>
          </button>
        </td>
      </tr>
    `;
    
    tbody.innerHTML += row;
  });
  
  lucide.createIcons();
}

// View Message
function viewMessage(id) {
  let messages = JSON.parse(localStorage.getItem('messages') || '[]');
  const message = messages.find(m => m.id === id);
  
  if (message) {
    // Mark as read
    if (message.status === 'new') {
      message.status = 'read';
      messages = messages.map(m => m.id === id ? message : m);
      localStorage.setItem('messages', JSON.stringify(messages));
      loadMessages();
    }
    
    const detailsHtml = `
      <div class="space-y-4">
        <div>
          <label class="text-sm font-medium text-gray-500">Nome</label>
          <p class="text-gray-900">${message.name}</p>
        </div>
        <div>
          <label class="text-sm font-medium text-gray-500">Email</label>
          <p class="text-gray-900">${message.email}</p>
        </div>
        <div>
          <label class="text-sm font-medium text-gray-500">Telefone</label>
          <p class="text-gray-900">${message.phone}</p>
        </div>
        <div>
          <label class="text-sm font-medium text-gray-500">Data</label>
          <p class="text-gray-900">${message.date}</p>
        </div>
        <div>
          <label class="text-sm font-medium text-gray-500">Mensagem</label>
          <p class="text-gray-900">${message.message}</p>
        </div>
        <div class="flex gap-3 pt-4">
          <a href="mailto:${message.email}" class="flex-1 bg-orange-600 text-white px-4 py-2 rounded-lg hover:bg-orange-700 transition text-center">
            Responder por Email
          </a>
          <a href="tel:${message.phone}" class="flex-1 bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 transition text-center">
            Ligar
          </a>
        </div>
      </div>
    `;
    
    document.getElementById('message-details').innerHTML = detailsHtml;
    document.getElementById('message-modal').classList.remove('hidden');
  }
}

// Delete Message
function deleteMessage(id) {
  if (confirm('Tem certeza que deseja excluir esta mensagem?')) {
    let messages = JSON.parse(localStorage.getItem('messages') || '[]');
    messages = messages.filter(m => m.id !== id);
    localStorage.setItem('messages', JSON.stringify(messages));
    loadMessages();
    alert('Mensagem excluída com sucesso!');
  }
}

// Load Testimonials
function loadTestimonials() {
  let testimonials = JSON.parse(localStorage.getItem('testimonials') || '[]');
  
  // Add demo testimonials if empty
  if (testimonials.length === 0) {
    testimonials = [
      { id: 1, name: 'Carlos Silva', role: 'Engenheiro Civil', content: 'Excelente qualidade dos produtos e atendimento impecável. Sempre encontro tudo que preciso para minhas obras.', rating: 5, status: 'approved' },
      { id: 2, name: 'Maria Santos', role: 'Arquiteta', content: 'Trabalho com a CONSTRUMAX há anos. Preços justos e entrega sempre no prazo. Super recomendo!', rating: 5, status: 'approved' },
      { id: 3, name: 'João Oliveira', role: 'Mestre de Obras', content: 'Produtos de primeira qualidade e a equipe sempre me auxilia na escolha dos materiais certos para cada projeto.', rating: 5, status: 'approved' },
      { id: 4, name: 'Ana Costa', role: 'Cliente', content: 'Fiz a reforma da minha casa com materiais da CONSTRUMAX. Ficou perfeito!', rating: 5, status: 'pending' }
    ];
    localStorage.setItem('testimonials', JSON.stringify(testimonials));
  }
  
  const grid = document.getElementById('testimonials-grid');
  grid.innerHTML = '';
  
  testimonials.forEach(testimonial => {
    const statusClass = testimonial.status === 'approved' ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800';
    const statusText = testimonial.status === 'approved' ? 'Aprovado' : 'Pendente';
    
    const card = `
      <div class="bg-white rounded-lg shadow p-6 relative">
        <span class="absolute top-4 right-4 px-2 py-1 text-xs rounded-full ${statusClass}">
          ${statusText}
        </span>
        
        <div class="flex gap-1 mb-4">
          ${Array(testimonial.rating).fill('<i data-lucide="star" class="w-4 h-4 fill-orange-500 text-orange-500"></i>').join('')}
        </div>
        
        <p class="text-gray-600 mb-4 italic">"${testimonial.content}"</p>
        
        <div class="mb-4">
          <p class="font-semibold">${testimonial.name}</p>
          <p class="text-sm text-gray-500">${testimonial.role}</p>
        </div>
        
        <div class="flex gap-2">
          ${testimonial.status === 'pending' ? `
            <button onclick="approveTestimonial(${testimonial.id})" class="flex-1 bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 transition text-sm">
              Aprovar
            </button>
          ` : ''}
          <button onclick="deleteTestimonial(${testimonial.id})" class="flex-1 bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-700 transition text-sm">
            Excluir
          </button>
        </div>
      </div>
    `;
    
    grid.innerHTML += card;
  });
  
  lucide.createIcons();
}

// Approve Testimonial
function approveTestimonial(id) {
  let testimonials = JSON.parse(localStorage.getItem('testimonials') || '[]');
  testimonials = testimonials.map(t => t.id === id ? {...t, status: 'approved'} : t);
  localStorage.setItem('testimonials', JSON.stringify(testimonials));
  loadTestimonials();
  alert('Depoimento aprovado com sucesso!');
}

// Delete Testimonial
function deleteTestimonial(id) {
  if (confirm('Tem certeza que deseja excluir este depoimento?')) {
    let testimonials = JSON.parse(localStorage.getItem('testimonials') || '[]');
    testimonials = testimonials.filter(t => t.id !== id);
    localStorage.setItem('testimonials', JSON.stringify(testimonials));
    loadTestimonials();
    alert('Depoimento excluído com sucesso!');
  }
}
