using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> nombres = new List<string> { "ana", "luis", "marta", "pablo" };


            var mayusculas = nombres.Select(n => n.ToUpper());
            Console.WriteLine(string.Join(", ", mayusculas));
            var numeros = nombres.Select(n => n.Length);
            Console.WriteLine(string.Join(", ", numeros));
        }
    }
}
