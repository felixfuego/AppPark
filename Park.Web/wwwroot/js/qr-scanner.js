// QR Scanner JavaScript para Blazor WebAssembly
let videoStream = null;
let videoElement = null;
let qrScanner = null;
let dotNetHelper = null;
let isScanning = false;

// Verificar soporte de cámara
window.checkCameraSupport = function () {
    return !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
};

// Inicializar el escáner QR
window.initializeQRScanner = function (videoId, dotNetRef) {
    videoElement = document.getElementById(videoId);
    dotNetHelper = dotNetRef;
    
    console.log('QR Scanner inicializado');
    
    // Verificar si el navegador soporta getUserMedia
    if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
        console.error('getUserMedia no está soportado en este navegador');
        return false;
    }
    
    return true;
};

// Iniciar la cámara
window.startCamera = async function () {
    try {
        if (videoStream) {
            stopCamera();
        }
        
        // Configuración de la cámara
        const constraints = {
            video: {
                facingMode: 'environment', // Preferir cámara trasera en móviles
                width: { ideal: 1280 },
                height: { ideal: 720 }
            }
        };
        
        videoStream = await navigator.mediaDevices.getUserMedia(constraints);
        videoElement.srcObject = videoStream;
        
        // Esperar a que el video esté listo
        await new Promise((resolve) => {
            videoElement.onloadedmetadata = () => {
                videoElement.play();
                resolve();
            };
        });
        
        // Iniciar el escaneo QR
        startQRScanning();
        
        console.log('Cámara iniciada correctamente');
        return true;
    } catch (error) {
        console.error('Error al iniciar la cámara:', error);
        throw error;
    }
};

// Detener la cámara
window.stopCamera = function () {
    if (videoStream) {
        videoStream.getTracks().forEach(track => track.stop());
        videoStream = null;
    }
    
    if (videoElement) {
        videoElement.srcObject = null;
    }
    
    stopQRScanning();
    console.log('Cámara detenida');
};

// Cambiar de cámara
window.switchCamera = async function () {
    try {
        if (!videoStream) return;
        
        // Detener la cámara actual
        stopCamera();
        
        // Esperar un momento
        await new Promise(resolve => setTimeout(resolve, 500));
        
        // Iniciar con la otra cámara
        const constraints = {
            video: {
                facingMode: videoStream.getVideoTracks()[0].getSettings().facingMode === 'user' ? 'environment' : 'user'
            }
        };
        
        videoStream = await navigator.mediaDevices.getUserMedia(constraints);
        videoElement.srcObject = videoStream;
        
        await new Promise((resolve) => {
            videoElement.onloadedmetadata = () => {
                videoElement.play();
                resolve();
            };
        });
        
        startQRScanning();
        console.log('Cámara cambiada');
    } catch (error) {
        console.error('Error al cambiar de cámara:', error);
        throw error;
    }
};

// Iniciar el escaneo QR
function startQRScanning() {
    if (isScanning) return;
    
    isScanning = true;
    
    // Crear canvas para procesar el video
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    
    // Función de escaneo
    function scanFrame() {
        if (!isScanning || !videoElement || videoElement.videoWidth === 0) {
            return;
        }
        
        try {
            // Configurar canvas
            canvas.width = videoElement.videoWidth;
            canvas.height = videoElement.videoHeight;
            
            // Dibujar frame del video en el canvas
            context.drawImage(videoElement, 0, 0, canvas.width, canvas.height);
            
            // Obtener datos de imagen
            const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
            
            // Procesar para detectar QR (simulación)
            const qrCode = detectQRCode(imageData);
            
            if (qrCode) {
                console.log('QR Code detectado:', qrCode);
                dotNetHelper.invokeMethodAsync('OnQRCodeDetected', qrCode);
                isScanning = false;
                return;
            }
            
            // Continuar escaneando
            requestAnimationFrame(scanFrame);
        } catch (error) {
            console.error('Error en el escaneo:', error);
            isScanning = false;
        }
    }
    
    // Iniciar el escaneo
    scanFrame();
}

// Detener el escaneo QR
function stopQRScanning() {
    isScanning = false;
}

// Detectar código QR (simulación - en producción usar librería como jsQR)
function detectQRCode(imageData) {
    // Esta es una simulación. En producción, usar una librería como jsQR
    // Por ahora, simulamos la detección para pruebas
    
    // Simular detección aleatoria (solo para pruebas)
    if (Math.random() < 0.001) { // 0.1% de probabilidad
        return generateMockQRCode();
    }
    
    return null;
}

