# Cómo usar la aplicación — guía de uso y estudio

Sistema de **gestión de ventas e inventario** en tu PC: un solo usuario, datos en **SQLite** local. Esta guía mezcla **pasos prácticos** y **referencias a qué parte del código** implementa cada cosa (para cruzar con `DOCUMENTACION.md`).

---

## 1. Cómo abrir la aplicación

### Desde Visual Studio

1. Abrí la solución `SistemaGestionVentas.sln`.
2. Proyecto de inicio: **SistemaGestionVentas**.
3. **F5** (depurar) o **Ctrl+F5** (sin depurar).

### Desde la terminal (.NET 8)

```powershell
cd ruta\a\SistemaGestionVentas\SistemaGestionVentas
dotnet run
```

La primera vez, la app **crea** la base si no existe (ver §2).

**Código relacionado:** `Program.cs` (`Migrate()`, ruta del `.db`).

---

## 2. Dónde se guardan tus datos

| Qué | Dónde |
|-----|--------|
| Archivo SQLite | `%LocalAppData%\SistemaGestionVentas\ventas.db` |
| ¿En el repo? | **No** — así podés copiar el código sin tus datos de prueba. |

Si borrás ese archivo o carpeta, empezás de cero.

**Copias de seguridad:** copiá `ventas.db` a otro lugar cuando te importe el contenido.

---

## 3. Pantalla principal (`Form1`)

| Botón | Función |
|--------|---------|
| **Productos** | Altas, ediciones, baja (si no hay movimientos), grilla con foto y datos; **compra rápida** desde la barra inferior. |
| **Movimientos de inventario** | Entradas de compra (con costo), salidas, ventas, ajustes. |
| **Reportes** | Stock bajo, valor inventario, rankings, historial, margen/ROI. |

Los formularios son **modales** (`ShowDialog`): al cerrarlos volvés al menú.

---

## 4. Productos — qué podés hacer

### 4.1 Grilla

- Muestra **foto**, **id**, **línea** (grupo), **nombre**, **modelo/variante**, **costo promedio**, **precio venta**, **stock**, **mínimo**.
- La tabla **se estira** con la ventana (no queda “tirada” a un costado).
- **Seleccioná una fila** para Editar, Eliminar o **Compra / ingreso**.

### 4.2 Nuevo producto (`FormProductoEdit`)

1. **Nombre** (obligatorio).
2. **Línea / grupo**: elegí del combo o escribí un **nuevo grupo** en el campo de texto (si hay texto, tiene prioridad y se crea o reutiliza el grupo).
3. **Modelo / variante** (opcional): ej. “1000 piezas paisaje”.
4. **Imagen** (opcional):
   - **Cargar…** — elegí archivo de imagen.
   - **Pegar** o **Ctrl+V** (con el foco fuera de los cuadros de texto) — pegá una imagen del **portapapeles** (se guarda como PNG en la base, con límite de tamaño).
   - **Quitar imagen** — borra la foto del producto.
5. **Costo prom.**: es el **costo promedio** que mostrará el sistema; al crear un producto es el valor inicial. Después se puede **recalcul automáticamente** con cada **compra / ingreso** (movimiento tipo entrada).
6. **Precio de venta** (lista).
7. **Stock inicial** (solo en alta): unidades al crear el producto (no genera un movimiento en el historial).
8. **Stock mínimo alerta**: umbral para el reporte de stock bajo.

**Aceptar** guarda con `ProductService.Create` / `ResolveProductGroupId`.

### 4.3 Editar

Igual que arriba, pero **el stock no se edita** en esta pantalla: solo con **movimientos**. Verás el stock actual como referencia.

**Código:** `FormProductoEdit.cs`, `FormProductos.cs`, `ProductService.cs`.

### 4.4 Eliminar

Solo si el producto **no tiene movimientos**. Si tiene, el sistema lo impedirá (integridad del historial).

### 4.5 Compra / ingreso (desde Productos)

