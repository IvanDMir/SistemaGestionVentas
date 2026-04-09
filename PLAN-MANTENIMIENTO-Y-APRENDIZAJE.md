# Plan: mantener, mejorar y aprender con esta aplicación

Este documento combina **qué hacer para cuidar el proyecto**, **cómo ir mejorándolo por etapas** y **cómo usar la misma app como laboratorio de aprendizaje** en C#, WinForms y Entity Framework Core.

---

## Parte A — Mantenimiento (poco esfuerzo, mucho valor)

### A1. Copias de seguridad de tus datos

- El archivo importante es **`ventas.db`** en `%LocalAppData%\SistemaGestionVentas\`.
- **Hábito:** una vez por semana (o antes de probar cambios grandes en el código), copiá ese archivo a otra carpeta, disco o nube.
- Si algo sale mal, reemplazás el `.db` por la copia.

### A2. Control de versiones (Git)

- Trabajá siempre dentro de la carpeta del proyecto donde está el `.git` (no desde `C:\Users\...` entero).
- **Commits pequeños** con mensajes claros: *"Agrego filtro por nombre en productos"* en lugar de *"cambios"*.
- Antes de experimentar fuerte, creá una rama: `git checkout -b experimento-reportes`.

### A3. Dependencias NuGet

- Cada tanto, en Visual Studio: **Administrar paquetes NuGet** → pestaña **Actualizaciones**. Leé las notas de versión antes de subir de major (por ejemplo 8 → 9).
- Tras actualizar, **compilá y probá** flujos críticos: alta de producto, venta, reporte de margen.

### A4. Migraciones de base de datos

- Si **agregás propiedades o tablas** a las entidades, el flujo habitual es:
  1. Cambiar modelos + `AppDbContext` si hace falta.
  2. `dotnet ef migrations add NombreDescriptivo` (desde la carpeta del `.csproj`).
  3. Probar: al arrancar la app, `Migrate()` aplica los cambios.
- **Regla:** no borres migraciones viejas que ya se aplicaron en tu `ventas.db` real sin entender el impacto; mejor nuevas migraciones que “arreglen” el esquema.

### A5. Definición de “listo para usar”

- Antes de dar por cerrado un cambio: compilación sin errores, abrir la app, crear un producto de prueba, un movimiento y mirar un reporte.
- Opcional: anotá en un `CHANGELOG.md` qué cambió en cada versión (útil cuando volvés al proyecto al cabo de meses).

---

## Parte B — Mejoras por etapas (elegí según ganas y tiempo)

Orden sugerido: de **menor riesgo / más aprendizaje visible** a **más ambicioso**.

| Etapa | Mejora | Qué aprendés |
|-------|--------|----------------|
| **B1** | Validaciones en UI (mensajes más claros, campos obligatorios marcados) | WinForms, UX básica |
| **B2** | Buscador de productos por nombre en la grilla | LINQ, filtrado en memoria o en consulta |
| **B3** | **Proveedor**, **etiquetas** o subcategorías (ya existe **línea/grupo** en `ProductGroup`) + migración | Modelo relacional, migraciones EF |
| **B4** | Exportar una grilla de reportes a **CSV** (guardar archivo) | `StreamWriter`, diálogos `SaveFileDialog` |
| **B5** | Pantalla de **configuración** (por ejemplo umbral global, carpeta de backup sugerida) | `Application.UserAppDataPath`, `Settings` |
| **B6** | **Inyección de dependencias** (registrar servicios una vez al inicio) | Patrones, testabilidad |
| **B7** | Pruebas unitarias de **servicios** (sin WinForms) con xUnit | Testing, diseño de API interna |
| **B8** | Consulta de precio vía **HTTP** (API pública o scraping ético) | `HttpClient`, async/await |
| **B9** | Instalador o **MSIX** / publicación firmada | Distribución real de escritorio |

No hace falta seguir el orden al pie de la letra: podés saltar a lo que más te motive siempre que leas un poco antes (por ejemplo async antes de tocar HTTP).

---

## Parte C — Cómo aprender “lo que hace esta app” (opciones)

### Opción 1 — Recorrido guiado por el código (recomendada)

Hacé **un archivo o una “lección” por sesión**, en este orden:

1. **`Program.cs`** → arranque, ruta de la BD, `Migrate()`, `DbContextOptions`.
2. **`Models/`** → entidades `Product`, `ProductGroup`, `StockMovement`; enum `MovementType`.
3. **`Data/AppDbContext.cs`** → `DbSet`, relaciones, `OnModelCreating`, precisión `decimal`.
4. **`Services/ProductService.cs`** → `GetAll` con `Include`, `ResolveProductGroupId`, `Create`/`Update`.
5. **`Services/InventoryService.cs`** → transacción, stock, **promedio ponderado** en compras (`Entrada`).
6. **`Services/ReportingService.cs`** → LINQ, materialización cuando hace falta para SQLite.
7. **`UI/UiTheme.cs`** → cómo se unifica el aspecto de botones y grillas.
8. **Un formulario** → `FormMovimientos` (eventos → `RegisterMovement`) o `FormProductos` (grilla + `Compra / ingreso`).

**Método:** leé el archivo, comentá en voz alta *qué haría yo si no existiera este método*, después compará con lo implementado.

### Opción 2 — Microsoft Learn (oficial, gratis, en español cuando existe)

- [Introducción a .NET](https://learn.microsoft.com/es-es/dotnet/core/introduction)
- [C#](https://learn.microsoft.com/es-es/dotnet/csharp/) (fundamentos, tipos, LINQ)
- [Entity Framework Core](https://learn.microsoft.com/es-es/ef/core/) → “Primeros pasos”, “Consultas”, “Migraciones”
- WinForms: buscá en Learn *Windows Forms* en inglés si hace falta; muchos conceptos (eventos, controles) son iguales en cualquier idioma.

**Método:** hacé un módulo de Learn y relacionálo con **un archivo** de tu repo (“esto es como `SaveChanges` que vimos en ProductService”).

### Opción 3 — Experimentos seguros (“sandbox”)

- **Duplicá** `ventas.db` con otro nombre y cambiá temporalmente la ruta en `Program.cs` (o usá otra carpeta) para **romper cosas sin miedo**.
- Añadí un `Console.WriteLine` o un punto de interrupción (**F9**) en `RegisterMovement` y seguí paso a paso (**F10/F11**) viendo variables.

### Opción 4 — Retos cortos (1–2 horas cada uno)

- Reto A: mostrar en el título del formulario principal la **cantidad de productos** en stock bajo.
- Reto B: impedir **precio de venta** menor que cero y menor que un porcentaje del costo (regla de negocio tuya).
- Reto C: agregar columna **“última venta”** en un reporte (requiere consulta con `Max`/`OrderBy` sobre movimientos tipo Venta).

### Opción 5 — Comunidad y referencias

- Documentación en el repo: **`DOCUMENTACION.md`** (arquitectura, servicios, fórmulas, orden de lectura), **`GUIA-USO.md`** (pasos en pantalla + tabla “dónde está en el código”).
- Foros: [Stack Overflow](https://stackoverflow.com/questions/tagged/c%23+winforms+entity-framework-core) con etiquetas `c#`, `winforms`, `entity-framework-core`.
- Vídeos: buscá *“C# WinForms tutorial”* y *“Entity Framework Core tutorial”*; contrastá siempre con lo que **ya tenés** en el proyecto para no mezclar versiones viejas (.NET Framework vs .NET 8).

