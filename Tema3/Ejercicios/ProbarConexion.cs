using Microsoft.Data.SqlClient;
using Tema03_BaseDatos.Datos;

namespace Tema03_BaseDatos.Ejercicios;

// 3.3.3 — Probar la conexión y diagnosticar errores
public static class ProbarConexion
{
    public static void Ejecutar()
    {
        try
        {
            using SqlConnection conexion = Conexion.Conectar();

            Console.WriteLine("Conexión abierta correctamente.");
            Console.WriteLine($"  Estado:        {conexion.State}");
            Console.WriteLine($"  Servidor:      {conexion.DataSource}");
            Console.WriteLine($"  Base de datos: {conexion.Database}");
            Console.WriteLine($"  Versión SQL:   {conexion.ServerVersion}");
        }
        catch (InvalidOperationException ex)
        {
            // Mostramos nuestro mensaje y el de SQL Server, que suele dar la pista:
            //  - "Cannot open database..."   -> la base de datos no existe
            //  - "A network-related..."      -> el servidor está mal escrito o LocalDB no arranca
            Console.WriteLine($"ERROR: {ex.Message}");
            if (ex.InnerException is SqlException sqlEx)
            {
                Console.WriteLine($"  SqlException nº {sqlEx.Number}: {sqlEx.Message}");
            }
        }
        // Al salir del using la conexión se cierra (vuelve al pool de conexiones).
    }

    // 3.3.4 — Abrir y cerrar en cada operación: el pool de conexiones.
    // Abrimos y cerramos varias veces: solo la primera es "lenta", las demás
    // reutilizan la conexión física que el pool mantiene abierta.
    public static void DemoPool()
    {
        for (int i = 1; i <= 5; i++)
        {
            var reloj = System.Diagnostics.Stopwatch.StartNew();
            using (SqlConnection conexion = Conexion.Conectar())
            {
                reloj.Stop();
                Console.WriteLine($"Apertura {i}: {reloj.ElapsedMilliseconds} ms");
            }
        }
        Console.WriteLine("La primera apertura crea la conexión física; el resto la toma del pool.");
    }
}
