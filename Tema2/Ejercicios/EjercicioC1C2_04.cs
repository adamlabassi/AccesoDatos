namespace Tema02_Ficheros.Ejercicios;

// Ejercicio C1_C2_04 — Limpieza de la carpeta de descargas (2.2.1 y 2.2.2)
public static class EjercicioC1C2_04
{
    public static void Ejecutar()
    {
        // --- Preparación (dada en el enunciado) ---
        string descargas = "descargas";

        if (Directory.Exists(descargas))
        {
            Directory.Delete(descargas, recursive: true);
        }
        Directory.CreateDirectory(descargas);

        string[] ficherosPrueba = { "apuntes.pdf", "foto1.jpg", "practica.zip",
                                    "foto2.jpg", "temp1.tmp", "examen.pdf", "temp2.tmp" };
        foreach (string nombre in ficherosPrueba)
        {
            File.WriteAllText(Path.Combine(descargas, nombre), $"Contenido de {nombre}");
        }

        // 1. Cuántos ficheros hay
        Console.WriteLine($"Ficheros en descargas: {Directory.GetFiles(descargas).Length}");

        // 2. Eliminar los .tmp
        // Usamos GetFiles (array completo) y no EnumerateFiles, porque vamos a
        // borrar/mover ficheros de la misma carpeta que estamos recorriendo.
        string[] temporales = Directory.GetFiles(descargas, "*.tmp");
        Array.Sort(temporales);
        foreach (string fichero in temporales)
        {
            File.Delete(fichero);
            Console.WriteLine($"Eliminado: {Path.GetFileName(fichero)}");
        }

        // 3. Mover cada fichero a una subcarpeta según su extensión (sin el punto)
        string[] restantes = Directory.GetFiles(descargas);
        Array.Sort(restantes);
        foreach (string fichero in restantes)
        {
            string extension = Path.GetExtension(fichero).TrimStart('.').ToLower();
            string carpetaDestino = Path.Combine(descargas, extension);
            Directory.CreateDirectory(carpetaDestino);   // no falla si ya existe

            string nombre = Path.GetFileName(fichero);
            File.Move(fichero, Path.Combine(carpetaDestino, nombre));
            Console.WriteLine($"Movido: {nombre} -> {extension}");
        }

        // 4. Copia de seguridad de la carpeta pdf (fuera de descargas)
        string copia = "copia_pdf";
        Directory.CreateDirectory(copia);
        string[] pdfs = Directory.GetFiles(Path.Combine(descargas, "pdf"));
        foreach (string pdf in pdfs)
        {
            File.Copy(pdf, Path.Combine(copia, Path.GetFileName(pdf)), overwrite: true);
        }
        Console.WriteLine($"Copia de seguridad: {pdfs.Length} fichero(s) en {copia}");

        // 5. Resumen final
        Console.WriteLine();
        Console.WriteLine($"Ficheros sueltos en descargas: {Directory.GetFiles(descargas).Length}");
        string[] subcarpetas = Directory.GetDirectories(descargas);
        Array.Sort(subcarpetas);
        foreach (string sub in subcarpetas)
        {
            Console.WriteLine($"{sub}: {Directory.GetFiles(sub).Length} fichero(s)");
        }

        // 6. Intentar borrar sin recursive: lanza IOException porque no está vacía
        Console.WriteLine();
        try
        {
            Directory.Delete(descargas);
        }
        catch (IOException)
        {
            Console.WriteLine($"No se puede borrar '{descargas}' sin recursive: true (no está vacía).");
        }
    }
}
