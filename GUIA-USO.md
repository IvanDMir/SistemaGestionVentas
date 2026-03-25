# Cómo usar la aplicación — guía rápida

Sistema de **gestión de ventas e inventario** en tu PC: un solo usuario, datos guardados en un archivo SQLite local.

---

## 1. Cómo abrir la aplicación

### Desde Visual Studio

1. Abrí la solución `SistemaGestionVentas.sln`.
2. Asegurate de que el proyecto de inicio sea **SistemaGestionVentas** (clic derecho en el proyecto → *Establecer como proyecto de inicio* si hace falta).
3. Presioná **F5** (depurar) o **Ctrl+F5** (sin depurar).

### Desde la terminal (si tenés el SDK de .NET 8)

```powershell
cd ruta\a\SistemaGestionVentas\SistemaGestionVentas
dotnet run
```

La primera vez que arranca, la app **crea sola** la base de datos si no existe (en la carpeta de datos locales, ver abajo).

---

## 2. Dónde se guardan tus datos

- El archivo de base de datos se llama **`ventas.db`**.
- Está en tu usuario de Windows, carpeta de datos de aplicación local, por ejemplo:
  - `%LocalAppData%\SistemaGestionVentas\ventas.db`
- **No** está dentro de la carpeta del código fuente: así podés copiar el proyecto sin mezclar tus datos de prueba con el repositorio.

Si borrás esa carpeta o el archivo `.db`, empezás de cero (productos y movimientos nuevos).

---

## 3. Pantalla principal

Verás **tres botones**:

| Botón | Para qué sirve |
|--------|----------------|
| **Productos** | Dar de alta, editar o eliminar productos y ver el stock actual. |
| **Movimientos de inventario** | Registrar entradas, salidas, ventas y ajustes de stock. |
| **Reportes** | Ver stock bajo, valor del inventario, qué se vende más, historial y márgenes. |

Los formularios se abren en ventana propia; al cerrarlos volvés al menú.

---

## 4. Primeros pasos recomendados

### Paso 1 — Crear productos

1. Clic en **Productos**.
2. **Nuevo**: completá al menos **Nombre**, **precio de compra**, **precio de venta** y, si querés, **stock inicial** y **stock mínimo** (para alertas).
3. **SKU** es opcional (código interno o de barras).
4. **Aceptar** para guardar.

**Editar**: seleccioná una fila y **Editar**. El **stock** no se cambia ahí: solo con movimientos.

**Eliminar**: solo podés borrar un producto que **no tenga** movimientos registrados.

### Paso 2 — Registrar movimientos

1. Clic en **Movimientos de inventario**.
2. Elegí **Producto** y **Tipo**:
   - **Entrada** — compra o ingreso de mercadería (suma stock).
   - **Salida** — egreso que no es venta (resta stock).
   - **Venta** — venta real (resta stock y guarda el **precio unitario** de esa venta).
   - **Ajuste + / −** — correcciones de inventario (conte físico, errores, etc.).
3. **Cantidad** siempre es un número positivo; el tipo indica si suma o resta.
4. En **Venta**, podés cambiar el precio unitario (por defecto toma el precio de lista del producto).
5. **Registrar**.

Si intentás vender o sacar más unidades de las que hay, la app te avisará que no hay stock suficiente.

### Paso 3 — Ver reportes

1. Clic en **Reportes**.
2. Revisá las pestañas:
   - **Stock bajo** — productos en o por debajo del mínimo que definiste.
   - **Valor inventario** — suma aproximada *stock × costo de compra*.
   - **Más vendidos** — unidades vendidas por producto.
   - **Más movidos (egresos)** — salidas + ventas + ajustes negativos.
   - **Historial** — elegí producto (o “Todos”), fechas **Desde / Hasta** y **Aplicar filtros**.
   - **Margen / ROI** — por producto, según ventas registradas (ingresos, costo al momento de cada venta, beneficio y % sobre costo).
3. **Actualizar todo** recalcula todas las pestañas con los datos actuales.

---

## 5. Consejos prácticos

- Usá **decimal** en la cabeza para dinero: la app ya trabaja con precisión adecuada en pantalla.
- Si cambiás el **costo** de un producto después, los **movimientos viejos** siguen usando el costo que quedó guardado en cada venta (**instantánea**), para que los reportes de margen tengan sentido histórico.
- Hacé **copias de seguridad** de `ventas.db` si los datos te importan (copiá el archivo a otro disco o nube).

---

## 6. Si algo no abre o falla

- Comprobá que tengas **.NET 8** y el workload de **desarrollo de escritorio .NET** en Visual Studio.
- Si la app no compila, en Visual Studio: menú **Compilar → Compilar solución** y leé el mensaje de error en la lista de errores.

Para detalles técnicos del código (capas, servicios, base de datos), mirá **`DOCUMENTACION.md`**.
