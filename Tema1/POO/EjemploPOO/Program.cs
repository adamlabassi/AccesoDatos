using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploPOO
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //var persona = new Persona("Ana", 20);
            //Console.WriteLine(persona.Nombre);        
            //persona.Nombre = "Ana García";

            Perro objPerro = new Perro("Firulais", 3);

            objPerro.HacerSonido();
            

            //Console.ReadKey();


        }
    }
}
