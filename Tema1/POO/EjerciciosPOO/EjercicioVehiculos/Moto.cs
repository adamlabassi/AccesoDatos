using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioVehiculos
{
    internal class Moto : Vehiculo
    {

        public int CC { get; set; }

        public Moto(string marca, string modelo, int anio, int cc)
            : base(marca, modelo, anio)
        {
            CC = cc;
        }

        public override void MostrarInformacion()
        {
            base.MostrarInformacion();
            Console.WriteLine($"Cilindradas: {CC}");
        }




    }
}
