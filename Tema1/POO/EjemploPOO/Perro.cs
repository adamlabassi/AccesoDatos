using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploPOO
{
    public class Perro : Animal
    {
        
        public int Edad { get; set; }
        public Perro(string nombre,int edad) : base(nombre) 
        {
            Edad = edad;
        }   // 'base' en vez de 'super'

        public override void HacerSonido()      // 'override' es OBLIGATORIO
        {
            Console.WriteLine($"{Nombre} ladra,  con edad {Edad}");
        }

        public static void objPerro(string nombre, int edad)      // 'override' es OBLIGATORIO
        {
            Perro perro = new Perro(nombre, edad);
            //Console.WriteLine($"{nombre} ladra,  con edad {edad}");
            perro.HacerSonido();
        }
    }
}
