Este proyecto es un sistema que gestiona vehículos utilizando una capa de acceso a datos persistente.
Permite manejar la información de los vehículos de forma flexible, con soporte para diferentes métodos de almacenamiento como memoria, archivos de texto, archivos binarios y archivos JSON.
El acceso a los datos se maneja mediante una interfaz común que facilita el cambio entre diferentes métodos de persistencia sin alterar el resto de la aplicación.

Estructura y Componentes
AccesoDatosAbstract (Clase Abstracta)
AccesoDatosAbstract es una clase abstracta que define las operaciones comunes para acceder y manipular los datos de los vehículos. Las operaciones incluyen:

GetAllAsync(): Obtiene todos los vehículos.
GetByIdAsync(string matricula): Obtiene un vehículo según su matrícula.
InsertAsync(Vehiculo vehiculo): Inserta un nuevo vehículo.
UpdateAsync(Vehiculo vehiculo): Actualiza los datos de un vehículo existente.
DeleteAsync(Vehiculo vehiculo): Elimina un vehículo.
Esta clase define la interfaz para todas las implementaciones de acceso a datos.

Implementaciones Concretas
Existen varias implementaciones de AccesoDatosAbstract, cada una encargada de almacenar los vehículos de diferente manera:

AccesoDatosMemoria
Almacena los vehículos en memoria, lo que significa que los datos se pierden cuando la aplicación termina. Es útil para operaciones rápidas de prueba o en escenarios donde la persistencia no es necesaria.

AccesoDatosFicheroTexto
Guarda los vehículos en un archivo de texto plano. Cada línea del archivo contiene los datos de un vehículo en un formato delimitado por punto y coma (;).
Este enfoque es sencillo pero no muy eficiente ni estructurado.

AccesoDatosFicheroBinario
Utiliza la serialización binaria para almacenar los vehículos. Este enfoque es más eficiente en términos de almacenamiento y velocidad en comparación con los archivos de texto.
Los datos se guardan en un archivo binario que puede ser leído y escrito de manera más rápida y compacta.

AccesoDatosFicheroJSON
Almacena los vehículos en un archivo JSON. JSON es un formato estructurado, fácil de leer y ampliamente utilizado. 
Es útil si se necesita compartir o integrar los datos con otros sistemas que también usen este formato.

Patrón Singleton
El proyecto utiliza el patrón Singleton para asegurar que haya solo una instancia de la clase encargada de gestionar el acceso a los datos.
La clase AccesoDatosAbstract tiene un método estático GetInstance(), que devuelve la única instancia según la configuración proporcionada en el archivo AppConfig.
Dependiendo del valor de ModoAccesoDatos (que puede ser "MEMORIA", "FICHERO_TEXTO", "FICHERO_BINARIO" o "FICHERO_JSON"), el sistema creará la instancia adecuada (por ejemplo, AccesoDatosMemoria si el modo es "MEMORIA").

Configuración Dinámica
El modo de acceso a los datos se puede cambiar dinámicamente mediante la configuración del archivo AppConfig.
Esta flexibilidad permite a la aplicación adaptarse a diferentes necesidades sin tener que modificar el código base.

Operaciones Asincrónicas
Todas las operaciones de acceso a los datos son asincrónicas. Se utilizan tareas (Task) y async/await para permitir la ejecución de operaciones sin bloquear el hilo principal.
Esto mejora la eficiencia y la capacidad de respuesta de la aplicación, especialmente cuando se trabaja con archivos o bases de datos de gran tamaño.

Clases y Métodos Auxiliares

Vehiculo: Representa la entidad de un vehículo, con propiedades como matrícula, marca, número de kilómetros, fecha de matriculación, descripción, precio, propietario y DNI del propietario.
AppConfig: Configura las rutas de los archivos y el tipo de acceso a los datos (en memoria, texto, binario o JSON).
Flujo de Trabajo
Inicialización
Cuando la aplicación se inicia, el sistema lee el archivo de configuración (AppConfig) para determinar qué modo de acceso a datos se debe utilizar.
A continuación, crea la instancia correspondiente (por ejemplo, AccesoDatosFicheroTexto o AccesoDatosFicheroBinario) y se conecta al almacenamiento elegido.
Dependiendo del tipo de almacenamiento, los datos de los vehículos pueden cargarse en memoria o leerse desde el archivo correspondiente.

Operaciones CRUD
Los usuarios pueden interactuar con la aplicación mediante operaciones CRUD (crear, leer, actualizar y eliminar) para gestionar los vehículos:

Leer: GetAllAsync() devuelve todos los vehículos almacenados. GetByIdAsync() devuelve un vehículo por su matrícula.
Crear: InsertAsync() agrega un nuevo vehículo a la colección.
Actualizar: UpdateAsync() reemplaza un vehículo existente con los nuevos datos.
Eliminar: DeleteAsync() elimina un vehículo de la colección.
Persistencia
Los cambios realizados en la colección de vehículos se guardan automáticamente en el almacenamiento correspondiente después de cada operación de inserción, actualización o eliminación.

Ventajas
Flexibilidad: El sistema soporta varios tipos de almacenamiento (memoria, texto, binario y JSON), lo que permite elegir el tipo de persistencia más adecuado según las necesidades del proyecto.
Desacoplamiento: Gracias al patrón de diseño Singleton y la separación de la interfaz (AccesoDatosAbstract) y las implementaciones concretas, el código es fácilmente extensible y adaptable.
Escalabilidad: La capacidad de elegir entre diferentes tipos de almacenamiento permite que el sistema escale según el tamaño de los datos y los requerimientos de rendimiento.
Asincronía: El uso de operaciones asincrónicas mejora la eficiencia del sistema, especialmente cuando se interactúa con archivos o bases de datos más grandes.
Conclusión
Este proyecto proporciona una solución flexible y escalable para gestionar vehículos en una aplicación, con múltiples opciones de almacenamiento y acceso a datos.
Gracias al patrón Singleton, la arquitectura está diseñada para ser fácilmente extensible y adaptable a futuras necesidades.
La implementación de operaciones asincrónicas mejora la eficiencia del sistema, haciendo que sea adecuado para proyectos de cualquier tamaño.
