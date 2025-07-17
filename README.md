# 🚗 Sistema de Gestión de Vehículos

**Sistema de Gestión de Vehículos** es una aplicación modular desarrollada en C# que permite administrar información de vehículos utilizando distintos métodos de almacenamiento persistente. Su diseño se basa en principios de **desacoplamiento**, **extensibilidad** y **eficiencia**, ideal para proyectos escalables y fácilmente adaptables.

---

## 🧱 Arquitectura y Componentes

### 🔌 Capa de Acceso a Datos

La persistencia de datos se maneja a través de una clase abstracta común que define las operaciones estándar, lo que permite cambiar el método de almacenamiento sin modificar el resto del sistema.

#### `AccesoDatosAbstract`

Métodos principales (todos asincrónicos):

- `GetAllAsync()` – Obtiene todos los vehículos.
- `GetByIdAsync(string matricula)` – Busca un vehículo por matrícula.
- `InsertAsync(Vehiculo vehiculo)` – Agrega un nuevo vehículo.
- `UpdateAsync(Vehiculo vehiculo)` – Actualiza un vehículo existente.
- `DeleteAsync(Vehiculo vehiculo)` – Elimina un vehículo.

---

### 📦 Implementaciones de Persistencia

| Implementación | Descripción |
|----------------|-------------|
| `AccesoDatosMemoria` | Almacena los datos en memoria. Útil para pruebas o entornos temporales. |
| `AccesoDatosFicheroTexto` | Utiliza archivos de texto plano (`.txt`) con delimitadores `;`. |
| `AccesoDatosFicheroBinario` | Serializa objetos binariamente para mayor eficiencia de lectura/escritura. |
| `AccesoDatosFicheroJSON` | Utiliza formato JSON para mejor interoperabilidad con otros sistemas. |

---

### 🧠 Patrón Singleton + Configuración Dinámica

- Se implementa el **patrón Singleton** para garantizar que solo exista una instancia del acceso a datos.
- El método `GetInstance()` selecciona dinámicamente la implementación según el valor definido en el archivo `AppConfig`.

---

### 🧾 Archivo de Configuración – `AppConfig`

Define los siguientes parámetros:

- **Tipo de almacenamiento:** `MEMORIA`, `FICHERO_TEXTO`, `FICHERO_BINARIO`, `FICHERO_JSON`.
- **Rutas de archivos:** Para los métodos de almacenamiento en disco.

> Permite cambiar el tipo de persistencia sin tocar el código fuente.

---

## ⚙️ Operaciones Asincrónicas

Todas las operaciones implementan `async/await` para garantizar un comportamiento **no bloqueante**, lo cual mejora la experiencia del usuario y el rendimiento en sistemas con archivos grandes o acceso frecuente a datos.

---

## 🚘 Entidad Principal: `Vehiculo`

Modelo que representa los datos de un vehículo:

| Propiedad            | Descripción                     |
|----------------------|----------------------------------|
| `Matricula`          | Identificador único del vehículo |
| `Marca`              | Marca del vehículo               |
| `Kilometraje`        | Kilómetros recorridos            |
| `FechaMatriculacion` | Fecha de registro del vehículo   |
| `Descripcion`        | Observaciones o detalles         |
| `Precio`             | Valor estimado                   |
| `Propietario`        | Nombre del propietario           |
| `DNIPropietario`     | DNI del propietario              |

---

## 🔁 Flujo de Trabajo

### 1. 🏁 Inicialización
- Al iniciar la aplicación, se lee `AppConfig`.
- Se crea automáticamente la instancia correspondiente del acceso a datos según el tipo de almacenamiento configurado.

### 2. 🔧 Operaciones CRUD

| Acción     | Método                 | Descripción                              |
|------------|------------------------|------------------------------------------|
| Crear      | `InsertAsync()`        | Agrega un nuevo vehículo                 |
| Leer       | `GetAllAsync()`        | Consulta todos los vehículos             |
| Leer (ID)  | `GetByIdAsync()`       | Busca por matrícula                      |
| Actualizar | `UpdateAsync()`        | Modifica un vehículo existente           |
| Eliminar   | `DeleteAsync()`        | Elimina un vehículo según su matrícula   |

> Después de cada operación, los datos se guardan automáticamente en el medio de almacenamiento seleccionado.

---

## ✅ Ventajas del Sistema

- **🔄 Flexibilidad:** Soporta múltiples formas de almacenamiento según necesidad.
- **🔌 Desacoplamiento:** Lógica de negocio separada de la persistencia.
- **📈 Escalabilidad:** Se adapta tanto a entornos pequeños como complejos.
- **➕ Extensibilidad:** Es fácil agregar nuevos métodos de persistencia.
- **⚡ Eficiencia:** Uso de asincronía para un rendimiento optimizado.

---

## 🧩 Conclusión

Este sistema ofrece una solución robusta, limpia y extensible para la gestión de vehículos. Su arquitectura modular y el uso de patrones como Singleton, junto con operaciones asincrónicas y configuración externa, lo convierten en un ejemplo ideal de buenas prácticas en aplicaciones modernas orientadas a la mantenibilidad y escalabilidad.

> Perfecto para desarrollos educativos, empresariales o como base para sistemas más complejos.

---

¿Te resultó útil este proyecto? ¡Déjale una ⭐ en GitHub o contribuye con mejoras!


🧩 Conclusión

Este sistema ofrece una solución robusta y adaptable para la gestión de vehículos. Su arquitectura desacoplada, unida al uso de patrones de diseño y asincronía, lo convierte en un proyecto ideal para aplicaciones de pequeña a gran escala, fácilmente integrable en entornos modernos.
