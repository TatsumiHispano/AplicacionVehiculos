using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tarea2_Persistencia;

public class AccesoDatosFicheroJSON : AccesoDatosAbstract
{
    // Lista interna para almacenar los vehículos cargados desde el archivo JSON
    private List<Vehiculo> vehiculos = new List<Vehiculo>();

    public AccesoDatosFicheroJSON()
    {
        // Llamar a la creación asincrónica del fichero
        CrearFicheroAsync(AppConfig.RutaFicheroJSON).Wait();  // Usamos .Wait() para esperar la tarea asincrónica
        CargarDatosAsync().Wait(); // Cargar los datos también de manera asíncrona
    }

    // Método asincrónico para crear el fichero
    private async Task CrearFicheroAsync(string filePath)
    {
        if (!File.Exists(filePath))// Verificamos si el archivo no existe
        {
            try
            {
                using (FileStream fs = File.Create(filePath))// Si no existe, creamos un archivo vacío
                {
                    await fs.FlushAsync();  // Operación asincrónica para asegurar la escritura en disco
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error al crear el fichero: {e.Message}");
            }
        }
    }

    // Método asincrónico para cargar datos
    private async Task CargarDatosAsync()
    {
        // Verificamos si el archivo no existe
        if (!File.Exists(AppConfig.RutaFicheroJSON))
        {
            // Si no existe, inicializamos la lista de vehículos vacía
            vehiculos = new List<Vehiculo>();
            return;
        }

        try
        {
            // Leemos todo el contenido JSON del archivo
            string jsonData = await File.ReadAllTextAsync(AppConfig.RutaFicheroJSON);
            // Deserializamos el contenido JSON en la lista de vehículos
            vehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(jsonData) ?? new List<Vehiculo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar los datos desde JSON: {ex.Message}");
            vehiculos = new List<Vehiculo>();
        }
    }

    // Método asincrónico para guardar datos
    private async Task GuardarDatosAsync()
    {
        try
        {
            // Serializamos la lista de vehículos a formato JSON con indentación
            string jsonData = JsonConvert.SerializeObject(vehiculos, Newtonsoft.Json.Formatting.Indented);
            // Guardamos el contenido JSON en el archivo
            await File.WriteAllTextAsync(AppConfig.RutaFicheroJSON, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar los datos en JSON: {ex.Message}");
        }
    }

    // Implementación asincrónica de GetAll
    public override async Task<List<Vehiculo>> GetAllAsync()
    {
        // Retornamos una copia de la lista actual, de forma asíncrona
        return await Task.FromResult(new List<Vehiculo>(vehiculos));
    }

    // Implementación asincrónica de GetById
    public override async Task<Vehiculo> GetByIdAsync(string matricula)
    {
        // Buscamos el vehículo de forma asíncrona
        return await Task.FromResult(vehiculos.Find(v => v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase)));
    }

    // Implementación asincrónica de Insert
    public override async Task InsertAsync(Vehiculo vehiculo)
    {
        // Verificamos si el vehículo ya existe en la lista
        if (vehiculos.Exists(v => v.Matricula.Equals(vehiculo.Matricula, StringComparison.OrdinalIgnoreCase)))
        {
            // Si ya existe, mostramos un mensaje y no insertamos el vehículo
            Console.WriteLine("El vehículo con esa matrícula ya existe.");
            return;
        }
        // Si no existe, agregamos el vehículo a la lista
        vehiculos.Add(vehiculo);
        // Guardamos la lista actualizada en el archivo JSON
        await GuardarDatosAsync();
    }

    // Implementación asincrónica de Update
    public override async Task UpdateAsync(Vehiculo vehiculo)
    {
        // Buscamos el índice del vehículo a actualizar según la matrícula
        int index = vehiculos.FindIndex(v => v.Matricula.Equals(vehiculo.Matricula, StringComparison.OrdinalIgnoreCase));
        // Si el vehículo no se encuentra, mostramos un mensaje de error

        if (index == -1)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }
        // Si se encuentra, lo reemplazamos por el nuevo vehículo
        vehiculos[index] = vehiculo;
        // Guardamos los cambios en el archivo JSON
        await GuardarDatosAsync();
    }

    // Implementación asincrónica de Delete
    public override async Task DeleteAsync(Vehiculo vehiculo)
    {
        // Obtenemos el vehículo que vamos a eliminar
        Vehiculo vehiculoAEliminar = await GetByIdAsync(vehiculo.Matricula);
        // Si el vehículo no se encuentra, mostramos un mensaje de error
        if (vehiculoAEliminar == null)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }
        // Si el vehículo se encuentra, lo eliminamos de la lista

        vehiculos.Remove(vehiculoAEliminar);
        // Guardamos los cambios en el archivo JSON
        await GuardarDatosAsync();
    }
}


