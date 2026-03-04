// Initialize Lucide icons
document.addEventListener('DOMContentLoaded', function() {
  // Create all icons
  lucide.createIcons();
  
  // Mobile Menu Toggle
  const mobileMenuBtn = document.getElementById('mobile-menu-btn');
  const mobileMenu = document.getElementById('mobile-menu');
  
  if (mobileMenuBtn && mobileMenu) {
    mobileMenuBtn.addEventListener('click', function() {
      mobileMenu.classList.toggle('hidden');
    });
    
    // Close mobile menu when clicking on a link
    const mobileLinks = mobileMenu.querySelectorAll('a');
    mobileLinks.forEach(link => {
      link.addEventListener('click', function() {
        mobileMenu.classList.add('hidden');
      });
    });
  }
  
  // Contact Form Submission
  const contactForm = document.getElementById('contact-form');
  
  if (contactForm) {
    contactForm.addEventListener('submit', function(e) {
      e.preventDefault();
      
      // Get form values
      const formData = {
        name: document.getElementById('name').value,
        email: document.getElementById('email').value,
        phone: document.getElementById('phone').value,
        message: document.getElementById('message').value
      };
      
      // Show success message
      alert('Obrigado pelo contato! Entraremos em contato em breve.');
      
      // Reset form
      contactForm.reset();
      
      // In a real application, you would send this data to a server
      console.log('Form submitted:', formData);
    });
  }
  
  // Smooth scroll for anchor links
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function(e) {
      const href = this.getAttribute('href');
      
      // Skip if href is just "#"
      if (href === '#') {
        e.preventDefault();
        return;
      }
      
      const target = document.querySelector(href);
      
      if (target) {
        e.preventDefault();
        
        const headerOffset = 80;
        const elementPosition = target.getBoundingClientRect().top;
        const offsetPosition = elementPosition + window.pageYOffset - headerOffset;
        
        window.scrollTo({
          top: offsetPosition,
          behavior: 'smooth'
        });
      }
    });
  });
  
  // Add active state to navigation links based on scroll position
  const sections = document.querySelectorAll('section[id]');
  const navLinks = document.querySelectorAll('nav a[href^="#"]');
  
  function setActiveLink() {
    let current = '';
    
    sections.forEach(section => {
      const sectionTop = section.offsetTop;
      const sectionHeight = section.clientHeight;
      
      if (window.pageYOffset >= sectionTop - 200) {
        current = section.getAttribute('id');
      }
    });
    
    navLinks.forEach(link => {
      link.classList.remove('text-orange-600');
      const href = link.getAttribute('href');
      
      if (href === `#${current}`) {
        link.classList.add('text-orange-600');
      }
    });
  }
  
  window.addEventListener('scroll', setActiveLink);
  
  // Add scroll-to-top functionality
  let scrollTopBtn = null;
  
  function createScrollTopButton() {
    scrollTopBtn = document.createElement('button');
    scrollTopBtn.innerHTML = '<i data-lucide="arrow-up" class="w-6 h-6"></i>';
    scrollTopBtn.className = 'fixed bottom-8 right-8 bg-orange-600 text-white p-3 rounded-full shadow-lg hover:bg-orange-700 transition-all opacity-0 pointer-events-none z-50';
    scrollTopBtn.setAttribute('aria-label', 'Voltar ao topo');
    document.body.appendChild(scrollTopBtn);
    
    // Re-initialize icons for the new button
    lucide.createIcons();
    
    scrollTopBtn.addEventListener('click', function() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth'
      });
    });
  }
  
  createScrollTopButton();
  
  window.addEventListener('scroll', function() {
    if (scrollTopBtn) {
      if (window.pageYOffset > 300) {
        scrollTopBtn.style.opacity = '1';
        scrollTopBtn.style.pointerEvents = 'auto';
      } else {
        scrollTopBtn.style.opacity = '0';
        scrollTopBtn.style.pointerEvents = 'none';
      }
    }
  });
  
  // Image lazy loading fallback
  const images = document.querySelectorAll('img');
  
  images.forEach(img => {
    img.setAttribute('data-loaded', 'false');
    
    img.addEventListener('load', function() {
      this.setAttribute('data-loaded', 'true');
    });
    
    img.addEventListener('error', function() {
      console.warn('Failed to load image:', this.src);
      this.setAttribute('data-loaded', 'true');
    });
  });
  
  // Add click handlers to "Solicitar Orçamento" buttons
  const quoteButtons = document.querySelectorAll('button');
  
  quoteButtons.forEach(button => {
    if (button.textContent.includes('Solicitar Orçamento')) {
      button.addEventListener('click', function() {
        // Scroll to contact form
        const contactSection = document.getElementById('contato');
        if (contactSection) {
          const headerOffset = 80;
          const elementPosition = contactSection.getBoundingClientRect().top;
          const offsetPosition = elementPosition + window.pageYOffset - headerOffset;
          
          window.scrollTo({
            top: offsetPosition,
            behavior: 'smooth'
          });
          
          // Focus on the form's first input
          setTimeout(() => {
            const nameInput = document.getElementById('name');
            if (nameInput) {
              nameInput.focus();
            }
          }, 800);
        }
      });
    }
  });
  
  // Add animation on scroll for elements
  const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -100px 0px'
  };
  
  const observer = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('animate-fade-in');
        observer.unobserve(entry.target);
      }
    });
  }, observerOptions);
  
  // Observe sections for animation
  sections.forEach(section => {
    observer.observe(section);
  });
});

// Handle window resize
let resizeTimer;
window.addEventListener('resize', function() {
  clearTimeout(resizeTimer);
  resizeTimer = setTimeout(function() {
    // Close mobile menu on resize to desktop
    if (window.innerWidth >= 768) {
      const mobileMenu = document.getElementById('mobile-menu');
      if (mobileMenu) {
        mobileMenu.classList.add('hidden');
      }
    }
  }, 250);
});

// Console message
console.log('%c CONSTRUMAX ', 'background: #ea580c; color: white; font-size: 20px; padding: 10px;');
console.log('%c Website desenvolvido com HTML, CSS e JavaScript ', 'color: #ea580c; font-size: 14px;');
