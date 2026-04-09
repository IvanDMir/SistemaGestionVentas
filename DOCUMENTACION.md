# Documentación — Sistema de gestión de ventas

Aplicación de **escritorio para un solo usuario**: **Windows Forms** (.NET 8), datos locales en **SQLite** mediante **Entity Framework Core**. Incluye **productos** (con línea/grupo, variante, imagen), **movimientos de inventario** (compras con recálculo de costo promedio, ventas, ajustes) y **reportes** (stock bajo, valor de inventario, rankings, historial, margen y ROI).

Este documento sirve para **estudiar el proyecto**: organización del código, **qué hace cada pieza** y **cómo encajan las funciones** entre sí.

---

## 1. Estructura de carpetas y archivos

```
SistemaGestionVentas/                    ← raíz de la solución (repo Git recomendado aquí)
├── .gitignore
├── DOCUMENTACION.md                     ← arquitectura y referencia técnica (este archivo)
├── GUIA-USO.md                          ← uso de la app paso a paso
├── PLAN-MANTENIMIENTO-Y-APRENDIZAJE.md  ← hábitos, mejoras y plan de estudio
├── publicar.bat / git-push.bat          ← scripts opcionales (si los tenés en el repo)
├── SistemaGestionVentas.sln
└── SistemaGestionVentas/                ← proyecto WinForms
    ├── Program.cs                       ← arranque, BD, migraciones
    ├── Form1.cs / Form1.Designer.cs     ← menú principal
    ├── Data/
    │   ├── AppDbContext.cs
    │   └── AppDbContextFactory.cs       ← diseño de migraciones (`dotnet ef`)
    ├── Models/
    │   ├── ProductGroup.cs
    │   ├── Product.cs
    │   ├── StockMovement.cs
    │   └── MovementType.cs
    ├── Services/
    │   ├── ProductService.cs
    │   ├── InventoryService.cs
    │   └── ReportingService.cs
    ├── UI/
    │   └── UiTheme.cs                   ← colores, estilos de botones y grillas
    ├── Forms/
    │   ├── FormProductos.cs (+ Designer)      ← listado + acciones
    │   ├── FormProductoEdit.cs (+ Designer)   ← alta/edición de producto
    │   ├── FormMovimientos.cs (+ Designer)
    │   └── FormReportes.cs (+ Designer)
    ├── Migrations/                      ← historial de esquema EF Core
    └── SistemaGestionVentas.csproj
```

**Base de datos en ejecución** (no va al repo):

`%LocalAppData%\SistemaGestionVentas\ventas.db`

---

## 2. Arquitectura en capas

| Capa | Ubicación | Responsabilidad |
|------|-----------|-----------------|
| **Modelos** | `Models/` | Entidades y enums; forma de los datos en BD. |
| **Datos** | `Data/` | `AppDbContext`: `DbSet<>`, relaciones, precisión decimal, reglas `OnDelete`. |
| **Servicios** | `Services/` | Reglas de negocio: CRUD productos/grupos, movimientos con transacción, reportes. |
| **Presentación** | `Form1`, `Forms/`, `UI/UiTheme.cs` | Ventanas y estilos. Los formularios **llaman a servicios**; no usan `DbContext` directamente. |

Flujo típico:

```text
Usuario → Formulario → Servicio → AppDbContext → SQLite
```

**Patrón de servicios:** reciben `DbContextOptions<AppDbContext>` (guardadas en `Program.DbContextOptions`) y en cada operación hacen `using var ctx = new AppDbContext(_options)` para un contexto corto y descartable.

---

## 3. Punto de entrada: `Program.cs`

| Elemento | Para qué sirve |
|----------|----------------|
| `[STAThread]` | Hilo STA requerido por WinForms. |
| `ApplicationConfiguration.Initialize()` | DPI / fuentes del proyecto moderno. |
| Ruta SQLite bajo `LocalApplicationData\SistemaGestionVentas` | Datos por usuario de Windows, fuera del código fuente. |
| `DbContextOptions<AppDbContext>` en `Program.DbContextOptions` | Una sola configuración de conexión para toda la app. |
| `db.Database.Migrate()` al iniciar | Aplica migraciones pendientes; crea o actualiza tablas. |
| `Application.Run(new Form1())` | Menú principal. |

---

## 4. Modelos (`Models/`)

### 4.1 `ProductGroup`

