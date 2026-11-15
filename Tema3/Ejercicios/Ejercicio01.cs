using Microsoft.Data.SqlClient;
using Tema03_BaseDatos.Datos;

namespace Tema03_BaseDatos.Ejercicios;

// Ejercicio 01 — Primera conexión y tabla
public static class Ejercicio01
{
    // 1. Abre una SqlConnection a LocalDB y la devuelve ya abierta.
    //    Si falla, captura SqlException y relanza con un mensaje claro.
    public static SqlConnection Conectar()
    {
        var conexion = new SqlConnection(Conexion.CadenaConexion);
        try
        {
            conexion.Open();
            return conexion;
        }
        catch (SqlException ex)
        {
            conexion.Dispose();
            throw new InvalidOperationException(
                "No se ha podido conectar a (localdb)\\MSSQLLocalDB / Tema03.", ex);
        }
    }

    public static void Ejecutar()
    {
        try
        {
            using SqlConnection conexion = Conectar();
            Console.WriteLine("Conexión con LocalDB abierta correctamente.");

            // 2. Equivalente a CREATE TABLE IF NOT EXISTS en SQL Server
            string sql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Personas' AND xtype = 'U')
                CREATE TABLE Personas (
                    Id     INT IDENTITY(1,1) PRIMARY KEY,
                    Nombre NVARCHAR(100),
                    Edad   INT
                );";

            using SqlCommand comando = new SqlCommand(sql, conexion);
            comando.ExecuteNonQuery();   // DDL: no devuelve filas

            // 3. Comprobamos que la tabla existe
            using SqlCommand comprobar = new SqlCommand(
                "SELECT COUNT(*) FROM sysobjects WHERE name = 'Personas' AND xtype = 'U'", conexion);
            int existe = (int)comprobar.ExecuteScalar();

            Console.WriteLine(existe == 1
                ? "Tabla 'Personas' lista (creada o ya existía)."
                : "Algo ha ido mal: la tabla 'Personas' no existe.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"  Detalle: {ex.InnerException.Message}");
            }
        }
    }
}
