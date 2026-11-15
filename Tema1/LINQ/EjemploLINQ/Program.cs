using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploLINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Alumno> alumnos = new List<Alumno>
{
                new Alumno("Ana",   "DAM",  9, 20),
                new Alumno("Luis",  "DAW",  4, 22),
                new Alumno("Marta", "DAM",  7, 19),
                new Alumno("Pablo", "ASIR", 3, 21),
                new Alumno("Lucía", "DAW",  8, 23),
                new Alumno("Jorge", "DAM",  5, 20)
};
            //List<Alumno> aprobados = alumnos.Where(a => a.Nota >= 5).ToList();

            //foreach(Alumno a in aprobados)
            //{
            //    Console.WriteLine(a.Nombre + " - " + a.Nota + " - " + a.Ciclo + " - " + a.Edad);
            //}

            List<string> nombres = alumnos.Select(a => a.Nombre).ToList();

            foreach (string a in nombres)
            {
                Console.WriteLine(a);
            }

            var porNota = alumnos.OrderBy(a => a.Nota).ToList();            // de menor a mayor
            var porNotaDesc = alumnos.OrderByDescending(a => a.Nota).ToList(); // de mayor a menor

            foreach (Alumno a in porNota)
            {
                Console.WriteLine(a);
            }

        }

    }
}
