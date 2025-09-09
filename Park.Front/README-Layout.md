# Park.Front - Layout y Navegación

## 🎯 Funcionalidades Implementadas

### ✅ Botón Toggle del Sidebar
- **Ubicación**: En el header del sidebar y en el header superior
- **Funcionalidad**: Permite ocultar/mostrar el menú lateral
- **Iconos**: Cambia entre `bi-chevron-left` y `bi-chevron-right`

### ✅ Soporte para Imagen de Logo
- **Propiedad**: `LogoImageUrl` en el MainLayout
- **Ubicación**: Reemplaza el icono `bi-building` por una imagen
- **Tamaño**: 32x32px con bordes redondeados
- **Formato**: Soporta SVG, PNG, JPG

### ✅ Alineación Mejorada de Iconos
- **Iconos del menú**: Perfectamente alineados con el texto
- **Tamaño**: 18px con centrado perfecto
- **Espaciado**: Consistente en todos los elementos

## 🚀 Cómo Usar

### 1. Usar Imagen de Logo

```razor
<!-- En App.razor o donde uses MainLayout -->
<MainLayout LogoImageUrl="images/mi-logo.png" />
```

### 2. Personalizar el Logo

1. Coloca tu imagen en `wwwroot/images/`
2. Actualiza la propiedad `LogoImageUrl` en `MainLayout.razor`
3. O usa el `CustomMainLayout.razor` que ya tiene un ejemplo

### 3. Ejemplo de Uso

```razor
@page "/"
@layout CustomMainLayout

<h1>Mi Página</h1>
```

## 📁 Archivos Modificados

- ✅ `Layout/MainLayout.razor` - Layout principal con toggle y soporte de imagen
- ✅ `Layout/CustomMainLayout.razor` - Ejemplo con logo personalizado
- ✅ `wwwroot/css/modern-minimalist.css` - Estilos mejorados
- ✅ `wwwroot/images/logo-placeholder.svg` - Logo de ejemplo

## 🎨 Estilos CSS Agregados

```css
/* Botón toggle */
.navbar-toggler {
    margin-left: auto;
    color: var(--text-sidebar);
}

/* Logo del navbar */
.navbar-brand .logo {
    width: 32px;
    height: 32px;
    border-radius: var(--border-radius-sm);
}

/* Sidebar colapsado */
.sidebar.collapsed {
    transform: translateX(-100%);
}

/* Iconos alineados */
.nav-link .bi {
    display: inline-flex;
    align-items: center;
    justify-content: center;
}
```

## 🔧 Personalización

### Cambiar Colores
Modifica las variables CSS en `modern-minimalist.css`:

```css
:root {
    --primary-color: #1e40af;
    --bg-sidebar: #1f2937;
    --text-sidebar: #d1d5db;
}
```

### Cambiar Tamaño del Sidebar
```css
.sidebar {
    width: 240px; /* Cambia este valor */
}
```

### Agregar Más Elementos al Menú
Edita `Layout/NavMenu.razor`:

```razor
<div class="nav-item">
    <NavLink class="nav-link" href="nueva-pagina">
        <i class="bi bi-nuevo-icono"></i>
        Nueva Página
    </NavLink>
</div>
```
