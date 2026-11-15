namespace Tema02_Ficheros.Ejercicios;

// Ejercicio C1_C2_01 — Analizador de rutas (2.1.1, la clase Path)
// Path solo trabaja con el TEXTO de la ruta: no crea ni comprueba nada en disco.
public static class EjercicioC1C2_01
{
    public static void Ejecutar()
    {
        // 1. Construir la ruta con Path.Combine (nunca concatenando "\\")
        string ruta = Path.Combine("datos", "2DAM", "alumnos.csv");
        Console.WriteLine($"Ruta completa: {ruta}");

        // 2. Partes de la ruta
        Console.WriteLine($"Nombre: {Path.GetFileName(ruta)}");
        Console.WriteLine($"Sin extensión: {Path.GetFileNameWithoutExtension(ruta)}");
        Console.WriteLine($"Extensión: {Path.GetExtension(ruta)}");
        Console.WriteLine($"Carpeta: {Path.GetDirectoryName(ruta)}");

        // 3. Cambiar la extensión
        Console.WriteLine($"Como JSON: {Path.ChangeExtension(ruta, ".json")}");

        // 4. Carpeta actual y ruta absoluta
        Console.WriteLine();
        Console.WriteLine($"Carpeta actual: {Directory.GetCurrentDirectory()}");
        Console.WriteLine($"Ruta absoluta: {Path.GetFullPath(ruta)}");

        // 5. ¿Es una imagen?
        Console.WriteLine();
        string[] rutas = { "foto.PNG", "notas.txt", "logo.gif", "informe.pdf", "playa.Jpg", "LEEME" };
        foreach (string r in rutas)
        {
            // Para "LEEME" GetExtension devuelve "" (cadena vacía), no null
            string extension = Path.GetExtension(r).ToLower();
            bool esImagen = extension == ".png" || extension == ".jpg" || extension == ".gif";
            Console.WriteLine($"{r} -> {(esImagen ? "imagen" : "no es imagen")}");
        }
    }
}
