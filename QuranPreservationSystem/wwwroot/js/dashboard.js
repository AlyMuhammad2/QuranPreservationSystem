// Dashboard JavaScript

// User Menu Dropdown
document.addEventListener('DOMContentLoaded', function() {
    
    // User Dropdown Toggle
    const userMenuBtn = document.getElementById('userMenuBtn');
    const userDropdown = document.getElementById('userDropdown');
    
    if (userMenuBtn && userDropdown) {
        userMenuBtn.addEventListener('click', function(e) {
            e.stopPropagation();
            userDropdown.classList.toggle('show');
        });
        
        // Close dropdown when clicking outside
        document.addEventListener('click', function(e) {
            if (!userMenuBtn.contains(e.target) && !userDropdown.contains(e.target)) {
                userDropdown.classList.remove('show');
            }
        });
    }
    
    // Notifications Toggle
    const notificationsBtn = document.getElementById('notificationsBtn');
    
    if (notificationsBtn) {
        notificationsBtn.addEventListener('click', function(e) {
            e.stopPropagation();
            // TODO: Implement notifications dropdown
            console.log('Notifications clicked');
        });
    }
    
    // Sidebar Active State (for mobile)
    const sidebarItems = document.querySelectorAll('.sidebar-item');
    
    sidebarItems.forEach(item => {
        item.addEventListener('click', function() {
            // Remove active from all items
            sidebarItems.forEach(i => i.classList.remove('active'));
            // Add active to clicked item
            this.classList.add('active');
        });
    });
    
    // Auto-hide mobile sidebar after click
    if (window.innerWidth <= 768) {
        sidebarItems.forEach(item => {
            item.addEventListener('click', function() {
                const sidebar = document.querySelector('.dashboard-sidebar');
                if (sidebar) {
                    sidebar.style.display = 'none';
                    setTimeout(() => {
                        sidebar.style.display = 'block';
                    }, 100);
                }
            });
        });
    }
    
    // Smooth scroll for page transitions
    window.addEventListener('beforeunload', function() {
        window.scrollTo(0, 0);
    });
    
    // Add loading animation
    const mainContent = document.querySelector('.main-content');
    if (mainContent) {
        mainContent.style.opacity = '0';
        setTimeout(() => {
            mainContent.style.transition = 'opacity 0.3s ease';
            mainContent.style.opacity = '1';
        }, 50);
    }
});

// Helper Functions

// Show Toast Notification
function showToast(message, type = 'success') {
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.innerHTML = `
        <i class="fas ${type === 'success' ? 'fa-check-circle' : type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle'}"></i>
        <span>${message}</span>
    `;
    
    // Add toast styles if not exist
    if (!document.getElementById('toast-styles')) {
        const style = document.createElement('style');
        style.id = 'toast-styles';
        style.textContent = `
            .toast {
                position: fixed;
                top: 90px;
                left: 20px;
                padding: 15px 25px;
                border-radius: 10px;
                color: white;
                font-weight: 500;
                box-shadow: 0 5px 20px rgba(0,0,0,0.2);
                z-index: 1100;
                display: flex;
                align-items: center;
                gap: 12px;
                transform: translateX(-150%);
                transition: transform 0.3s ease;
                min-width: 300px;
            }
            .toast.show {
                transform: translateX(0);
            }
            .toast-success {
                background: linear-gradient(135deg, #43a047 0%, #2e7d32 100%);
            }
            .toast-error {
                background: linear-gradient(135deg, #e53935 0%, #c62828 100%);
            }
            .toast-warning {
                background: linear-gradient(135deg, #ff9800 0%, #ef6c00 100%);
            }
            .toast-info {
                background: linear-gradient(135deg, #2196f3 0%, #1976d2 100%);
            }
        `;
        document.head.appendChild(style);
    }
    
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.classList.add('show');
    }, 100);
    
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => {
            toast.remove();
        }, 300);
    }, 3000);
}

// Confirm Dialog
function confirmAction(message, callback) {
    if (confirm(message)) {
        callback();
    }
}

// Format Date
function formatDate(dateString) {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
}

// Format Number
function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

