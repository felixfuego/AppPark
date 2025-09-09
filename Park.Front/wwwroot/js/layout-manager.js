// Layout Manager para Park.Front
window.layoutManager = {
    init: function () {
        console.log('Layout Manager initialized');
        this.setupResponsiveHandlers();
        this.setupSmoothTransitions();
    },

    setupResponsiveHandlers: function () {
        // Manejar cambios de tamaño de ventana
        window.addEventListener('resize', function () {
            const sidebar = document.querySelector('.sidebar');
            const main = document.querySelector('main');
            
            if (window.innerWidth <= 768) {
                // En móvil, asegurar que el sidebar esté oculto por defecto
                if (sidebar && !sidebar.classList.contains('show')) {
                    sidebar.style.transform = 'translateX(-100%)';
                }
            } else {
                // En desktop, restaurar comportamiento normal
                if (sidebar && sidebar.classList.contains('collapsed')) {
                    sidebar.style.transform = 'translateX(-100%)';
                }
            }
        });
    },

    setupSmoothTransitions: function () {
        // Añadir transiciones suaves a todos los elementos del layout
        const style = document.createElement('style');
        style.textContent = `
            .sidebar, main, .top-row, .content {
                transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            }
            
            .sidebar.collapsed {
                transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            }
            
            main.sidebar-collapsed {
                transition: margin-left 0.3s cubic-bezier(0.4, 0, 0.2, 1), 
                           width 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            }
        `;
        document.head.appendChild(style);
    },

    toggleSidebar: function (collapsed) {
        const sidebar = document.querySelector('.sidebar');
        const main = document.querySelector('main');
        
        if (sidebar && main) {
            if (collapsed) {
                sidebar.classList.add('collapsed');
                main.classList.add('sidebar-collapsed');
            } else {
                sidebar.classList.remove('collapsed');
                main.classList.remove('sidebar-collapsed');
            }
        }
    },

    showMobileSidebar: function (show) {
        const sidebar = document.querySelector('.sidebar');
        const overlay = document.querySelector('.sidebar-overlay');
        
        if (sidebar) {
            if (show) {
                sidebar.classList.add('show');
                if (overlay) overlay.style.display = 'block';
            } else {
                sidebar.classList.remove('show');
                if (overlay) overlay.style.display = 'none';
            }
        }
    }
};