Agrupa productos en una **línea** o familia (ej. “Rompecabezas”). Es opcional: un producto puede no tener grupo (`ProductGroupId` null).

| Propiedad | Significado |
|-----------|-------------|
| `Id` | Clave primaria. |
| `Name` | Nombre del grupo (único en la práctica por resolución en `ProductService.ResolveProductGroupId`). |
| `Products` | Colección de productos (navegación EF). |

### 4.2 `Product`

| Propiedad | Significado |
|-----------|-------------|
| `Id` | Clave primaria. |
| `ProductGroupId` / `ProductGroup` | Línea opcional. |
| `Name` | Nombre del artículo dentro de la línea. |
| `VariantLabel` | Modelo o variante (ej. “1000 piezas paisaje”). |
| `ImageData` | BLOB opcional (PNG/JPEG/etc.) en SQLite. |
| `CostPrice` | **Costo promedio ponderado** del stock actual; se recalcula en cada **Compra / ingreso** (`MovementType.Entrada`). También se puede editar manualmente en el formulario de producto (no pasa por movimiento). |
| `ListSalePrice` | Precio de venta de lista (referencia; las ventas pueden usar otro precio unitario). |
| `StockQuantity` | Stock actual; cambia con movimientos y con el stock inicial al crear. |
| `MinStockThreshold` | Umbral para reporte de stock bajo. |
| `Movements` | Historial de movimientos. |

### 4.3 `StockMovement`

| Propiedad | Significado |
|-----------|-------------|
| `ProductId` / `Product` | Producto afectado. |
| `Type` | `MovementType`. |
| `Quantity` | Siempre positiva; el signo en stock lo define el tipo. |
| `UnitSalePrice` | Precio unitario de **venta** (solo tiene sentido en `Venta`; en otros suele ser 0). |
| `UnitCostSnapshot` | En **Venta**: costo promedio del producto al momento de la venta (histórico para márgenes). En **Entrada**: costo unitario **de esa compra**. En ajustes: costo promedio vigente. |
| `OccurredAt` | Fecha/hora (`DateTime.Now` al registrar). |
| `Note` | Texto opcional. |

### 4.4 `MovementType` (enum)

| Valor | Stock | Precio en formulario | Efecto en `CostPrice` del producto |
|-------|-------|----------------------|-----------------------------------|
| `Entrada` | +cantidad | **Costo unitario de esta compra** | **Recalcula promedio ponderado** (ver §6.2). |
| `Salida` | −cantidad | No aplica (0) | No cambia. |
| `Venta` | −cantidad | Precio unitario de venta | No cambia (usa snapshot de costo para margen). |
| `AjustePositivo` | +cantidad | No aplica | No cambia (mantiene el mismo costo promedio). |
| `AjusteNegativo` | −cantidad | No aplica | No cambia. |

---

## 5. Acceso a datos: `AppDbContext` y migraciones

### 5.1 `AppDbContext`

- `DbSet<ProductGroup>`, `DbSet<Product>`, `DbSet<StockMovement>`.
- Decimales con `HasPrecision(18, 2)` donde corresponde (dinero).
- `Product` → `ProductGroup`: FK opcional, `OnDelete(SetNull)` si se borrara el grupo (no implementado en UI).
- `StockMovement` → `Product`: `OnDelete(Restrict)` (no borrar producto con movimientos desde BD; el servicio también valida).

### 5.2 `AppDbContextFactory`

Implementa `IDesignTimeDbContextFactory<AppDbContext>` para `dotnet ef migrations add ...` sin ejecutar la aplicación.

### 5.3 Migraciones relevantes (orden)

1. `InitialCreate` — tablas base.  
2. `ReplaceSkuWithProductImage` — imagen en producto (BLOB).  
3. `ProductGroupsVariantsAndPurchaseAvg` — grupos, variante, y documentación de costo como promedio de compras.

`Program.cs` ejecuta `Migrate()` sobre `ventas.db` real.

---

## 6. Servicios (`Services/`)

### 6.1 `ProductService`

