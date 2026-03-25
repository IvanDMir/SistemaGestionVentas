# Documentación — Sistema de gestión de ventas

Aplicación de **escritorio para un solo usuario**: **Windows Forms** (.NET 8), datos locales en **SQLite** mediante **Entity Framework Core**. Incluye **productos**, **movimientos de inventario** y **reportes** (stock bajo, valor de inventario, ranking de ventas/movimientos, historial, margen y ROI por producto).

Este documento resume **qué se implementó**, **cómo está organizado el código** y **qué hace cada parte importante**.

---

## 1. Estructura de carpetas y archivos

```
SistemaGestionVentas/                 ← raíz de la solución (también el repo Git recomendado)
├── .gitignore                        ← ignora bin/, obj/, .vs/, etc.
├── DOCUMENTACION.md                  ← este archivo
├── SistemaGestionVentas.sln        ← solución de Visual Studio
└── SistemaGestionVentas/            ← proyecto WinForms
    ├── Program.cs                   ← arranque, BD, migraciones
    ├── Form1.cs / Form1.Designer.cs ← menú principal
    ├── Data/
    │   ├── AppDbContext.cs          ← EF Core: tablas y reglas de mapeo
    │   └── AppDbContextFactory.cs   ← contexto para herramientas (migraciones)
    ├── Models/
    │   ├── Product.cs
    │   ├── StockMovement.cs
    │   └── MovementType.cs
    ├── Services/
    │   ├── ProductService.cs
    │   ├── InventoryService.cs
    │   └── ReportingService.cs
    ├── Forms/
    │   ├── FormProductos.cs (+ Designer)
    │   ├── FormProductoEdit.cs (+ Designer)   ← diálogo alta/edición
    │   ├── FormMovimientos.cs (+ Designer)
    │   └── FormReportes.cs (+ Designer)
    ├── Migrations/                  ← historial de esquema SQLite (EF)
    └── SistemaGestionVentas.csproj
```

La **base de datos en uso** no vive en el código fuente: el archivo `ventas.db` se crea en:

`%LocalAppData%\SistemaGestionVentas\ventas.db`

Así los datos personales no se mezclan con el repositorio Git.

---

## 2. Arquitectura en capas

| Capa | Ubicación | Responsabilidad |
|------|-----------|-----------------|
| **Modelos** | `Models/` | Definen la forma de los datos (entidades y enums). No contienen lógica de negocio compleja. |
| **Datos** | `Data/` | `AppDbContext`: conexión conceptual con SQLite, `DbSet<>`, reglas de columnas y relaciones. |
| **Servicios** | `Services/` | Reglas de negocio y acceso a datos: CRUD de productos, registrar movimientos con transacción, consultas de reportes. |
| **Interfaz** | `Form1`, `Forms/` | Ventanas, botones, grillas. Llaman a los **servicios**; no deberían contener SQL ni usar `DbContext` directamente. |

Flujo típico:

```text
Usuario → Formulario → Servicio → AppDbContext → SQLite
```

---

## 3. Punto de entrada: `Program.cs`

- **`[STAThread]`**: requisito habitual de Windows Forms para el hilo de interfaz.
- **`ApplicationConfiguration.Initialize()`**: configuración DPI/fuentes del proyecto WinForms moderno.
- Se arma la **ruta del archivo SQLite** bajo `Environment.SpecialFolder.LocalApplicationData`, carpeta `SistemaGestionVentas`.
- Se construye **`DbContextOptions<AppDbContext>`** con `UseSqlite("Data Source=...")` y se guarda en **`Program.DbContextOptions`** para que formularios y servicios puedan crear contextos coherentes sin duplicar la cadena de conexión.
- **`using (var db = new AppDbContext(...)) { db.Database.Migrate(); }`**: al iniciar, EF aplica las **migraciones** pendientes y crea o actualiza tablas.
- **`Application.Run(new Form1())`**: muestra el menú principal.

---

## 4. Modelos (`Models/`)

### 4.1 `Product`

Representa un artículo del inventario.

| Propiedad | Significado |
|-----------|-------------|
| `Id` | Clave primaria (autogenerada por la BD). |
| `Name` | Nombre del producto (obligatorio en validación). |
| `Sku` | Código opcional. |
| `CostPrice` | Precio de compra unitario (usa `decimal` para dinero). |
| `ListSalePrice` | Precio de venta de lista (referencia para ventas). |
| `StockQuantity` | Stock actual; se modifica con **movimientos** (y stock inicial al crear). |
| `MinStockThreshold` | Umbral para alertas de “stock bajo” en reportes. |
| `Movements` | Colección de movimientos (navegación EF hacia `StockMovement`). |

### 4.2 `StockMovement`

