namespace Tema02_Ficheros.Ejercicios;

// Ejercicio C1_C2_03 — Esqueleto de un proyecto (2.2.1, comprobar y crear)
// Se puede ejecutar varias veces: la segunda vez no crea nada nuevo.
public static class EjercicioC1C2_03
{
    public static void Ejecutar()
    {
        string raiz = "ProyectoDAM";
        string[] subcarpetas = { "docs", "src", "tests" };

        // 1 y 2. Crear carpetas y LEEME.txt si no existen
        foreach (string nombre in subcarpetas)
        {
            string carpeta = Path.Combine(raiz, nombre);

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);   // crea también ProyectoDAM si falta
                Console.WriteLine($"Creada: {carpeta}");
            }
            else
            {
                Console.WriteLine($"Ya existía: {carpeta}");
            }

            string leeme = Path.Combine(carpeta, "LEEME.txt");
            if (!File.Exists(leeme))
            {
                File.WriteAllText(leeme, $"Carpeta {nombre} del ProyectoDAM");
            }
        }

        // 3. Información de cada LEEME.txt con FileInfo
        Console.WriteLine();
        foreach (string nombre in subcarpetas)
        {
            FileInfo info = new FileInfo(Path.Combine(raiz, nombre, "LEEME.txt"));
            Console.WriteLine($"{info.Name} ({nombre}) - {info.Length} bytes - " +
                              $"creado el {info.CreationTime:dd/MM/yyyy HH:mm}");
        }

        // 4. ¿Fichero o carpeta? File solo "ve" ficheros y Directory solo carpetas
        Console.WriteLine();
        string rutaCarpeta = Path.Combine(raiz, "docs");
        string rutaFichero = Path.Combine(raiz, "docs", "LEEME.txt");
        Console.WriteLine($"{rutaCarpeta} -> File.Exists: {File.Exists(rutaCarpeta)} | " +
                          $"Directory.Exists: {Directory.Exists(rutaCarpeta)}");
        Console.WriteLine($"{rutaFichero} -> File.Exists: {File.Exists(rutaFichero)} | " +
                          $"Directory.Exists: {Directory.Exists(rutaFichero)}");
    }
}
