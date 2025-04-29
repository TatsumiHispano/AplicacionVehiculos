🚗 Sistema de Gestión de Vehículos
Este proyecto es una aplicación modular para la gestión de vehículos con soporte para distintos métodos de almacenamiento persistente, diseñada con principios de desacoplamiento, extensibilidad y eficiencia.

Permite almacenar, consultar, modificar y eliminar información de vehículos usando diferentes formatos: memoria, archivos de texto, archivos binarios y JSON.

🧱 Arquitectura y Componentes
🔌 Capa de Acceso a Datos
El acceso a los datos se maneja mediante una clase abstracta común, lo que permite cambiar fácilmente el método de persistencia sin afectar el resto del sistema.

AccesoDatosAbstract
Define las operaciones comunes de acceso:

GetAllAsync() – Obtiene todos los vehículos.

GetByIdAsync(string matricula) – Obtiene un vehículo por matrícula.

InsertAsync(Vehiculo vehiculo) – Inserta un nuevo vehículo.

UpdateAsync(Vehiculo vehiculo) – Actualiza un vehículo existente.

DeleteAsync(Vehiculo vehiculo) – Elimina un vehículo.

📦 Implementaciones
AccesoDatosMemoria
Almacena los datos en memoria. Ideal para pruebas o entornos donde no se requiere persistencia.

AccesoDatosFicheroTexto
Utiliza un archivo de texto plano con formato delimitado por ;.

AccesoDatosFicheroBinario
Emplea serialización binaria para una mayor eficiencia de lectura/escritura.

AccesoDatosFicheroJSON
Usa formato JSON, ideal para interoperabilidad con otros sistemas.

🧠 Patrón Singleton y Configuración Dinámica
El sistema emplea el patrón Singleton para asegurar que solo exista una instancia de acceso a datos. El método estático GetInstance() de AccesoDatosAbstract selecciona la implementación concreta basándose en el valor de ModoAccesoDatos definido en el archivo de configuración AppConfig.

🧾 Configuración (AppConfig)
Define:

Tipo de almacenamiento: MEMORIA, FICHERO_TEXTO, FICHERO_BINARIO, FICHERO_JSON.

Rutas de los archivos si aplica.

Este enfoque permite cambiar el tipo de persistencia sin modificar el código fuente.

⚙️ Asincronía
Todas las operaciones de acceso a datos son asincrónicas utilizando async/await. Esto evita el bloqueo del hilo principal y mejora el rendimiento, especialmente con archivos o datos extensos.

🚘 Entidades y Clases Auxiliares
Vehiculo
Representa un vehículo con las siguientes propiedades:

Matrícula

Marca

Kilometraje

Fecha de matriculación

Descripción

Precio

Propietario

DNI del propietario

🔁 Flujo de Trabajo
1. Inicialización
Al iniciar, se lee AppConfig para determinar el tipo de almacenamiento.

Se crea una instancia concreta de acceso a datos según la configuración.

2. Operaciones CRUD

Acción	Método	Descripción
Crear	InsertAsync()	Agrega un nuevo vehículo

Leer	GetAllAsync() / GetByIdAsync()	Consulta vehículos existentes

Actualizar	UpdateAsync()	Modifica los datos de un vehículo existente

Eliminar	DeleteAsync()	Borra un vehículo por su matrícula

Después de cada operación, los cambios se persisten automáticamente en el medio de almacenamiento correspondiente.

✅ Ventajas del Sistema

Flexibilidad: Soporte para múltiples formas de almacenamiento.

Desacoplamiento: Separación entre lógica de negocio y persistencia de datos.

Escalabilidad: Elige el tipo de almacenamiento según el tamaño o requerimientos del sistema.

Extensibilidad: Se pueden agregar nuevos formatos de almacenamiento fácilmente.

Asincronía: Operaciones no bloqueantes que mejoran la eficiencia general.

🧩 Conclusión

Este sistema ofrece una solución robusta y adaptable para la gestión de vehículos. Su arquitectura desacoplada, unida al uso de patrones de diseño y asincronía, lo convierte en un proyecto ideal para aplicaciones de pequeña a gran escala, fácilmente integrable en entornos modernos.