Un registro de cambio de stock (y de venta, si aplica).

| Propiedad | Significado |
|-----------|-------------|
| `ProductId` / `Product` | Producto afectado. |
| `Type` | Tipo de movimiento (`MovementType`). |
| `Quantity` | Siempre positiva; el efecto en stock lo define el tipo. |
| `UnitSalePrice` | Precio unitario real de venta (relevante si `Type == Venta`; en otros casos suele ser 0). |
| `UnitCostSnapshot` | Copia del costo del producto **en el momento del movimiento** (sirve para márgenes históricos aunque después cambies el costo en el producto). |
| `OccurredAt` | Fecha/hora del movimiento (`DateTime.Now`). |
| `Note` | Observación opcional. |

### 4.3 `MovementType` (enum)

| Valor | Efecto en stock |
|-------|-----------------|
| `Entrada` | Suma cantidad. |
| `Salida` | Resta cantidad (sin tratarse como venta en reportes de margen). |
| `Venta` | Resta cantidad; usa `UnitSalePrice` para ingresos. |
| `AjustePositivo` | Suma (corrección de inventario). |
| `AjusteNegativo` | Resta (corrección). |

---

## 5. Acceso a datos: `AppDbContext` y migraciones

### 5.1 `AppDbContext`

- Hereda de `DbContext`.
- Expone `DbSet<Product> Products` y `DbSet<StockMovement> StockMovements`.
- En **`OnModelCreating`**:
  - Limita longitudes de texto y **`HasPrecision(18, 2)`** en decimales (dinero).
  - Índice en `Sku`.
  - Relación **uno a muchos**: un producto tiene muchos movimientos; clave foránea `ProductId`.
  - **`OnDelete(DeleteBehavior.Restrict)`**: no se puede borrar un producto “en cascada” desde la BD si hay movimientos; además el servicio impide borrar si existen movimientos.

### 5.2 `AppDbContextFactory`

Implementa `IDesignTimeDbContextFactory<AppDbContext>`. Cuando ejecutás **`dotnet ef migrations add ...`** desde la terminal, EF necesita crear un contexto **sin ejecutar la aplicación**; esta fábrica usa un archivo temporal `SistemaGestionVentas_design.db` solo para diseño.

### 5.3 Carpeta `Migrations/`

Contiene clases generadas por EF que describen el **esquema** (tablas, columnas, índices). `Migrate()` en `Program.cs` las aplica al `ventas.db` real en `%LocalAppData%`.

---

## 6. Servicios (`Services/`)

Los servicios reciben **`DbContextOptions<AppDbContext>`** y, en cada operación, hacen **`using var ctx = new AppDbContext(_options)`** para un contexto corto y descartable.

### 6.1 `ProductService`

| Método | Comportamiento |
|--------|----------------|
| `GetAll` | Lista productos ordenados por nombre. Usa **`AsNoTracking()`** para lecturas sin seguimiento de cambios (más simple y eficiente para grillas). |
| `GetById` | Un producto por id o `null`. |
| `Create` | Valida nombre y precios; crea producto con **stock inicial**. |
| `Update` | Actualiza nombre, SKU, precios y umbral mínimo. **No** cambia el stock aquí (el stock lo manejan los movimientos). |
| `Delete` | Elimina solo si **no** hay movimientos para ese producto. |

Validaciones básicas en métodos privados/`Validate`: nombre obligatorio, precios no negativos, umbral mínimo no negativo.

### 6.2 `InventoryService`

| Método | Comportamiento |
|--------|----------------|
| `RegisterMovement` | Abre una **transacción** (`BeginTransaction`). Carga el producto, calcula el **delta** de stock según el tipo, comprueba que el stock no quede negativo, guarda el movimiento con **snapshot de costo** y precio de venta si es `Venta`, actualiza `StockQuantity`, `SaveChanges()` y **`Commit`**. Si algo falla, **`Rollback`**. |

La transacción evita estados inconsistentes (por ejemplo, movimiento guardado pero stock no actualizado).

### 6.3 `ReportingService` y registros DTO

Además de la clase `ReportingService`, el archivo define **records** que son “filas de vista” para las grillas (no son tablas de BD):

- `LowStockRow` — productos con `StockQuantity <= MinStockThreshold`.
- `MovementStatRow` — totales por producto (vendidos o egresados).
- `MovementHistoryRow` — línea de historial con etiqueta de tipo en español.
- `MarginRow` — ingresos, costo de unidades vendidas, beneficio y **ROI % sobre costo**.

