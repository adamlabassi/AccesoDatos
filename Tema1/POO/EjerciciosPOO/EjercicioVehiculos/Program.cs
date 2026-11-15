using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioVehiculos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---Coche---");
            Coche objCoche = new Coche("Toyota", "Yaris", 2020, 4);
            objCoche.MostrarInformacion();

            Console.WriteLine("---Moto---");
            Moto objMoto =  new Moto("Honda", "CBR500R", 2021, 20);
            objMoto.MostrarInformacion();

        }
    }
}
