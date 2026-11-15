using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string carpeta = "datos/2dam";

            string fichero = Path.Combine(carpeta, "alumnos.csv");

            string[] rutas = { "foto.PNG", "notas.txt", "logo.gif", "informe.pdf", "playa.Jpg", "LEEME" };
            
        }
    }
}
