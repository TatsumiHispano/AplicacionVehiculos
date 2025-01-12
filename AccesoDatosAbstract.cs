using Colecciones;
using System;
using System.Collections.Generic;
using Tarea2_Persistencia;

public abstract class AccesoDatosAbstract
{
    protected List<Vehiculo> vehiculos = new List<Vehiculo>();
    private static AccesoDatosAbstract instancia;// Instancia de la clase singleton

    // Método estático que retorna la instancia única de la clase de acceso a datos
    public static AccesoDatosAbstract GetInstance()
    {
        // Si la instancia aún no ha sido creada, se crea una nueva según el modo de acceso configurado
        if (instancia == null)
        {
            instancia = AppConfig.ModoAccesoDatos switch
            {
                "MEMORIA" => new AccesoDatosMemoria(),// Si el modo es "MEMORIA", se usa AccesoDatosMemoria
                "FICHERO_TEXTO" => new AccesoDatosFicheroTexto(), // Si el modo es "FICHERO_TEXTO", se usa AccesoDatosFicheroTexto
                "FICHERO_BINARIO" => new AccesoDatosFicheroBinario(), // Si el modo es "FICHERO_BINARIO", se usa AccesoDatosFicheroBinario
                "FICHERO_JSON" => new AccesoDatosFicheroJSON(), // Si el modo es "FICHERO_JSON", se usa AccesoDatosFicheroJSON
                
                _ => new AccesoDatosMemoria() // Valor predeterminado si no coincide ningún caso
            };
        }
        return instancia;// Retorna la instancia única creada
    }

      public abstract Task<List<Vehiculo>> GetAllAsync();// Obtiene todos los vehículos
    public abstract Task<Vehiculo> GetByIdAsync(string matricula);// Obtiene un vehículo por matrícula
    public abstract Task InsertAsync(Vehiculo vehiculo);// Inserta un vehículo
    public abstract Task UpdateAsync(Vehiculo vehiculo);// Actualiza un vehículo
    public abstract Task DeleteAsync(Vehiculo vehiculo);// Elimina un vehículo
}

