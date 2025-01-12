using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarea2_Persistencia;

namespace Colecciones
{
    public class AccesoDatosMySQL : AccesoDatosAbstract
    {
        // Cadena de conexión para conectar con la base de datos MySQL
        private readonly string connectionString;
        // Constructor que recibe la cadena de conexión
        public AccesoDatosMySQL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override async Task<List<Vehiculo>> GetAllAsync()
        {
            var vehiculos = new List<Vehiculo>();// Lista para almacenar los vehículo
            string query = "SELECT * FROM Vehiculo";// Consulta SQL para obtener todos los registros

            using (var conn = new MySqlConnection(connectionString)) // Creamos la conexión
            {
                await conn.OpenAsync();// Abrimos la conexión de manera asíncrona
                using (var cmd = new MySqlCommand(query, conn))// Preparamos el comando SQL
                {
                    using (var reader = await cmd.ExecuteReaderAsync())// Ejecutamos la consulta y obtenemos un lector
                    {
                        while (await reader.ReadAsync()) // Leemos los registros uno por uno
                        {
                            // Convertimos cada registro en un objeto Vehiculo y lo añadimos a la lista
                            vehiculos.Add(new Vehiculo(
                                matricula: reader.GetString("matricula"),
                                marca: reader.GetString("marca"),
                                numKms: reader.GetInt32("numKms"),
                                fechaMat: reader.GetDateTime("fechaMat"),
                                descripcion: reader.GetString("descripcion"),
                                precio: reader.GetInt32("precio"),
                                propietario: reader.GetString("propietario"),
                                dniPropietario: reader.GetString("dniPropietario")
                            ));
                        }
                    }
                }
            }

            return vehiculos;// Retornamos la lista de vehículos
        }

        public override async Task<Vehiculo> GetByIdAsync(string matricula)
        {
            Vehiculo vehiculo = null;// Inicializamos el vehículo como null
            string query = "SELECT * FROM Vehiculo WHERE matricula = @matricula";// Consulta con filtro por matrícula

            using (var conn = new MySqlConnection(connectionString))// Conexión a la base de datos
            {
                await conn.OpenAsync();// Abrimos la conexión
                using (var cmd = new MySqlCommand(query, conn))// Preparamos el comando SQL
                {
                    cmd.Parameters.AddWithValue("@matricula", matricula); // Asignamos el parámetro @matricula para evitar inyección SQL

                    using (var reader = await cmd.ExecuteReaderAsync())// Ejecutamos la consulta
                    {
                        if (await reader.ReadAsync())// Si hay un resultado
                        {
                            // Convertimos el registro en un objeto Vehiculo
                            vehiculo = new Vehiculo(
                                matricula: reader.GetString("matricula"),
                                marca: reader.GetString("marca"),
                                numKms: reader.GetInt32("numKms"),
                                fechaMat: reader.GetDateTime("fechaMat"),
                                descripcion: reader.GetString("descripcion"),
                                precio: reader.GetInt32("precio"),
                                propietario: reader.GetString("propietario"),
                                dniPropietario: reader.GetString("dniPropietario")
                            );
                        }
                    }
                }
            }

            return vehiculo; // Retornamos el vehículo encontrado o null
        }

        public override async Task InsertAsync(Vehiculo vehiculo)
        {
            /// Inserta un nuevo vehículo en la base de datos.
            string query = @"INSERT INTO Vehiculo 
                        (matricula, marca, numKms, fechaMat, descripcion, precio, propietario, dniPropietario) 
                        VALUES 
                        (@matricula, @marca, @numKms, @fechaMat, @descripcion, @precio, @propietario, @dniPropietario)";

            using (var conn = new MySqlConnection(connectionString))// Conexión a MySQL
            {
                await conn.OpenAsync();// Abrimos la conexión
                using (var cmd = new MySqlCommand(query, conn))// Preparamos el comando SQL
                {
                    // Asignamos los valores de los parámetros
                    cmd.Parameters.AddWithValue("@matricula", vehiculo.Matricula);
                    cmd.Parameters.AddWithValue("@marca", vehiculo.Marca);
                    cmd.Parameters.AddWithValue("@numKms", vehiculo.NumKms);
                    cmd.Parameters.AddWithValue("@fechaMat", vehiculo.FechaMat);
                    cmd.Parameters.AddWithValue("@descripcion", vehiculo.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", vehiculo.Precio);
                    cmd.Parameters.AddWithValue("@propietario", vehiculo.Propietario);
                    cmd.Parameters.AddWithValue("@dniPropietario", vehiculo.DniPropietario);
                    // Ejecutamos la consulta de inserción
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public override async Task UpdateAsync(Vehiculo vehiculo)
        {
            /// Actualiza un vehículo existente.
            string query = @"UPDATE Vehiculo 
                        SET marca = @marca, numKms = @numKms, fechaMat = @fechaMat, 
                            descripcion = @descripcion, precio = @precio, 
                            propietario = @propietario, dniPropietario = @dniPropietario 
                        WHERE matricula = @matricula";

            using (var conn = new MySqlConnection(connectionString))// Conexión a MySQL
            {
                await conn.OpenAsync();// Abrimos la conexión
                using (var cmd = new MySqlCommand(query, conn))// Preparamos el comando SQL
                {
                    // Asignamos los valores de los parámetros
                    cmd.Parameters.AddWithValue("@matricula", vehiculo.Matricula);
                    cmd.Parameters.AddWithValue("@marca", vehiculo.Marca);
                    cmd.Parameters.AddWithValue("@numKms", vehiculo.NumKms);
                    cmd.Parameters.AddWithValue("@fechaMat", vehiculo.FechaMat);
                    cmd.Parameters.AddWithValue("@descripcion", vehiculo.Descripcion);
                    cmd.Parameters.AddWithValue("@precio", vehiculo.Precio);
                    cmd.Parameters.AddWithValue("@propietario", vehiculo.Propietario);
                    cmd.Parameters.AddWithValue("@dniPropietario", vehiculo.DniPropietario);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public override async Task DeleteAsync(Vehiculo vehiculo)
        {
            /// Elimina un vehículo de la base de datos.
            string query = "DELETE FROM Vehiculo WHERE matricula = @matricula";// Consulta SQL para eliminar

            using (var conn = new MySqlConnection(connectionString))// Conexión a MySQL
            {
                await conn.OpenAsync();// Abrimos la conexión
                using (var cmd = new MySqlCommand(query, conn))// Preparamos el comando SQL
                {
                    // Asignamos el parámetro @matricula
                    cmd.Parameters.AddWithValue("@matricula", vehiculo.Matricula);
                    await cmd.ExecuteNonQueryAsync();// Ejecutamos la consulta de eliminación
                }
            }
        }
    }
}