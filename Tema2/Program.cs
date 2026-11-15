using Tema02_Ficheros.Ejercicios;

// Tema 2 — Persistencia en ficheros (apartados 2.1, 2.2 y 2.3)
// Menú para ejecutar cada ejercicio por separado.
// Los ficheros se crean en la carpeta de ejecución (bin\Debug\net10.0\).

Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== TEMA 2 - PERSISTENCIA EN FICHEROS =====");
    Console.WriteLine("Ejercicios de clase (2.1 y 2.2):");
    Console.WriteLine("  1. C1_C2_01 - Analizador de rutas");
    Console.WriteLine("  2. C1_C2_02 - ¿Cuándo se cierra el fichero? (using)");
    Console.WriteLine("  3. C1_C2_03 - Esqueleto de un proyecto");
    Console.WriteLine("  4. C1_C2_04 - Limpieza de la carpeta de descargas");
    Console.WriteLine("  5. C1_C2_05 - Informe de una carpeta");
    Console.WriteLine("Ejercicios del tema (2.1, 2.2 y 2.3):");
    Console.WriteLine("  6. Ejercicio01 - Gestión básica de ficheros y carpetas");
    Console.WriteLine("  7. Ejercicio02 - Explorador de directorio");
    Console.WriteLine("  8. Ejercicio03 - Libreta de notas en texto plano");
    Console.WriteLine("  9. Ejercicio04 - Lector robusto de configuración");
    Console.WriteLine("  0. Salir");
    Console.Write("Opción: ");

    string? opcion = Console.ReadLine();
    Console.WriteLine();

    switch (opcion)
    {
        case "1": EjercicioC1C2_01.Ejecutar(); break;
        case "2": EjercicioC1C2_02.Ejecutar(); break;
        case "3": EjercicioC1C2_03.Ejecutar(); break;
        case "4": EjercicioC1C2_04.Ejecutar(); break;
        case "5": EjercicioC1C2_05.Ejecutar(); break;
        case "6": Ejercicio01.Ejecutar(); break;
        case "7": Ejercicio02.Ejecutar(); break;
        case "8": Ejercicio03.Ejecutar(); break;
        case "9": Ejercicio04.Ejecutar(); break;
        case "0": return;
        default: Console.WriteLine("Opción no válida."); break;
    }
}