// Generar código QR simulado para pruebas
function generateMockQRCode() {
    const mockCodes = [
        'eyJWaXNpdENvZGUiOiJWSVMyMDI0MDgyMjAwMDEiLCJWaXNpdG9yTmFtZSI6Ikp1YW4gUGVyZXoiLCJDb21wYW55TmFtZSI6IkVtcHJlc2EgVGVzdCIsIlNjaGVkdWxlZERhdGUiOiIyMDI0LTA4LTIyVDEwOjAwOjAwIiwiR2F0ZUlkIjoiR0FURTAxIiwiU2VjdXJpdHlIYXNoIjoiYWJjZGVmZ2hpamtsbW5vcHFyc3R1dnd4eXoifQ==',
        'eyJWaXNpdENvZGUiOiJWSVMyMDI0MDgyMjAwMDIiLCJWaXNpdG9yTmFtZSI6Ik1hcmlhIEdhcmNpYSIsIkNvbXBhbnlOYW1lIjoiQ29tcGFueSBUZXN0IDIiLCJTY2hlZHVsZWREYXRlIjoiMjAyNC0wOC0yMlQxNDowMDowMCIsIkdhdGVJZCI6IkdBVEUwMiIsIlNlY3VyaXR5SGFzaCI6InF3ZXJ0eXVpb3Bhc2RmZ2hqa2x6eGN2Ym5tIn0='
    ];
    
    return mockCodes[Math.floor(Math.random() * mockCodes.length)];
}

// Reproducir sonido de confirmación
window.playBeepSound = function () {
    try {
        // Crear un beep simple usando Web Audio API
        const audioContext = new (window.AudioContext || window.webkitAudioContext)();
        const oscillator = audioContext.createOscillator();
        const gainNode = audioContext.createGain();
        
        oscillator.connect(gainNode);
        gainNode.connect(audioContext.destination);
        
        oscillator.frequency.setValueAtTime(800, audioContext.currentTime);
        oscillator.type = 'sine';
        
        gainNode.gain.setValueAtTime(0.3, audioContext.currentTime);
        gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.1);
        
        oscillator.start(audioContext.currentTime);
        oscillator.stop(audioContext.currentTime + 0.1);
    } catch (error) {
        console.log('No se pudo reproducir el sonido:', error);
    }
};

// Función para generar QR real (requiere librería externa)
window.generateQRCode = function (data) {
    // En producción, usar una librería como qrcode.js
    // Por ahora, retornamos los datos codificados en Base64
    return btoa(JSON.stringify(data));
};

// Función para decodificar QR real (requiere librería externa)
window.decodeQRCode = function (qrData) {
    try {
        // Decodificar Base64
        const decoded = atob(qrData);
        return JSON.parse(decoded);
    } catch (error) {
        console.error('Error al decodificar QR:', error);
        return null;
    }
};

// Limpiar recursos al cerrar
window.cleanupQRScanner = function () {
    stopCamera();
    dotNetHelper = null;
    console.log('QR Scanner limpiado');
};

