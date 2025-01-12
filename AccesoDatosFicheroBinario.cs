using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tarea2_Persistencia;

public class AccesoDatosFicheroBinario : AccesoDatosAbstract
{
    public AccesoDatosFicheroBinario()
    {
        // Llamadas asincrónicas para crear fichero y cargar datos
        CrearFicheroAsync(AppConfig.RutaFicheroBinario).Wait();  // Usamos .Wait() para esperar la tarea asincrónica
        CargarDatosAsync().Wait(); // Cargar los datos también de manera asíncrona
    }

    // Método asincrónico para crear el fichero
    private async Task CrearFicheroAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            try
            {
                // Creamos el archivo de forma asincrónica si no existe
                using (FileStream fs = File.Create(filePath))
                {
                    await fs.FlushAsync();  // Aseguramos que el archivo se cree en disco de manera asincrónica
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error al crear el fichero: {e.Message}");
            }
        }
    }

    // Método asincrónico para cargar los datos
    private async Task CargarDatosAsync()
    {
        if (!File.Exists(AppConfig.RutaFicheroBinario))
        {
            vehiculos = new List<Vehiculo>();// Si no existe, inicializamos la lista vacía
            return;
        }

        try
        {
            using (FileStream fs = new FileStream(AppConfig.RutaFicheroBinario, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length > 0)
                {
                    // Deserializamos el contenido binario de forma asincrónica usando JsonSerializer
                    vehiculos = await JsonSerializer.DeserializeAsync<List<Vehiculo>>(fs) ?? new List<Vehiculo>();
                }
                else
                {
                    vehiculos = new List<Vehiculo>(); // Si el archivo está vacío, inicializamos la lista vacía
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos: {ex.Message}");
            vehiculos = new List<Vehiculo>();// Si ocurre un error, dejamos la lista vacía
        }
    }

    // Método asincrónico para guardar los datos
    private async Task GuardarDatosAsync()
    {
        try
        {
            using (FileStream fs = new FileStream(AppConfig.RutaFicheroBinario, FileMode.Create, FileAccess.Write))
            {
                // Serializamos los datos de la lista de vehículos de forma asincrónica

                await JsonSerializer.SerializeAsync(fs, vehiculos, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar datos: {ex.Message}");
        }
    }

    // Implementación asincrónica de GetAll
    public override async Task<List<Vehiculo>> GetAllAsync()
    {
        // Retornamos una copia de la lista de vehículos de forma asincrónica
        return await Task.FromResult(new List<Vehiculo>(vehiculos));
    }

    // Implementación asincrónica de GetById
    public override async Task<Vehiculo> GetByIdAsync(string matricula)
    {
        // Buscamos el vehículo de forma asincrónica
        return await Task.FromResult(vehiculos.Find(v => v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase)));
    }

    // Implementación asincrónica de Insert
    public override async Task InsertAsync(Vehiculo vehiculo)
    {
        // Verificamos si el vehículo ya existe en la lista por matrícula
        if (vehiculos.Exists(v => v.Matricula.Equals(vehiculo.Matricula, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("El vehículo con esa matrícula ya existe.");
            return;
        }
        // Si no existe, lo agregamos a la lista
        vehiculos.Add(vehiculo);
        // Guardamos los cambios en el archivo binario
        await GuardarDatosAsync();
    }

    // Implementación asincrónica de Update
    public override async Task UpdateAsync(Vehiculo vehiculo)
    {
        // Buscamos el índice del vehículo en la lista
        int index = vehiculos.FindIndex(v => v.Matricula.Equals(vehiculo.Matricula, StringComparison.OrdinalIgnoreCase));
        if (index == -1)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }
        // Actualizamos el vehículo en la lista
        vehiculos[index] = vehiculo;
        // Guardamos los cambios en el archivo binario
        await GuardarDatosAsync();
    }

    // Implementación asincrónica de Delete
    public override async Task DeleteAsync(Vehiculo vehiculo)
    {
        // Obtenemos el vehículo a eliminar
        Vehiculo vehiculoAEliminar = await GetByIdAsync(vehiculo.Matricula);
        if (vehiculoAEliminar == null)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }
        // Eliminamos el vehículo de la lista
        vehiculos.Remove(vehiculoAEliminar);
        // Guardamos los cambios en el archivo binario
        await GuardarDatosAsync();
    }
}

