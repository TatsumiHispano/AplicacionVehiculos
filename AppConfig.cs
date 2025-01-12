using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tarea2_Persistencia
{
    public static class AppConfig
    {
        public static string ModoAccesoDatos { get; set; } = "MEMORIA"; // Valores: FICHERO_TEXTO, FICHERO_BINARIO, FICHERO_JSON, MEMORIA
        public static string RutaFicheroTexto { get; set; } = "C:\\IA\\FicheroTexto.txt";
        public static string RutaFicheroBinario { get; set; } = "C:\\IA\\FicheroBinario.bin";
        public static string RutaFicheroJSON { get; set; } = "C:\\IA\\FicheroJSON.json";
        public static string ConexionMySQL { get; set; } = "server=localhost;port=3307;database=concesionario;user=root;password=;";

        private const string ConfigFile = "appconfig.json";

        /// <summary>
        /// Carga la configuración desde un archivo o solicita al usuario configurarla si no existe.
        /// </summary>
        public static void CargarConfiguracion()
        {
            // Verifica si el archivo de configuración existe
            if (File.Exists(ConfigFile))
            {
                try
                {
                    // Intenta leer y deserializar la configuración desde el archivo JSON
                    var config = JsonSerializer.Deserialize<Configuracion>(File.ReadAllText(ConfigFile));
                    if (config != null)
                    {
                        // Si la configuración existe, asigna los valores
                        ModoAccesoDatos = config.ModoAccesoDatos ?? ModoAccesoDatos;
                        RutaFicheroTexto = config.RutaFicheroTexto ?? RutaFicheroTexto;
                        RutaFicheroBinario = config.RutaFicheroBinario ?? RutaFicheroBinario;
                        RutaFicheroJSON = config.RutaFicheroJSON ?? RutaFicheroJSON;
                        ConexionMySQL = config.ConexionMySQL ?? ConexionMySQL;
                        Console.WriteLine("Configuración cargada correctamente.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la configuración: {ex.Message}");
                }
            }

            // Si no hay configuración válida, se solicita al usuario
            Console.WriteLine("No se encontró una configuración válida. Proporcione la información para configurar el sistema:");
            // Solicita al usuario los valores necesarios con un mensaje y un valor por defecto
            ModoAccesoDatos = SolicitarDato("Modo de acceso a datos (MEMORIA, FICHERO_TEXTO, FICHERO_BINARIO, FICHERO_JSON, MYSQL):", ModoAccesoDatos);
            RutaFicheroTexto = SolicitarDato("Ruta para Fichero Texto:", RutaFicheroTexto);
            RutaFicheroBinario = SolicitarDato("Ruta para Fichero Binario:", RutaFicheroBinario);
            RutaFicheroJSON = SolicitarDato("Ruta para Fichero JSON:", RutaFicheroJSON);
            ConexionMySQL = SolicitarDato("Cadena de conexión MySQL:", ConexionMySQL);

            GuardarConfiguracion();
        }

        /// <summary>
        /// Guarda la configuración actual en un archivo JSON.
        /// </summary>
        private static void GuardarConfiguracion()
        {
            // Crea un objeto de configuración con los valores actuales
            var config = new Configuracion
            {
                ModoAccesoDatos = ModoAccesoDatos,
                RutaFicheroTexto = RutaFicheroTexto,
                RutaFicheroBinario = RutaFicheroBinario,
                RutaFicheroJSON = RutaFicheroJSON,
                ConexionMySQL = ConexionMySQL
            };

            try
            {
                // Serializa el objeto de configuración a JSON y guarda en un archivo
                File.WriteAllText(ConfigFile, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
                Console.WriteLine("Configuración guardada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la configuración: {ex.Message}");
            }
        }

        /// <summary>
        /// Solicita un dato al usuario con una opción por defecto.
        /// </summary>
        private static string SolicitarDato(string mensaje, string valorPorDefecto)
        {
            // Muestra el mensaje con el valor por defecto
            Console.Write($"{mensaje} (Default: {valorPorDefecto}): ");
            var entrada = Console.ReadLine();
            // Si el usuario no ingresa nada, se usa el valor por defecto
            return string.IsNullOrEmpty(entrada) ? valorPorDefecto : entrada;
        }

        // Clase interna para manejar la configuración JSON
        private class Configuracion
        {
            // Propiedades que corresponden a los campos en el archivo JSON
            public string ModoAccesoDatos { get; set; }
            public string RutaFicheroTexto { get; set; }
            public string RutaFicheroBinario { get; set; }
            public string RutaFicheroJSON { get; set; }
            public string ConexionMySQL { get; set; }
        }
    }
}