### Opción 6 — Llevar un diario de aprendizaje

Archivo `APRENDIZAJE.md` o cuaderno: **3 líneas por sesión** — *qué leí, qué cambié, qué no entendí*. Volver a lo “no entendí” en la siguiente sesión acelera mucho.

---

## Parte D — Ritmo sugerido (ejemplo de 8 semanas, flexible)

| Semana | Enfoque |
|--------|---------|
| 1 | Recorrido `Program` + `Models` + ejecutar y usar la app todos los días 10 min |
| 2 | `AppDbContext` + leer una migración generada (SQL implícito) |
| 3 | `ProductService` + modificar un mensaje de validación |
| 4 | `InventoryService` + depuración paso a paso de una venta |
| 5 | `ReportingService` + dibujar en papel el flujo de datos del reporte de margen |
| 6 | Mejora B2 o B4 (buscador o CSV) |
| 7 | Microsoft Learn: bloque LINQ + reescribir una consulta del `ReportingService` comentando la anterior |
| 8 | Backup de `ventas.db`, commit en Git, decidir siguiente mejora (B3 o B6) |

Ajustá la velocidad: **constancia breve** gana a maratones esporádicos.

---

## Resumen

- **Mantener:** backups del `.db`, Git con commits claros, actualizar NuGet con prudencia, migraciones nuevas cuando cambie el modelo.
- **Mejorar:** lista B1–B9 por dificultad e interés; cada ítem es un proyecto de aprendizaje.
- **Aprender:** recorrido por archivos del repo + Learn + retos pequeños + depurador; documentá dudas y victorias.

Cuando completes una etapa, podés tacharla en este archivo o añadir una sección **“Hecho”** con fecha para ver tu progreso.
