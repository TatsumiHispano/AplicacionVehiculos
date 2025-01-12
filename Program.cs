using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ConcesionarioMain
{
    private static AccesoDatosAbstract accesoDatos = AccesoDatosAbstract.GetInstance();

    public static async Task Main(string[] args)
    {
        int opcion;

        do
        {
            Console.WriteLine("GESTIÓN DE VEHÍCULOS DE UN CONCESIONARIO");
            Console.WriteLine("1. Nuevo Vehículo.");
            Console.WriteLine("2. Listar Vehículos.");
            Console.WriteLine("3. Buscar Vehículo por matrícula.");
            Console.WriteLine("4. Sumar Kilómetros de Vehículo.");
            Console.WriteLine("5. Eliminar Vehículo.");
            Console.WriteLine("6. Salir.");
            Console.Write("Elige una opción: \n");
            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    await AgregarVehiculo();
                    break;
                case 2:
                    await ListarVehiculos();
                    break;
                case 3:
                    await BuscarVehiculoPorMatricula();
                    break;
                case 4:
                    await SumarKilometros();
                    break;
                case 5:
                    await EliminarVehiculo();
                    break;
                case 6:
                    Console.WriteLine("Saliendo...\n");
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        } while (opcion != 6);
    }
    // Método para agregar un vehículo al concesionario
    private static async Task AgregarVehiculo()
    {
        

        string marca = "Ford";
        string matricula = "1111AAA";
        string descripcion = "coche 1";
        int num_kms = 100000;
        int precio = 3000;
        string propietario = "David Guerrero Gonzalez";
        string dniPropietario = "11111111A";
        DateTime fechaMat = new DateTime(2001, 1, 1);
        // Crear un objeto Vehiculo
        Vehiculo vehiculo = new Vehiculo(matricula, marca, num_kms, fechaMat, descripcion, precio, propietario, dniPropietario);
        // Validar los datos del vehículo antes de almacenarlo
        if (!ValidarVehiculo(vehiculo))
        {
            return;
        }
        // Insertar el vehículo en la base de datos
        await accesoDatos.InsertAsync(vehiculo);
        Console.WriteLine("El vehículo ha sido creado en el concesionario.");
    }

    private static async Task ListarVehiculos()
    {
        // Obtener la lista de vehículos desde la base de datos
        List<Vehiculo> vehiculos = await accesoDatos.GetAllAsync();
        // Imprimir cada vehículo en la lista
        foreach (Vehiculo vehiculo in vehiculos)
        {
            Console.WriteLine(vehiculo.ToString());
        }
    }

    private static async Task BuscarVehiculoPorMatricula()
    {
        Console.Write("Introduce la matrícula: ");
        string matricula = Console.ReadLine();
        // Buscar el vehículo por matrícula en la base de datos
        Vehiculo vehiculo = await accesoDatos.GetByIdAsync(matricula);
        if (vehiculo == null)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }
        Console.WriteLine(vehiculo.ToString());
        // Mostrar los datos del vehículo encontrado
    }

    private static async Task SumarKilometros()
    {
        // Buscar el vehículo por matrícula
        Console.Write("Introduce la matrícula: ");
        string matricula = Console.ReadLine();
        Vehiculo vehiculo = await accesoDatos.GetByIdAsync(matricula);
        if (vehiculo == null)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }
        Console.Write("Introduce los km: ");
        int km = int.Parse(Console.ReadLine());
        // Actualizar los kilómetros del vehículo
        int newKm = vehiculo.NumKms + km;
        vehiculo.NumKms = newKm;
        // Guardar los cambios en la base de datos
        await accesoDatos.UpdateAsync(vehiculo);
        Console.WriteLine(vehiculo.ToString());
    }

    private static async Task EliminarVehiculo()
    {
        Console.Write("Introduce la matrícula: ");
        string matricula = Console.ReadLine();
        Vehiculo vehiculo = await accesoDatos.GetByIdAsync(matricula);
        if (vehiculo == null)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }
        await accesoDatos.DeleteAsync(vehiculo);
        Console.WriteLine("Vehículo eliminado");
    }

    private static bool ValidarVehiculo(Vehiculo vehiculo)
    {
        // Validación de la fecha.
        if (!Validar.ComparaFecha(vehiculo.FechaMat))
        {
            Console.WriteLine("Los datos de la fecha de matriculación son incorrectos o la fecha no es anterior a la actual.");
            return false;
        }
        if (!Validar.ValidaDNI(vehiculo.DniPropietario))
        {
            Console.WriteLine("DNI incorrecto.");
            return false;
        }
        if (!Validar.ValidaMatricula(vehiculo.Matricula))
        {
            Console.WriteLine("Matrícula incorrecta. Formato: NNNNLLL.");
            return false;
        }
        if (!Validar.ValidaNombre(vehiculo.Propietario))
        {
            Console.WriteLine("Propietario incorrecto. Formato: Nombre Apellido1 Apellido2.");
            return false;
        }

        return true;
    }
}