| Método | Idea de negocio |
|--------|-----------------|
| `GetLowStock` | Alertas de reposición. |
| `GetInventoryTotalValue` | Suma de `StockQuantity * CostPrice` (valorización a costo). |
| `GetTopSold` | Agrupa movimientos **`Venta`** por producto y ordena por unidades. |
| `GetTopOutgoingMovementVolume` | Suma egresos: `Salida`, `Venta`, `AjusteNegativo`. |
| `GetMovementHistory` | Filtros opcionales por producto y rango de fechas; incluye nombre de producto y etiquetas legibles. |
| `GetMarginByProduct` | Solo ventas: ingreso = `Sum(UnitSalePrice * Quantity)`, costo = `Sum(UnitCostSnapshot * Quantity)`; beneficio y ROI = `(beneficio / costo) * 100` si costo &gt; 0. |

**Nota:** Los parámetros `fromUtc` / `toUtc` del historial se usan como rango de fechas; los movimientos usan `DateTime.Now` local, coherente con filtros del formulario de reportes.

---

## 7. Interfaz de usuario (formularios)

### 7.1 `Form1` — menú principal

Tres botones que abren formularios **modales** (`ShowDialog`): Productos, Movimientos, Reportes. Cada formulario hijo crea sus servicios internamente usando `Program.DbContextOptions`.

### 7.2 `FormProductos` y `FormProductoEdit`

- **Grilla** (`DataGridView`) de solo lectura con columnas principales del producto.
- **Nuevo / Editar**: abre `FormProductoEdit`. En **alta** se puede definir **stock inicial**; en **edición** el stock se muestra como referencia pero **no** se edita ahí (solo movimientos).
- **Eliminar**: confirmación; errores del servicio se muestran en un `MessageBox`.

### 7.3 `FormMovimientos`

- Combo de producto, tipo de movimiento (textos en español), cantidad, precio unitario de venta (habilitado solo para **Venta**, por defecto el precio de lista del producto), nota opcional.
- **Registrar** llama a `InventoryService.RegisterMovement`.

### 7.4 `FormReportes`

`TabControl` con pestañas:

1. **Stock bajo** — grilla desde `GetLowStock`.
2. **Valor inventario** — etiqueta con total desde `GetInventoryTotalValue`.
3. **Más vendidos** — `GetTopSold`.
4. **Más movidos (egresos)** — `GetTopOutgoingMovementVolume`.
5. **Historial** — filtros producto / desde-hasta y botón “Aplicar filtros”.
6. **Margen / ROI** — `GetMarginByProduct`.

Botón **Actualizar todo** refresca todas las pestañas.

---

## 8. Dependencias NuGet (resumen)

En el `.csproj` del proyecto:

- **`Microsoft.EntityFrameworkCore.Sqlite`** — proveedor SQLite para EF Core.
- **`Microsoft.EntityFrameworkCore.Design`** — soporte de herramientas (`dotnet ef migrations ...`).

---

## 9. Git y seguimiento de versiones

En la carpeta `SistemaGestionVentas` (donde está el `.sln`) conviene tener el **único** `.git` del proyecto. El archivo **`.gitignore`** evita subir `bin/`, `obj/`, `.vs/` y archivos temporales de usuario.

**No** se versiona el `ventas.db` de uso diario (está fuera del repo, en `LocalAppData`). Sí tiene sentido versionar el **código** y las **migraciones** para que cualquier clon del repo pueda recrear el esquema con `Migrate()`.

---

## 10. Cómo extender el sistema (ideas)

- **Búsqueda de precios en la web**: nuevo servicio que use `HttpClient` y una pantalla opcional; no requiere cambiar el modelo mínimo si solo consultás.
- **Categorías de producto**: nueva entidad y FK en `Product`, migración nueva.
- **Exportar reportes** (CSV/Excel): desde los mismos DTO que ya devuelve `ReportingService`.
- **Inyección de dependencias** (por ejemplo `Microsoft.Extensions.Hosting` en WinForms): reemplazaría el patrón actual de `new ProductService(Program.DbContextOptions)` por servicios registrados una vez.

---

## 11. Glosario rápido

| Término | Significado breve |
|---------|-------------------|
| **EF Core** | ORM de Microsoft: mapea clases C# a tablas y traduce LINQ a SQL. |
| **DbContext** | Unidad de trabajo: acceso a tablas (`DbSet`) y `SaveChanges`. |
| **Migración** | Cambio versionado del esquema de la BD. |
| **`AsNoTracking()`** | Lectura sin seguimiento de cambios en memoria (útil en listados). |
| **Transacción** | Bloque atómico: o se confirma todo (`Commit`) o se deshace (`Rollback`). |
| **DTO** | Objeto solo para transferir datos a la UI (aquí, los `record` de reportes). |

---

*Documento generado como guía del código del proyecto SistemaGestionVentas. Actualizalo cuando agregues módulos o cambies reglas de negocio.*