1. Seleccioná un producto en la grilla.
2. Clic en **Compra / ingreso**.
3. Se abre **Movimientos de inventario** con ese producto y el tipo **Compra / ingreso** ya elegidos.
4. Ingresá **cantidad** y **costo unitario de esa compra** (el precio al que compraste **esa** tanda).
5. **Registrar**. El mensaje te mostrará el **nuevo costo promedio** y el **stock** actual.

**Por qué sirve:** cada compra a distinto precio **actualiza el costo promedio ponderado** del producto (ver fórmula en `DOCUMENTACION.md` §6.2 y `InventoryService.cs`).

---

## 5. Movimientos de inventario

1. Elegí **producto** y **tipo**:

| Tipo en pantalla | Qué hace |
|------------------|----------|
| **Compra / ingreso (suma stock y recalcula costo promedio)** | Suma stock. Pedís el **costo unitario de esa compra**. Actualiza el costo promedio del producto. |
| **Salida (sin venta)** | Resta stock (egreso que no es venta). |
| **Venta** | Resta stock. Pedís **precio unitario de venta** (por defecto el precio de lista). |
| **Ajuste: sumar stock (mismo costo promedio)** | Suma unidades **sin** cambiar el costo promedio (conteo, corrección). |
| **Ajuste: restar stock** | Resta unidades sin tratarlas como venta. |

2. **Cantidad** siempre positiva; el tipo define si suma o resta.
3. **Nota** opcional.
4. **Registrar**.

Si no hay stock suficiente para una salida/venta/ajuste negativo, verás un error.

**Código principal:** `FormMovimientos.cs` → `InventoryService.RegisterMovement`.

**Constructor con producto preseleccionado:** al abrir desde **Compra / ingreso** en productos se usa `new FormMovimientos(idProducto, MovementType.Entrada)` (ver `DOCUMENTACION.md` §7.4).

---

## 6. Reportes

1. Abrí **Reportes**.
2. Pestañas:
   - **Stock bajo** — en o por debajo del mínimo configurado.
   - **Valor inventario** — aproximación **stock × costo promedio actual**.
   - **Más vendidos** — unidades vendidas por producto.
   - **Más movidos (egresos)** — salidas + ventas + ajustes negativos.
   - **Historial** — filtro por producto y fechas **Desde / Hasta** → **Aplicar filtros**.
   - **Margen / ROI** — según ventas registradas (usa el costo **guardado en cada venta**, no el costo actual si cambió después).
3. **Actualizar todo** refresca todas las pestañas.

**Código:** `FormReportes.cs`, `ReportingService.cs`.

---

## 7. Ideas para estudiar leyendo el código

| Pregunta | Dónde mirar |
|----------|-------------|
| ¿Cómo se calcula el costo después de una compra? | `InventoryService.cs`, rama `MovementType.Entrada`. |
| ¿Cómo se crea o elige un grupo de producto? | `ProductService.ResolveProductGroupId`. |
| ¿Por qué el margen no “se mueve” si cambio el costo hoy? | Las ventas guardan `UnitCostSnapshot` en cada movimiento; el reporte usa eso. |
| ¿Cómo se arma la grilla de productos? | `FormProductos.RefreshGrid` (columnas, `Fill`, miniaturas en `CellFormatting`). |
| ¿Dónde está el tema visual? | `UI/UiTheme.cs`. |

Lista más larga de archivos y orden de lectura: **`DOCUMENTACION.md` §13**.

---

## 8. Consejos prácticos

- **Dinero:** la app usa `decimal` internamente; en pantalla verás formato con coma decimal según Windows.
- **Costo manual vs compras:** podés cambiar el costo en **Editar producto**, pero lo habitual para reflejar compras reales es usar **Compra / ingreso** con el precio de cada tanda.
- **Backups:** copiá `ventas.db` antes de experimentos con código o migraciones.

---

## 9. Si algo no compila o no abre

- Comprobá **.NET 8** y workload **Desarrollo de escritorio .NET** en Visual Studio.
- **Compilar solución** y revisar la lista de errores.

**Detalle técnico (arquitectura, servicios, modelos):** `DOCUMENTACION.md`.  
**Hábitos, mejoras futuras y plan por semanas:** `PLAN-MANTENIMIENTO-Y-APRENDIZAJE.md`.
