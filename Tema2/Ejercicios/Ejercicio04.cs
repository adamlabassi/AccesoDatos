namespace Tema02_Ficheros.Ejercicios;

// Ejercicio 04 — Lector robusto de configuración (2.3.2, excepciones de E/S)
public static class Ejercicio04
{
    public static string LeerConfiguracion(string ruta)
    {
        try
        {
            return File.ReadAllText(ruta);
        }
        catch (FileNotFoundException)
        {
            // Si el fichero no existe, devolvemos un valor por defecto sin caernos
            Console.WriteLine($"Aviso: '{ruta}' no existe, se usa la configuración por defecto.");
            return "modo=normal";
        }
        catch (IOException ex)
        {
            // Cualquier otro error de E/S: avisamos y relanzamos con throw;
            // (throw; conserva la traza original; throw ex; la perdería)
            Console.WriteLine($"Error de E/S leyendo '{ruta}': {ex.Message}");
            throw;
        }
        // Nota: FileNotFoundException hereda de IOException, por eso va ANTES.
    }

    public static void Ejecutar()
    {
        // Preparamos un fichero que sí existe
        string existente = "config.txt";
        File.WriteAllText(existente, "modo=oscuro" + Environment.NewLine + "idioma=es");

        Console.WriteLine("--- Con un fichero que existe ---");
        Console.WriteLine(LeerConfiguracion(existente));

        Console.WriteLine();
        Console.WriteLine("--- Con un fichero que NO existe ---");
        Console.WriteLine(LeerConfiguracion("no_existe.txt"));
    }
}
