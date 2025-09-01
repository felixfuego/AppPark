// Variables globales para almacenar las instancias de los gráficos
let visitsChart = null;
let statusChart = null;
let companyChart = null;
let hourChart = null;

// Función para renderizar el gráfico de visitas por día
window.renderVisitsChart = function (canvasId, labels, values) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    // Destruir gráfico existente si existe
    if (visitsChart) {
        visitsChart.destroy();
    }

    visitsChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Visitas',
                data: values,
                borderColor: '#007bff',
                backgroundColor: 'rgba(0, 123, 255, 0.1)',
                borderWidth: 2,
                fill: true,
                tension: 0.4,
                pointBackgroundColor: '#007bff',
                pointBorderColor: '#fff',
                pointBorderWidth: 2,
                pointRadius: 4,
                pointHoverRadius: 6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: '#fff',
                    bodyColor: '#fff',
                    borderColor: '#007bff',
                    borderWidth: 1,
                    cornerRadius: 8,
                    displayColors: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0, 0, 0, 0.1)',
                        drawBorder: false
                    },
                    ticks: {
                        color: '#6c757d',
                        font: {
                            size: 12
                        }
                    }
                },
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        color: '#6c757d',
                        font: {
                            size: 12
                        }
                    }
                }
            },
            interaction: {
                intersect: false,
                mode: 'index'
            }
        }
    });
};

// Función para renderizar el gráfico de estado de visitas
window.renderStatusChart = function (canvasId, labels, values, colors) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    // Destruir gráfico existente si existe
    if (statusChart) {
        statusChart.destroy();
    }

    statusChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: colors,
                borderWidth: 2,
                borderColor: '#fff',
                hoverBorderWidth: 3,
                hoverBorderColor: '#fff'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 20,
                        usePointStyle: true,
                        font: {
                            size: 12
                        }
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: '#fff',
                    bodyColor: '#fff',
                    borderColor: '#007bff',
                    borderWidth: 1,
                    cornerRadius: 8,
                    callbacks: {
                        label: function (context) {
                            const label = context.label || '';
                            const value = context.parsed;
                            const total = context.dataset.data.reduce((a, b) => a + b, 0);
                            const percentage = ((value / total) * 100).toFixed(1);
                            return `${label}: ${value} (${percentage}%)`;
                        }
                    }
                }
            },
            cutout: '60%'
        }
    });
};

// Función para renderizar el gráfico de visitas por empresa
window.renderCompanyChart = function (canvasId, labels, values) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    // Destruir gráfico existente si existe
    if (companyChart) {
        companyChart.destroy();
    }

    // Generar colores dinámicos
    const colors = generateColors(labels.length);

    companyChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Visitas',
                data: values,
                backgroundColor: colors,
                borderColor: colors.map(color => color.replace('0.8', '1')),
                borderWidth: 1,
                borderRadius: 4,
                borderSkipped: false
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: '#fff',
                    bodyColor: '#fff',
                    borderColor: '#007bff',
                    borderWidth: 1,
                    cornerRadius: 8,
                    displayColors: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0, 0, 0, 0.1)',
                        drawBorder: false
                    },
                    ticks: {
                        color: '#6c757d',
                        font: {
                            size: 12
                        }
                    }
                },
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        color: '#6c757d',
                        font: {
                            size: 11
                        },
                        maxRotation: 45,
                        minRotation: 45
                    }
                }
            }
        }
    });
};

// Función para renderizar el gráfico de horarios de visita
window.renderHourChart = function (canvasId, labels, values) {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    // Destruir gráfico existente si existe
    if (hourChart) {
        hourChart.destroy();
    }

    hourChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Visitas',
                data: values,
                backgroundColor: 'rgba(40, 167, 69, 0.8)',
                borderColor: '#28a745',
                borderWidth: 1,
                borderRadius: 4,
                borderSkipped: false
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleColor: '#fff',
                    bodyColor: '#fff',
                    borderColor: '#28a745',
                    borderWidth: 1,
                    cornerRadius: 8,
                    displayColors: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0, 0, 0, 0.1)',
                        drawBorder: false
                    },
                    ticks: {
                        color: '#6c757d',
                        font: {
                            size: 12
                        }
                    }
                },
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        color: '#6c757d',
                        font: {
                            size: 12
                        }
                    }
                }
            }
        }
    });
};

// Función para generar colores dinámicos
function generateColors(count) {
    const colors = [
        'rgba(0, 123, 255, 0.8)',
        'rgba(40, 167, 69, 0.8)',
        'rgba(255, 193, 7, 0.8)',
        'rgba(220, 53, 69, 0.8)',
        'rgba(23, 162, 184, 0.8)',
        'rgba(102, 16, 242, 0.8)',
        'rgba(253, 126, 20, 0.8)',
        'rgba(108, 117, 125, 0.8)',
        'rgba(111, 66, 193, 0.8)',
        'rgba(214, 51, 108, 0.8)'
    ];

    const result = [];
    for (let i = 0; i < count; i++) {
        result.push(colors[i % colors.length]);
    }
    return result;
}

// Función para limpiar todos los gráficos
window.clearAllCharts = function () {
    if (visitsChart) {
        visitsChart.destroy();
        visitsChart = null;
    }
    if (statusChart) {
        statusChart.destroy();
        statusChart = null;
    }
    if (companyChart) {
        companyChart.destroy();
        companyChart = null;
    }
    if (hourChart) {
        hourChart.destroy();
        hourChart = null;
    }
};

// Función para redimensionar todos los gráficos
window.resizeAllCharts = function () {
    if (visitsChart) visitsChart.resize();
    if (statusChart) statusChart.resize();
    if (companyChart) companyChart.resize();
    if (hourChart) hourChart.resize();
};

// Event listener para redimensionar gráficos cuando cambie el tamaño de la ventana
window.addEventListener('resize', function () {
    setTimeout(resizeAllCharts, 100);
});
