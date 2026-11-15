namespace Tema02_Ficheros.Ejercicios;

// Ejercicio C1_C2_05 — Informe de una carpeta (2.2.2, recorrido recursivo)
public static class EjercicioC1C2_05
{
    public static void Ejecutar()
    {
        // --- Preparación (dada en el enunciado) ---
        string raiz = "instituto";
        if (Directory.Exists(raiz))
        {
            Directory.Delete(raiz, recursive: true);
        }

        var ficheros = new Dictionary<string, int>
        {
            { Path.Combine("1DAM", "programacion.txt"),        120 },
            { Path.Combine("1DAM", "bases_datos.txt"),          80 },
            { Path.Combine("1DAM", "horario.pdf"),             300 },
            { Path.Combine("2DAM", "acceso_datos.txt"),        150 },
            { Path.Combine("2DAM", "proyectos", "tfc.pdf"),    900 },
            { Path.Combine("2DAM", "proyectos", "notas.csv"),   60 },
            { "normas.pdf",                                    200 }
        };

        foreach (var par in ficheros)
        {
            string ruta = Path.Combine(raiz, par.Key);
            Directory.CreateDirectory(Path.GetDirectoryName(ruta)!);
            File.WriteAllText(ruta, new string('x', par.Value));
        }

        // 1. Subcarpetas (recursivo)
        Console.WriteLine("Subcarpetas:");
        var carpetas = Directory.EnumerateDirectories(raiz, "*", SearchOption.AllDirectories)
                                .OrderBy(c => c);
        foreach (string carpeta in carpetas)
        {
            Console.WriteLine($"  {Path.GetRelativePath(raiz, carpeta)}");
        }

        // 2. Ficheros con ruta relativa y tamaño
        Console.WriteLine();
        Console.WriteLine("Ficheros:");
        var rutasFicheros = Directory.EnumerateFiles(raiz, "*", SearchOption.AllDirectories)
                                     .OrderBy(r => r);

        int total = 0;
        long tamanoTotal = 0;
        FileInfo? masGrande = null;

        foreach (string ruta in rutasFicheros)
        {
            FileInfo info = new FileInfo(ruta);
            Console.WriteLine($"  {Path.GetRelativePath(raiz, ruta)} ({info.Length} bytes)");

            total++;
            tamanoTotal += info.Length;
            if (masGrande == null || info.Length > masGrande.Length)
            {
                masGrande = info;
            }
        }

        // 3 y 4. Totales y fichero más grande
        Console.WriteLine();
        Console.WriteLine($"Total: {total} ficheros, {tamanoTotal} bytes");
        if (masGrande != null)
        {
            Console.WriteLine($"Más grande: {masGrande.Name} ({masGrande.Length} bytes)");
        }

        // 5. Número de PDF con patrón de búsqueda
        int pdfs = Directory.EnumerateFiles(raiz, "*.pdf", SearchOption.AllDirectories).Count();
        Console.WriteLine($"Ficheros PDF: {pdfs}");

        // Reto opcional: tamaño de cada subcarpeta directa (incluyendo su contenido)
        Console.WriteLine();
        foreach (string sub in Directory.GetDirectories(raiz).OrderBy(s => s))
        {
            long tamano = Directory.EnumerateFiles(sub, "*", SearchOption.AllDirectories)
                                   .Sum(f => new FileInfo(f).Length);
            Console.WriteLine($"{Path.GetFileName(sub)}: {tamano} bytes");
        }
    }
}
