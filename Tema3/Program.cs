using Tema03_BaseDatos.Datos;
using Tema03_BaseDatos.Ejercicios;

// Tema 3 — Persistencia en bases de datos (apartados 3.1, 3.2 y 3.3)
// La parte de SSMS (3.2) está en Scripts\biblioteca.sql.
// Aquí está la parte de C#: conexión con SQL Server LocalDB (3.3) y el Ejercicio 01.

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Creamos la base de datos Tema03 si aún no existe, para no tener que hacerlo a mano.
try
{
    Conexion.CrearBaseDatosSiNoExiste();
}
catch (Exception ex)
{
    Console.WriteLine($"No se pudo crear/comprobar la base de datos Tema03: {ex.Message}");
}

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== TEMA 3 - PERSISTENCIA EN BASE DE DATOS =====");
    Console.WriteLine("  1. Probar la conexión con LocalDB (3.3.3)");
    Console.WriteLine("  2. Pool de conexiones: abrir y cerrar en cada operación (3.3.4)");
    Console.WriteLine("  3. Ejercicio01 - Primera conexión y tabla Personas");
    Console.WriteLine("  0. Salir");
    Console.Write("Opción: ");

    string? opcion = Console.ReadLine();
    Console.WriteLine();

    switch (opcion)
    {
        case "1": ProbarConexion.Ejecutar(); break;
        case "2": ProbarConexion.DemoPool(); break;
        case "3": Ejercicio01.Ejecutar(); break;
        case "0": return;
        default: Console.WriteLine("Opción no válida."); break;
    }
}
