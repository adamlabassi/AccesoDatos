using Microsoft.Data.SqlClient;

namespace Tema03_BaseDatos.Datos;

// 3.3.2 — Cadena de conexión y SqlConnection
// Único sitio del proyecto donde está la cadena de conexión: si cambia el
// servidor o la base de datos, solo se toca aquí.
public static class Conexion
{
    // Servidor:  (localdb)\MSSQLLocalDB  -> instancia LocalDB que instala Visual Studio
    // Catálogo:  Tema03                  -> base de datos con la que trabajamos
    // Integrated Security=true            -> autenticación de Windows (sin usuario/contraseña)
    public const string CadenaConexion =
        @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=Tema03;Integrated Security=true;";

    // Misma instancia, pero contra la base de datos del sistema "master".
    // Solo la usamos para crear Tema03 si todavía no existe.
    private const string CadenaMaster =
        @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=true;";

    // Devuelve una conexión YA ABIERTA. Quien la pide la cierra con using.
    public static SqlConnection Conectar()
    {
        var conexion = new SqlConnection(CadenaConexion);
        try
        {
            conexion.Open();
            return conexion;
        }
        catch (SqlException ex)
        {
            conexion.Dispose();
            throw new InvalidOperationException(
                "No se pudo conectar con la base de datos Tema03 en LocalDB. " +
                "Comprueba que LocalDB está instalado y que la base de datos existe.", ex);
        }
    }

    // Crea la base de datos Tema03 si no existe (equivale a ejecutar
    // CREATE DATABASE Tema03; desde SSMS o el Explorador de objetos SQL).
    public static void CrearBaseDatosSiNoExiste()
    {
        using var conexion = new SqlConnection(CadenaMaster);
        conexion.Open();

        string sql = "IF DB_ID('Tema03') IS NULL CREATE DATABASE Tema03;";
        using var comando = new SqlCommand(sql, conexion);
        comando.ExecuteNonQuery();
    }
}
