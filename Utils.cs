using System;
using System.Globalization;

public class Utils
{
    public static DateTime ParseFecha(string fecha)
    {
        try
        {
            // Usamos el formato de fecha similar al de Java: "ddd MMM dd HH:mm:ss zzz yyyy"
            DateTime fechaParseada = DateTime.ParseExact(fecha, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            return fechaParseada;
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
            return DateTime.Now;  // Retorna la fecha actual como valor por defecto
        }
    }
}

