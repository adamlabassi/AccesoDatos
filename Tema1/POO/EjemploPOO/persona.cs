using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploPOO
{
    internal class Persona
    {

        // Auto-propiedad: el compilador genera el campo oculto, el getter y el setter
        public string Nombre { get; set; }

        // Propiedad de solo lectura (equivalente a un getter sin setter en Java)
        public int Edad { get; }

        public Persona(string nombre, int edad)
        {
            Nombre = nombre;
            Edad = edad;   // solo se puede asignar en el constructor: la propiedad es "get-only"
        }

        // Propiedad con lógica personalizada (similar a un getter calculado en Java)
        public bool EsMayorDeEdad => Edad >= 18;

    }
}
