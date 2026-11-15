using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploPOO
{
    public class Animal
    {
        public string Nombre { get; set; }

        public Animal(string nombre) => Nombre = nombre;

        public virtual void HacerSonido()      // 'virtual' = se puede sobrescribir
        {
            Console.WriteLine($"{Nombre} hace un sonido.");
        }
    }
}
