namespace Tema02_Ficheros.Ejercicios;

// Ejercicio 02 — Explorador de directorio
public static class Ejercicio02
{
    public static void Ejecutar()
    {
        // 1. Pedir la ruta; si se deja vacía o no existe, usamos la carpeta actual
        Console.Write("Ruta de la carpeta (Intro = carpeta actual): ");
        string? ruta = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ruta) || !Directory.Exists(ruta))
        {
            ruta = Directory.GetCurrentDirectory();
        }
        Console.WriteLine($"Explorando: {ruta}");

        // 2. Ficheros .cs solo de esa carpeta
        Console.WriteLine();
        Console.WriteLine("Ficheros .cs (sin subcarpetas):");
        foreach (string fichero in Directory.EnumerateFiles(ruta, "*.cs"))
        {
            Console.WriteLine($"  {Path.GetFileName(fichero)}");
        }

        // 3. Todos los ficheros, incluidas subcarpetas
        Console.WriteLine();
        Console.WriteLine("Todos los ficheros (con subcarpetas):");
        int total = 0;
        foreach (string fichero in Directory.EnumerateFiles(ruta, "*", SearchOption.AllDirectories))
        {
            Console.WriteLine($"  {Path.GetRelativePath(ruta, fichero)}");
            total++;
        }

        // 4. Total
        Console.WriteLine();
        Console.WriteLine($"Total de ficheros: {total}");
    }
}