| Método | Comportamiento |
|--------|----------------|
| `GetAllGroups` | Lista de `ProductGroup` ordenados por nombre (`AsNoTracking`). |
| `GetAll` | Productos con `Include(ProductGroup)`, orden por grupo, nombre y variante. |
| `GetById` | Un producto con grupo, o `null`. |
| `ResolveProductGroupId(newGroupName, selectedGroupId)` | Si `newGroupName` tiene texto: busca grupo ignorando mayúsculas o **crea** uno nuevo y devuelve su `Id`. Si no, devuelve `selectedGroupId` (puede ser `null`). |
| `Create(...)` | Valida; crea producto con stock inicial, imagen clonada, grupo y variante. |
| `Update(...)` | Actualiza datos del producto **excepto** `StockQuantity` (eso solo por movimientos). Incluye `CostPrice` manual si el usuario lo cambia en el editor. |
| `Delete` | Solo si no hay movimientos para ese producto. |

Validaciones privadas: nombre obligatorio, precios no negativos, umbral mínimo ≥ 0.

### 6.2 `InventoryService` — `RegisterMovement`

Abre **transacción** (`BeginTransaction`): carga el producto, calcula **delta** de stock, evita stock negativo, actualiza campos según el tipo, inserta `StockMovement`, `SaveChanges`, `Commit`; ante error hace `Rollback`.

**Costo promedio en `Entrada`** (promedio ponderado):

- Si el stock actual `sIn` es **0** o menos (caso borde): `CostPrice = precio unitario de la compra`.
- Si no:  
  `CostPrice = (sIn × CostPrice_actual + cantidad × precio_compra) / (sIn + cantidad)`  

donde `precio_compra` es el parámetro `unitPrice` del movimiento.

En código (`InventoryService.cs`, rama `MovementType.Entrada`): se guarda en el movimiento `UnitCostSnapshot = precio de esa compra` (no el promedio), para dejar constancia histórica del lote.

### 6.3 `ReportingService`

Define **records** tipo fila de grilla (`LowStockRow`, `MovementStatRow`, `MovementHistoryRow`, `MarginRow`, etc.).

| Método | Idea |
|--------|------|
| `GetLowStock` | `StockQuantity <= MinStockThreshold`. |
| `GetInventoryTotalValue` | Valorización a costo: suma de `StockQuantity × CostPrice` (agregación en cliente donde hace falta por limitaciones de traducción SQL en SQLite). |
| `GetTopSold` | Agrupa `Venta` por producto. |
| `GetTopOutgoingMovementVolume` | Egresos: `Salida`, `Venta`, `AjusteNegativo`. |
| `GetMovementHistory` | Filtros por producto y fechas; etiquetas de tipo en español. |
| `GetMarginByProduct` | Por ventas: ingresos con `UnitSalePrice`, costo con `UnitCostSnapshot`; beneficio y ROI % sobre costo. |

Algunas consultas usan tipos anónimos + materialización (`ToList`) y proyección en memoria para compatibilidad con EF Core y SQLite.

---

## 7. Interfaz de usuario

### 7.1 `UiTheme` (`UI/UiTheme.cs`)

Centraliza colores, fuentes y métodos como `ApplyForm`, `StylePrimaryButton`, `StyleDataGridView`, etc., para que los formularios compartan la misma apariencia.

### 7.2 `Form1`

Tres botones modales: **Productos**, **Movimientos de inventario**, **Reportes**. Cada formulario crea sus servicios con `Program.DbContextOptions` (o constructores por defecto equivalentes).

### 7.3 `FormProductos` y `FormProductoEdit`

**`FormProductos`**

- Contenedor **`TableLayoutPanel`**: fila superior la grilla (`Dock Fill`), fila inferior barra de botones (altura fija), para que al maximizar la ventana la tabla use todo el espacio.
- **`DataGridView`**: `AutoSizeColumnsMode = Fill` con `FillWeight` y `MinimumWidth` por columna para repartir el ancho sin huecos raros.
- Columnas: foto miniatura, id, línea (desde `CellFormatting` y grupo), nombre, modelo, costos, stock, mínimo.
- Botones: **Nuevo**, **Editar**, **Eliminar**, **Compra / ingreso** (abre `FormMovimientos` con el producto seleccionado y tipo `Entrada`), **Cerrar**.

**`FormProductoEdit`**

- Campos: nombre, combo de línea/grupo, texto opcional “nuevo grupo”, variante, imagen (`PictureBox`), costo promedio, precio venta, stock inicial (solo alta) o stock actual (solo lectura en edición), mínimo alerta.
- Imagen: **Cargar…** (archivo), **Pegar** / **Ctrl+V** cuando el foco no está en un `TextBox`/`ComboBox` (portapapeles → PNG en memoria, límite de tamaño como archivo).
- Al aceptar, `FormProductos` llama a `ProductService` con `ResolveProductGroupId`.

