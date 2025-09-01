// Funciones para manejar el responsive del menú
window.getWindowWidth = function () {
    return window.innerWidth;
};

window.addResizeListener = function (dotNetHelper) {
    window.addEventListener('resize', function () {
        dotNetHelper.invokeMethodAsync('OnResize', window.innerWidth);
    });
};

// Función para cerrar el menú al hacer clic en un enlace (móviles)
window.closeMobileMenu = function () {
    // Esta función será llamada desde los enlaces del menú en móviles
    if (window.innerWidth <= 768) {
        // Disparar un evento personalizado para cerrar el menú
        window.dispatchEvent(new CustomEvent('closeMobileMenu'));
    }
};

// Agregar listener para cerrar el menú móvil
window.addMobileMenuListener = function (dotNetHelper) {
    window.addEventListener('closeMobileMenu', function () {
        dotNetHelper.invokeMethodAsync('OnCloseMobileMenu');
    });
};

// Detectar si es móvil al cargar la página
document.addEventListener('DOMContentLoaded', function () {
    if (window.innerWidth <= 768) {
        document.body.classList.add('mobile-device');
    }
});
