using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tarea2_Persistencia;

public class AccesoDatosFicheroTexto : AccesoDatosAbstract
{
    // Ruta del archivo de texto donde se almacenarán los datos
    private string filePath = AppConfig.RutaFicheroTexto;
    // Constructor que crea el archivo si no existe
    public AccesoDatosFicheroTexto()
    {
        // Crear el archivo si no existe de forma asincrónica
        CrearFicheroAsync(filePath).Wait();
    }

    // Crear el archivo si no existe de forma asincrónica
    private async Task CrearFicheroAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            try
            {
                // Crear el archivo de texto vacío
                using (var fs = File.Create(filePath))
                {
                    await fs.FlushAsync();// Asegurarse de que se escriba en disco
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error al crear el fichero: {e.Message}");
            }
        }
    }

    // Método asincrónico para cargar los datos
    private async Task<List<Vehiculo>> LoadDataAsync()
    {
        List<Vehiculo> vehiculos = new List<Vehiculo>();

        try
        {
            // Leer todas las líneas del archivo de texto
            string[] lines = await File.ReadAllLinesAsync(filePath);

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Parsear cada línea en un objeto Vehiculo
                    Vehiculo vehiculo = Parse(line);
                    vehiculos.Add(vehiculo);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar los datos del archivo de texto: {ex.Message}");
        }

        return vehiculos;
    }

    // Método asincrónico para guardar los datos
    private async Task SaveDataAsync(List<Vehiculo> vehiculos)
    {
        try
        {
            List<string> lines = new List<string>();

            foreach (Vehiculo vehiculo in vehiculos)
            {
                // Convertir cada vehículo a su representación en texto
                lines.Add(Format(vehiculo));
            }
            // Guardar todas las líneas en el archivo de texto
            await File.WriteAllLinesAsync(filePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar los datos en el archivo de texto: {ex.Message}");
        }
    }

    
    // Convertir una línea de texto en un objeto Vehiculo
    private Vehiculo Parse(string line)
    {
        // Dividir la línea en partes usando el delimitador ';'
        string[] parts = line.Split(';');
        // Verificar que la línea tenga el formato esperado
        if (parts.Length != 8)
        {
            throw new Exception("El formato de la línea no es válido.");
        }
        // Crear y devolver un objeto Vehiculo a partir de las partes
        return new Vehiculo(
            parts[0], // Matricula
            parts[1], // Marca
            int.Parse(parts[2]), // NumKms
            DateTime.Parse(parts[3]), // FechaMat
            parts[4], // Descripcion
            int.Parse(parts[5]), // Precio
            parts[6], // Propietario
            parts[7]  // DniPropietario
        );
    }

    // Formato de un objeto Vehiculo a cadena de texto
    private string Format(Vehiculo vehiculo)
    {
        return $"{vehiculo.Matricula};{vehiculo.Marca};{vehiculo.NumKms};{vehiculo.FechaMat};{vehiculo.Descripcion};{vehiculo.Precio};{vehiculo.Propietario};{vehiculo.DniPropietario}";
    }

    // Implementación asincrónica de GetAll
    public override async Task<List<Vehiculo>> GetAllAsync()
    {
        List<Vehiculo> vehiculos = await LoadDataAsync();// Cargar los vehículos desde el archivo
        vehiculos.Sort(); // Ordenar por matrícula usando IComparable
        return vehiculos;
    }

    // Implementación asincrónica de GetById
    public override async Task<Vehiculo> GetByIdAsync(string matricula)
    {
        List<Vehiculo> vehiculos = await LoadDataAsync();// Cargar los vehículos desde el archivo
        return vehiculos.Find(v => v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase));// Buscar el vehículo que coincida con la matrícula
    }

    // Implementación asincrónica de Insert
    public override async Task InsertAsync(Vehiculo vehiculo)
    {
        List<Vehiculo> vehiculos = await LoadDataAsync();// Cargar los vehículos desde el archivo

        // Verificar si el vehículo ya existe
        if (vehiculos.Exists(v => v.Matricula.Equals(vehiculo.Matricula, StringComparison.OrdinalIgnoreCase)))
        {
            throw new Exception("Ya existe un vehículo con la misma matrícula.");
        }
        // Agregar el vehículo a la lista
        vehiculos.Add(vehiculo);
        // Guardar los cambios en el archivo
        await SaveDataAsync(vehiculos);
    }

    // Implementación asincrónica de Update
    public override async Task UpdateAsync(Vehiculo vehiculo)
    {
        List<Vehiculo> vehiculos = await LoadDataAsync();// Cargar los vehículos desde el archivo
                                                        
        // Buscar el índice del vehículo en la lista
        int index = vehiculos.FindIndex(v => v.Matricula.Equals(vehiculo.Matricula, StringComparison.OrdinalIgnoreCase));

        if (index == -1)
        {
            throw new Exception("Vehículo no encontrado.");
        }
        // Reemplazar el vehículo viejo con el nuevo
        vehiculos[index] = vehiculo;
        // Guardar los cambios en el archivo
        await SaveDataAsync(vehiculos);
    }

    // Implementación asincrónica de Delete
    public override async Task DeleteAsync(Vehiculo vehiculo)
    {
        List<Vehiculo> vehiculos = await LoadDataAsync(); // Cargar los vehículos desde el archivo
        // Intentar eliminar el vehículo de la lista
        bool removed = vehiculos.RemoveAll(v => v.Matricula.Equals(vehiculo.Matricula, StringComparison.OrdinalIgnoreCase)) > 0;

        if (!removed)
        {
            throw new Exception("Vehículo no encontrado.");
        }
        // Guardar los cambios en el archivo
        await SaveDataAsync(vehiculos);
    }
}

