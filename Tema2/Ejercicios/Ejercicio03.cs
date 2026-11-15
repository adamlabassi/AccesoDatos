namespace Tema02_Ficheros.Ejercicios;

// Ejercicio 03 — Libreta de notas en texto plano (2.3.1)
public static class Ejercicio03
{
    public static void Ejecutar()
    {
        string fichero = "notas.txt";

        // 1 y 2. Pedir alumno y nota hasta que escriban "fin"
        while (true)
        {
            Console.Write("Nombre del alumno (\"fin\" para terminar): ");
            string? nombre = Console.ReadLine();
            if (nombre == null || nombre.Trim().ToLower() == "fin")
            {
                break;
            }

            Console.Write("Nota: ");
            string? nota = Console.ReadLine();

            // AppendAllText añade al final sin borrar lo que ya hubiera
            File.AppendAllText(fichero, $"{nombre.Trim()},{nota?.Trim()}{Environment.NewLine}");
        }

        // 3. Leer línea a línea y mostrar numeradas
        Console.WriteLine();
        if (!File.Exists(fichero))
        {
            Console.WriteLine("No hay notas registradas.");
            return;
        }

        Console.WriteLine("Notas registradas:");
        int numero = 1;
        using (StreamReader lector = new StreamReader(fichero))
        {
            string? linea;
            while ((linea = lector.ReadLine()) != null)
            {
                string[] partes = linea.Split(',');
                Console.WriteLine($"{numero}. {partes[0]} -> {(partes.Length > 1 ? partes[1] : "")}");
                numero++;
            }
        }
    }
}
