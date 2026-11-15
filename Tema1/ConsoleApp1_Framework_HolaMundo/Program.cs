using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1_Framework_HolaMundo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("Introduce tu edad: ");
            //string? entrada = Console.ReadLine();

            //if (int.TryParse(entrada, out int edad))
            //{
            //    Console.WriteLine($"El año que viene tendrás {edad + 1} años.");
            //}
            //else
            //{
            //    Console.WriteLine("Eso no es un número válido.");
            //}

            int nota = 7;
            string resultado = (nota >= 5) ? "Aprobado" : "Suspenso";
            Console.WriteLine($"El resultado es: {resultado}");


        }
    }
}