// Mostrar código QR en modal
window.showQRCode = function (qrCodeData, visitCode) {
    try {
        // Escapar caracteres especiales para evitar errores de sintaxis
        const escapedQRCode = qrCodeData.replace(/'/g, "\\'").replace(/"/g, '\\"');
        const escapedVisitCode = visitCode.replace(/'/g, "\\'").replace(/"/g, '\\"');
        
        // Crear modal para mostrar el QR
        const modalHtml = `
            <div class="modal fade" id="qrModal" tabindex="-1">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <i class="fas fa-qrcode me-2"></i>
                                Código QR - Visita ${escapedVisitCode}
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body text-center">
                            <div class="mb-3">
                                <div id="qrCodeDisplay" class="border p-3 d-inline-block">
                                    <canvas id="qrCanvas" width="200" height="200" style="display: inline-block;"></canvas>
                                </div>
                            </div>
                            <p class="text-muted small">
                                Escanea este código con el escáner QR para realizar el check-in
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            <button type="button" class="btn btn-primary" onclick="downloadQRCode('${escapedQRCode}', '${escapedVisitCode}')">
                                <i class="fas fa-download me-2"></i>Descargar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        // Remover modal existente si hay uno
        const existingModal = document.getElementById('qrModal');
        if (existingModal) {
            existingModal.remove();
        }
        
        // Agregar nuevo modal al body
        document.body.insertAdjacentHTML('beforeend', modalHtml);
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('qrModal'));
        modal.show();
        
        // Generar QR visual después de que el modal esté visible
        setTimeout(() => {
            const success = generateVisualQR(qrCodeData, 'qrCanvas');
            if (!success) {
                // Si falla el canvas, mostrar como texto
                showQRAsText(qrCodeData, 'qrCanvas');
            }
        }, 100);
        
        // Limpiar modal cuando se cierre
        document.getElementById('qrModal').addEventListener('hidden.bs.modal', function () {
            this.remove();
        });
        
    } catch (error) {
        console.error('Error al mostrar QR:', error);
        alert('Error al mostrar el código QR');
    }
};

// Generar QR visual usando una librería simple
function generateVisualQR(data, canvasId) {
    try {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.error('Canvas no encontrado:', canvasId);
            return;
        }
        
        // Verificar que sea un elemento canvas
        if (canvas.tagName !== 'CANVAS') {
            console.error('El elemento no es un canvas:', canvas.tagName);
            return;
        }
        
        // Crear un QR simple usando caracteres ASCII
        const qrSize = 200;
        canvas.width = qrSize;
        canvas.height = qrSize;
        
        const ctx = canvas.getContext('2d');
        if (!ctx) {
            console.error('No se pudo obtener el contexto 2D del canvas');
            return;
        }
        
        // Limpiar canvas
        ctx.fillStyle = 'white';
        ctx.fillRect(0, 0, qrSize, qrSize);
        
        // Generar patrón QR simple basado en los datos
        const hash = simpleHash(data);
        const pattern = generateQRPattern(hash, 25); // 25x25 matriz
        
        const cellSize = qrSize / 25;
        
        // Dibujar patrón QR
        for (let i = 0; i < 25; i++) {
            for (let j = 0; j < 25; j++) {
                if (pattern[i][j]) {
                    ctx.fillStyle = 'black';
                    ctx.fillRect(j * cellSize, i * cellSize, cellSize, cellSize);
                }
            }
        }
        
        // Agregar marco
        ctx.strokeStyle = 'black';
        ctx.lineWidth = 2;
        ctx.strokeRect(0, 0, qrSize, qrSize);
        
        console.log('QR visual generado correctamente');
        return true;
        
    } catch (error) {
        console.error('Error al generar QR visual:', error);
        // Mostrar un mensaje de error en el canvas si es posible
        try {
            const canvas = document.getElementById(canvasId);
            if (canvas && canvas.getContext) {
                const ctx = canvas.getContext('2d');
                ctx.fillStyle = 'white';
                ctx.fillRect(0, 0, 200, 200);
                ctx.fillStyle = 'red';
                ctx.font = '12px Arial';
                ctx.textAlign = 'center';
                ctx.fillText('Error al generar QR', 100, 100);
            }
        } catch (e) {
            console.error('Error al mostrar mensaje de error:', e);
        }
        return false;
    }
}

// Función alternativa para mostrar QR como texto
function showQRAsText(qrData, elementId) {
    try {
        const element = document.getElementById(elementId);
        if (!element) return;
        
        // Si es un canvas, reemplazarlo con un div
        if (element.tagName === 'CANVAS') {
            const parent = element.parentNode;
            const newDiv = document.createElement('div');
            newDiv.id = elementId;
            newDiv.style.width = '200px';
            newDiv.style.height = '200px';
            newDiv.style.border = '1px solid #ccc';
            newDiv.style.backgroundColor = 'white';
            newDiv.style.display = 'inline-block';
            newDiv.style.padding = '10px';
            newDiv.style.fontFamily = 'monospace';
            newDiv.style.fontSize = '8px';
            newDiv.style.overflow = 'auto';
            newDiv.style.wordBreak = 'break-all';
            
            // Mostrar los datos del QR
            newDiv.innerHTML = `
                <div style="text-align: center; margin-bottom: 10px;">
                    <strong>Código QR</strong><br>
                    <small>${qrData.substring(0, 50)}...</small>
                </div>
                <div style="text-align: left;">
                    <strong>Datos completos:</strong><br>
                    <pre style="margin: 0; font-size: 6px;">${qrData}</pre>
                </div>
            `;
            
            parent.replaceChild(newDiv, element);
        }
        
        console.log('QR mostrado como texto');
        
    } catch (error) {
        console.error('Error al mostrar QR como texto:', error);
    }
}

// Función hash simple para generar patrón
function simpleHash(str) {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
        const char = str.charCodeAt(i);
        hash = ((hash << 5) - hash) + char;
        hash = hash & hash; // Convertir a 32-bit integer
    }
    return Math.abs(hash);
}

// Generar patrón QR simple
function generateQRPattern(hash, size) {
    const pattern = [];
    for (let i = 0; i < size; i++) {
        pattern[i] = [];
        for (let j = 0; j < size; j++) {
            // Usar el hash para determinar si la celda debe ser negra
            const index = (i * size + j) % 32;
            const bit = (hash >> index) & 1;
            pattern[i][j] = bit === 1;
        }
    }
    return pattern;
}

// Descargar código QR
window.downloadQRCode = function (qrCodeData, visitCode) {
    try {
        // Crear archivo de texto con el código QR
        const blob = new Blob([qrCodeData], { type: 'text/plain' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `QR_${visitCode}.txt`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    } catch (error) {
        console.error('Error al descargar QR:', error);
        alert('Error al descargar el código QR');
    }
};
