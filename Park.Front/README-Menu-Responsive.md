# Park.Front - Menú Responsive y Ajustable

## 🎯 Funcionalidades Implementadas

### ✅ Menú Ajustable con Más Espacio
- **Menú Expandido**: 240px de ancho, muestra iconos + textos
- **Menú Colapsado**: 60px de ancho, muestra solo iconos
- **Contenido Principal**: Se ajusta automáticamente al ancho del menú
- **Más Espacio**: El contenido principal tiene más espacio cuando el menú está colapsado

### ✅ Comportamiento en Móviles
- **Desktop/Tablet**: Menú ajustable (240px ↔ 60px)
- **Mobile**: Menú completamente oculto, se muestra con overlay
- **Botones Diferentes**: Comportamiento específico para cada dispositivo

### ✅ Transiciones Suaves
- **Animación**: 0.3s para cambios de ancho
- **Responsive**: Funciona perfectamente en todos los dispositivos
- **Tooltips**: Informativos cuando el menú está colapsado

## 🎨 Comportamiento Visual

### Desktop - Menú Expandido (240px)
```
┌─────────────────┬──────────────────────┐
│ 🏢 Park.Front   │ Dashboard            │
│ 🏠 Home         │                      │
│ ➕ Counter      │ Contenido de la      │
│ ☁️ Weather      │ página aquí...       │
│ 👥 Usuarios     │                      │
│ 📅 Visitas      │                      │
└─────────────────┴──────────────────────┘
```

### Desktop - Menú Colapsado (60px)
```
┌─────┬──────────────────────────────────┐
│ 🏢  │ Dashboard                        │
│ 🏠  │                                 │
│ ➕  │ Contenido de la                 │
│ ☁️  │ página aquí...                  │
│ 👥  │                                 │
│ 📅  │                                 │
└─────┴──────────────────────────────────┘
```

### Mobile - Menú Oculto
```
┌────────────────────────────────────────┐
│ ☰ Dashboard                            │
│                                        │
│ Contenido de la                        │
│ página aquí...                         │
│                                        │
└────────────────────────────────────────┘
```

### Mobile - Menú Visible (Overlay)
```
┌─────────────────┬──────────────────────┐
│ 🏢 Park.Front   │ Dashboard            │
│ 🏠 Home         │                      │
│ ➕ Counter      │ Contenido de la      │
│ ☁️ Weather      │ página aquí...       │
│ 👥 Usuarios     │                      │
│ 📅 Visitas      │                      │
└─────────────────┴──────────────────────┘
```

## 🔧 Estilos CSS Implementados

### Menú Ajustable
```css
.sidebar {
    width: 240px;
    transition: width 0.3s ease, transform 0.3s ease;
}

.sidebar.collapsed {
    width: 60px;
}
```

### Contenido Principal Ajustable
```css
main {
    margin-left: 240px;
    transition: margin-left 0.3s ease;
}

main.sidebar-collapsed {
    margin-left: 60px;
}
```

### Comportamiento en Móviles
```css
@media (max-width: 768px) {
    .sidebar {
        transform: translateX(-100%);
        width: 240px !important;
    }
    
    .sidebar.show {
        transform: translateX(0);
    }
    
    main {
        margin-left: 0 !important;
    }
    
    main.sidebar-collapsed {
        margin-left: 0 !important;
    }
}
```

### Tooltips para Menú Colapsado
```css
.sidebar.collapsed .nav-link:hover::after {
    content: attr(title);
    position: absolute;
    left: 100%;
    top: 50%;
    transform: translateY(-50%);
    background-color: var(--bg-sidebar);
    color: var(--text-sidebar);
    padding: var(--spacing-xs) var(--spacing-sm);
    border-radius: var(--border-radius-sm);
    font-size: 0.875rem;
    white-space: nowrap;
    z-index: 1001;
    margin-left: var(--spacing-sm);
    box-shadow: var(--shadow-md);
    border: 1px solid #374151;
}
```

## 🚀 Cómo Funciona

### 1. Desktop/Tablet
- **Botón Toggle**: Colapsa/expande el menú (240px ↔ 60px)
- **Contenido Principal**: Se ajusta automáticamente al ancho del menú
- **Más Espacio**: El contenido tiene más espacio cuando el menú está colapsado

### 2. Mobile
- **Menú Oculto**: Por defecto, el menú está completamente oculto
- **Botón Toggle**: Muestra/oculta el menú con overlay
- **Contenido Completo**: El contenido principal ocupa toda la pantalla

### 3. Estados del Menú
- **Expandido**: Muestra iconos + textos (240px en desktop, overlay en mobile)
- **Colapsado**: Muestra solo iconos (60px en desktop, oculto en mobile)
- **Tooltips**: Al hacer hover en iconos colapsados muestra el nombre

## 🎯 Beneficios del Nuevo Diseño

1. **Más Espacio**: El contenido principal tiene más espacio cuando el menú está colapsado
2. **Responsive**: Funciona perfectamente en todos los dispositivos
3. **Ajustable**: El usuario puede elegir cuánto espacio quiere para el contenido
4. **Profesional**: Aspecto limpio y moderno
5. **Intuitivo**: Comportamiento esperado en cada dispositivo
6. **Tooltips Informativos**: Los usuarios saben qué hace cada icono

## 📱 Responsive Breakpoints

- **Desktop**: > 768px - Menú ajustable (240px ↔ 60px)
- **Tablet**: > 768px - Mismo comportamiento que desktop
- **Mobile**: ≤ 768px - Menú oculto con overlay

## 🔧 Personalización

### Cambiar Ancho del Menú Colapsado
```css
.sidebar.collapsed {
    width: 50px; /* Cambia este valor */
}

main.sidebar-collapsed {
    margin-left: 50px; /* Debe coincidir con el ancho */
}
```

### Cambiar Velocidad de Transición
```css
.sidebar {
    transition: width 0.5s ease; /* Cambia 0.3s por el valor deseado */
}
```

### Cambiar Breakpoint para Mobile
```css
@media (max-width: 992px) { /* Cambia 768px por el valor deseado */
    /* Estilos para mobile */
}
```

## 🎨 Iconos y Botones

### Botones Toggle
- **Desktop**: Botón para colapsar/expandir menú
- **Mobile**: Botón para mostrar/ocultar menú
- **Icono**: Tres líneas clásicas (hamburguesa)
- **Ubicación**: Header del sidebar y header superior

### Tooltips
- **Activación**: Hover en iconos cuando el menú está colapsado
- **Contenido**: Nombre del elemento del menú
- **Diseño**: Fondo oscuro con bordes y sombra
- **Posición**: A la derecha del icono
