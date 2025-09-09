# Park.Front - Menú Colapsado

## 🎯 Funcionalidades Implementadas

### ✅ Menú Colapsado con Solo Iconos
- **Estado Expandido**: Muestra iconos + texto (240px de ancho)
- **Estado Colapsado**: Muestra solo iconos (60px de ancho)
- **Transición**: Animación suave de 0.3s
- **Tooltips**: Al hacer hover sobre iconos colapsados muestra el nombre

### ✅ Espaciado Corregido
- **Menú Expandido**: Contenido principal con margen izquierdo de 240px
- **Menú Colapsado**: Contenido principal con margen izquierdo de 60px
- **Sin Espacios**: Eliminado el espacio entre menú y cuerpo

### ✅ Botones Toggle Mejorados
- **Ubicación 1**: En el header del sidebar
- **Ubicación 2**: En el header superior del contenido
- **Iconos**: Cambian entre `bi-chevron-left` y `bi-chevron-right`
- **Tooltips**: Muestran "Expandir menú" o "Colapsar menú"

## 🎨 Comportamiento Visual

### Menú Expandido (240px)
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

### Menú Colapsado (60px)
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

## 🔧 Estilos CSS Implementados

### Transiciones Suaves
```css
.sidebar {
    transition: width 0.3s ease, transform 0.3s ease;
}
```

### Menú Colapsado
```css
.sidebar.collapsed {
    width: 60px;
}

.sidebar.collapsed .brand-text {
    display: none;
}

.sidebar.collapsed .nav-link {
    justify-content: center;
    padding: var(--spacing-sm);
}

.sidebar.collapsed .nav-link span {
    display: none;
}
```

### Tooltips Personalizados
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

### Márgenes del Contenido
```css
main {
    margin-left: 240px;
    transition: margin-left 0.3s ease;
}

main.sidebar-collapsed {
    margin-left: 60px;
}
```

## 🚀 Cómo Usar

### 1. Toggle del Menú
- **Click en el botón del sidebar**: Colapsa/expande el menú
- **Click en el botón del header**: Colapsa/expande el menú
- **Hover en iconos colapsados**: Muestra tooltip con el nombre

### 2. Personalización

#### Cambiar Ancho del Menú Colapsado
```css
.sidebar.collapsed {
    width: 50px; /* Cambia este valor */
}

main.sidebar-collapsed {
    margin-left: 50px; /* Debe coincidir con el ancho */
}
```

#### Cambiar Velocidad de Transición
```css
.sidebar {
    transition: width 0.5s ease; /* Cambia 0.3s por el valor deseado */
}
```

#### Personalizar Tooltips
```css
.sidebar.collapsed .nav-link:hover::after {
    background-color: #your-color;
    color: #your-text-color;
    font-size: 0.8rem; /* Cambia el tamaño */
}
```

## 📱 Responsive Design

El menú colapsado funciona perfectamente en dispositivos móviles:
- **Desktop**: Menú colapsado a 60px
- **Tablet**: Menú colapsado a 60px
- **Mobile**: Menú completamente oculto (transform: translateX(-100%))

## 🎯 Beneficios

1. **Más Espacio**: El contenido principal tiene más espacio cuando el menú está colapsado
2. **Navegación Rápida**: Los iconos son reconocibles y fáciles de usar
3. **Tooltips Informativos**: Los usuarios saben qué hace cada icono
4. **Transiciones Suaves**: Experiencia de usuario fluida
5. **Diseño Limpio**: Interfaz más minimalista y profesional
