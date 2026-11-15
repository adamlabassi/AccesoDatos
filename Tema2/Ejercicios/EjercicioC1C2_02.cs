namespace Tema02_Ficheros.Ejercicios;

// Clase propia que implementa IDisposable para ver cuándo se llama a Dispose()
public class RecursoDemo : IDisposable
{
    private readonly string nombre;

    public RecursoDemo(string nombre)
    {
        this.nombre = nombre;
        Console.WriteLine($"[{nombre}] Abierto");
    }

    public void Usar()
    {
        Console.WriteLine($"[{nombre}] Trabajando...");
    }

    public void Dispose()
    {
        Console.WriteLine($"[{nombre}] Cerrado (Dispose)");
    }
}

// Ejercicio C1_C2_02 — ¿Cuándo se cierra el fichero? (2.1.2, using e IDisposable)
public static class EjercicioC1C2_02
{
    public static void Ejecutar()
    {
        Console.WriteLine("--- Parte A ---");

        // using de bloque: Dispose() se llama al cerrar la llave
        using (RecursoDemo recurso = new RecursoDemo("Bloque"))
        {
            recurso.Usar();
        }
        Console.WriteLine("Después del bloque using");
        Console.WriteLine();

        // Con excepción: Dispose() se llama ANTES de entrar en el catch
        try
        {
            using (RecursoDemo recurso = new RecursoDemo("Excepción"))
            {
                recurso.Usar();
                throw new InvalidOperationException("Fallo a propósito");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Capturada: {ex.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("--- Parte B ---");

        // 4. Escribir con un using de bloque
        using (StreamWriter escritor = new StreamWriter("diario.txt"))
        {
            escritor.WriteLine("Lunes: empezamos el Tema 2");
            escritor.WriteLine("Martes: practicamos con System.IO");
            escritor.WriteLine("Miércoles: ya domino using");
        }

        // 5. Leer con una using declaration (sin llaves): se cierra al acabar el método
        using StreamReader lector = new StreamReader("diario.txt");
        Console.WriteLine("Contenido de diario.txt:");
        Console.Write(lector.ReadToEnd());

        // PREGUNTA: ¿qué pasaría en el paso 5 si el StreamWriter se hubiera creado
        // sin using y sin llamar a Close()/Dispose()?
        // RESPUESTA: StreamWriter guarda lo escrito en un búfer en memoria y solo lo
        // vuelca al disco al hacer Flush/Close/Dispose. Sin cerrarlo, el fichero
        // seguiría abierto y probablemente vacío (o incompleto), así que ReadToEnd()
        // leería "" o solo parte del texto. Además el fichero quedaría bloqueado.
    }
}
