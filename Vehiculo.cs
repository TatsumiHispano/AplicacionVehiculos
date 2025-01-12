using System;

public class Vehiculo : IComparable<Vehiculo>
{
    public string Matricula { get; set; }
    public string Marca { get; set; }
    public int NumKms { get; set; }
    public DateTime FechaMat { get; set; }
    public string Descripcion { get; set; }
    public int Precio { get; set; }
    public string Propietario { get; set; }
    public string DniPropietario { get; set; }

    // Constructor
    public Vehiculo(string matricula, string marca, int numKms, DateTime fechaMat, string descripcion, int precio, string propietario, string dniPropietario)
    {
        this.Matricula = matricula;
        this.Marca = marca;
        this.NumKms = numKms;
        this.FechaMat = fechaMat;
        this.Descripcion = descripcion;
        this.Precio = precio;
        this.Propietario = propietario;
        this.DniPropietario = dniPropietario;
    }

    // Implementación de CompareTo para comparar matrículas

    public int CompareTo(Vehiculo other) => Matricula.CompareTo(other.Matricula);
   
    public override string ToString()
    {
        return $"Vehiculo [Matrícula: {Matricula}, Marca: {Marca}, Kms: {NumKms}, FechaMat: {FechaMat}, Descripción: {Descripcion}, Precio: {Precio}, Propietario: {Propietario}, DNI Propietario: {DniPropietario}]";
    }
}

