using System;
using System.Globalization;
using System.Text.RegularExpressions;

public class Validar
{
    public static bool ComparaFecha(DateTime fechaMat)
    {
        // Obtener la fecha actual sin la hora (se usa DateTime.Today)
        DateTime hoy = DateTime.Today;

        // Comparar si la fechaMat es antes de hoy
        return fechaMat < hoy;
    }

    public static bool ValidaDNI(string dni)
    {
        // Versión simple para validar un DNI (8 dígitos seguidos de una letra mayúscula)
        return Regex.IsMatch(dni, "^[0-9]{8}[A-Z]$");
    }

    public static bool ValidaMatricula(string matricula)
    {
        // Versión simple para validar una matrícula (4 dígitos seguidos de 3 letras mayúsculas)
        return Regex.IsMatch(matricula, "^[0-9]{4}[A-Z]{3}$");
    }

    public static bool ValidaNombre(string nombre)
    {
        if (nombre.Length > 40) return false; // Comprobar que el tamaño no supera los 40 caracteres

        int posicion = nombre.IndexOf(" "); // Buscar el primer espacio en blanco.

        if (posicion == -1) return false; // Si no existe
        else
        {
            posicion = nombre.IndexOf(" ", posicion + 1); // Buscar el siguiente espacio en blanco.
            if (posicion == -1) return false; // Si no lo encontramos
        }

        return true;
    }
}
