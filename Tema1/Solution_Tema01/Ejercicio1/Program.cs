using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const decimal iva = 0.21m;
            const decimal gratis = 50;
            const decimal costeEnvio = 4.95m;

            Console.WriteLine("Introduce el nombre del producto: ");
            string nombre = Console.ReadLine();

            Console.WriteLine("Introduce el precio del producto: ");
            string precio = Console.ReadLine();

            Console.WriteLine("Introduce la cantidad del producto: ");
            string cantidad = Console.ReadLine();

            decimal precioDecimal = 0m;
            decimal cantidadDecimal = 0m;

            if (decimal.TryParse(precio, out precioDecimal) && decimal.TryParse(cantidad, out cantidadDecimal) && cantidadDecimal > 0 && precioDecimal > 0)
            {
                //datos válidos, continuar con el cálculo
            }
            else
            {
                Console.WriteLine("Precio o cantidad inválidos.");
            }

            decimal BaseImponible = precioDecimal * cantidadDecimal;

            decimal subTotal = BaseImponible + (BaseImponible * iva);


            if (subTotal > gratis)
            {
                Console.WriteLine("El precio total es: " + subTotal);
            }
            else
            {
                decimal total = subTotal + costeEnvio;
                Console.WriteLine("El precio total es: " + total);

            }
        }
    }
}
