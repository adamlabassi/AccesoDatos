namespace Tema02_Ficheros.Ejercicios;

// Ejercicio 01 — Gestión básica de ficheros y carpetas
public static class Ejercicio01
{
    public static void Ejecutar()
    {
        // 1. Crear la carpeta datos si no existe
        string carpeta = "datos";
        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
            Console.WriteLine($"Carpeta '{carpeta}' creada.");
        }

        // 2. Crear saludo.txt si no existe
        string saludo = Path.Combine(carpeta, "saludo.txt");
        if (!File.Exists(saludo))
        {
            File.WriteAllText(saludo, "Hola, Acceso a Datos!");
            Console.WriteLine($"Fichero '{saludo}' creado.");
        }

        // 3. Copiar (sobrescribiendo si ya existiera)
        string copia = Path.Combine(carpeta, "saludo_copia.txt");
        File.Copy(saludo, copia, overwrite: true);
        Console.WriteLine($"Copiado a '{copia}'.");

        // 4. Tamaño en bytes con FileInfo
        Console.WriteLine($"{saludo}: {new FileInfo(saludo).Length} bytes");
        Console.WriteLine($"{copia}: {new FileInfo(copia).Length} bytes");

        // 5. Eliminar la copia
        File.Delete(copia);
        Console.WriteLine($"'{copia}' eliminado. ¿Existe todavía? {File.Exists(copia)}");
    }
}