### 7.4 `FormMovimientos`

- Combos: producto (texto formateado con línea y variante), tipo de movimiento con textos explícitos (compra recalcula costo; ajuste positivo aclara que no cambia el promedio).
- **Cantidad** y, si aplica, **precio unitario**: en **Venta** = precio de venta; en **Compra / ingreso** = costo unitario de esa compra (por defecto sugiere el costo promedio actual del producto).
- Tras registrar una **Entrada**, el mensaje muestra **costo promedio actual** y **stock** actualizado.

**Constructores útiles para estudio y reutilización:**

- `FormMovimientos()` — uso normal desde el menú.
- `FormMovimientos(preselectProductId, initialMovementType)` — usado desde **Compra / ingreso** en productos: abre con producto y tipo ya elegidos.
- `FormMovimientos(productService, inventoryService, ...)` — pruebas o extensiones.

### 7.5 `FormReportes`

`TabControl` con pestañas que consumen `ReportingService` (stock bajo, valor inventario, rankings, historial con filtros, margen/ROI). Botón **Actualizar todo**.

---

## 8. Flujos que conviene entender al estudiar

### 8.1 Alta de producto con stock inicial

`FormProductoEdit` → `ProductService.Create` → una fila en `Products` con `StockQuantity = initialStock`. **No** se crea movimiento; el stock “nace” en el producto.

### 8.2 Compra posterior con nuevo precio

`FormMovimientos` con tipo **Compra / ingreso** → `InventoryService.RegisterMovement(Entrada, cantidad, costoUnitarioCompra, ...)` → suma stock y **actualiza `CostPrice`** con la fórmula del §6.2 → fila en `StockMovements` con `UnitCostSnapshot = costo de esa compra`.

### 8.3 Venta y margen

`RegisterMovement(Venta, ..., precioVenta, ...)` → resta stock; `UnitCostSnapshot` en el movimiento es el costo promedio **antes** de aplicar esa venta (en el código actual se lee del producto ya cargado). Los reportes de margen usan esas instantáneas por línea de venta.

---

## 9. Dependencias NuGet (resumen)

- `Microsoft.EntityFrameworkCore.Sqlite`
- `Microsoft.EntityFrameworkCore.Design` (herramientas CLI `dotnet ef`)

---

## 10. Git y datos

Convéniente un **único** `.git` en la carpeta del proyecto (no en todo el usuario de Windows). **`.gitignore`**: `bin/`, `obj/`, `.vs/`, etc.

No versionar `ventas.db` de uso diario. Sí versionar **código** y **migraciones**.

---

## 11. Cómo extender (ideas)

- Exportar grillas a CSV.
- Proveedor o etiquetas adicionales (nueva entidad + migración).
- Inyección de dependencias para servicios.
- Pruebas unitarias de `InventoryService` (cálculo de promedio ponderado).

---

## 12. Glosario rápido

| Término | Significado breve |
|---------|-------------------|
| **EF Core** | ORM: clases C# ↔ tablas; LINQ traducido a SQL. |
| **`DbContext`** | Unidad de trabajo: acceso a tablas y `SaveChanges`. |
| **Migración** | Cambio versionado del esquema. |
| **`AsNoTracking()`** | Lectura sin seguimiento (listados). |
| **Transacción** | Todo confirma (`Commit`) o nada (`Rollback`). |
| **DTO / record** | Objetos solo para mostrar datos en UI (reportes). |
| **Promedio ponderado de costo** | Al ingresar mercadería a distinto precio, el costo del producto es el promedio del valor total del stock dividido por las unidades. |

---

## 13. Orden sugerido para leer el código (estudio)

1. `Program.cs`  
2. `Models/*.cs`  
3. `Data/AppDbContext.cs`  
4. `Services/InventoryService.cs` (transacción + fórmula de entrada)  
5. `Services/ProductService.cs`  
6. `Services/ReportingService.cs` (una consulta a la vez)  
7. `Forms/FormMovimientos.cs` (eventos → servicio)  
8. `Forms/FormProductos.cs` + `FormProductoEdit.cs`  
9. `UI/UiTheme.cs`

Para uso cotidiano de la aplicación, leé **`GUIA-USO.md`**.

---

*Actualizá este archivo cuando cambien reglas de negocio, modelos o pantallas importantes.*
