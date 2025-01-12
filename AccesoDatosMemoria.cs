using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AccesoDatosMemoria : AccesoDatosAbstract
{
    // Simulación de la lista en memoria
    private List<Vehiculo> vehiculos = new List<Vehiculo>();

    // Método asincrónico para eliminar un vehículo
    public override async Task DeleteAsync(Vehiculo vehiculo)
    {
        await Task.Run(() =>
        {
            // Eliminamos el vehículo de la lista usando el método Remove
            vehiculos.Remove(vehiculo);
        });
    }

    // Método asincrónico para obtener todos los vehículos
    public override async Task<List<Vehiculo>> GetAllAsync()
    {
        return await Task.Run(() =>
        {
            // Ordenamos los vehículos por matrícula de forma ascendente
            return vehiculos.OrderBy(v => v.Matricula).ToList();
        });
    }

    // Método asincrónico para obtener un vehículo por su matrícula
    public override async Task<Vehiculo> GetByIdAsync(string matricula)
    {
        return await Task.Run(() =>
        {
            // Buscamos el vehículo en la lista que tenga la matrícula solicitada (ignorando mayúsculas/minúsculas)
            return vehiculos.Find(v => v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase));
        });
    }

    // Método asincrónico para insertar un vehículo
    public override async Task InsertAsync(Vehiculo vehiculo)
    {
        await Task.Run(() =>
        {
            // Agregamos el vehículo a la lista en memoria
            vehiculos.Add(vehiculo);
        });
    }

    // Método asincrónico para actualizar un vehículo
    public override async Task UpdateAsync(Vehiculo vehiculo)
    {
        await Task.Run(() =>
        {
            // Obtenemos el vehículo a actualizar usando la matrícula
            var v = GetByIdAsync(vehiculo.Matricula).Result;
            if (v != null)// Si el vehículo existe, se actualizan los valores
            {
                v.Marca = vehiculo.Marca;
                v.NumKms = vehiculo.NumKms;
                v.FechaMat = vehiculo.FechaMat;
                v.Descripcion = vehiculo.Descripcion;
                v.Precio = vehiculo.Precio;
                v.Propietario = vehiculo.DniPropietario;
            }
        });
    }
}
