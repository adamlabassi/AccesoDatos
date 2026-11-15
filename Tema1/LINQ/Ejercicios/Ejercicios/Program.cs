using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicios
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<int> numeros = new List<int> { 12, 7, 25, 3, 18, 40, 9, 16 };

            var pares = numeros.Where(n => n % 2 == 0);
            
            var mayores10 = numeros.Where(n=> n > 10);

            var entre10y20 = numeros.Where(n => n >= 10 && n <= 20);

            var impares = numeros.Count(n => n % 2 != 0);

            Console.WriteLine("Números pares:");
            //foreach (var numero in pares)
            //{
            //    Console.WriteLine(numero);
            //}
            Console.WriteLine(string.Join(", ", pares));

            Console.WriteLine("Números mayores a 10:");
            Console.WriteLine(string.Join(", ", mayores10));

            Console.WriteLine("Números entre 10 y 20:");
            Console.WriteLine(string.Join(", ", entre10y20));

            Console.WriteLine("Cuantos impares");
            Console.WriteLine(impares);

        }
    }
}
