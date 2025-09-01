// Funciones para manejar modales de Bootstrap
window.showModal = function (modalId) {
    const modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
};

window.hideModal = function (modalId) {
    const modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modal) {
        modal.hide();
    }
};
