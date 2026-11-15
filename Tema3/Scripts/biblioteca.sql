/* =====================================================================
   Tema 3 — Actividad de clase: La biblioteca municipal en SQL Server
   (apartado 3.2 — SQL Server Management Studio)

   Ejecutar en SSMS conectado a (localdb)\MSSQLLocalDB con autenticación
   de Windows. Antes: copiar prestamos_biblioteca.txt a C:\Datos\
   (las rutas dentro de OneDrive suelen dar problemas al importar).
   ===================================================================== */


/* =====================================================================
   PARTE 1 — La base de datos
   ===================================================================== */

-- Se puede ejecutar varias veces: solo la crea si no existe
IF DB_ID('Biblioteca') IS NULL
    CREATE DATABASE Biblioteca;
GO

USE Biblioteca;
GO


/* =====================================================================
   PARTE 2 — La tabla y la importación
   ===================================================================== */

-- 2.1 Crear la tabla
IF OBJECT_ID('Prestamos', 'U') IS NULL
CREATE TABLE Prestamos (
    IdPrestamo              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CodigoSocio             VARCHAR(10)   NOT NULL,
    NombreSocio             NVARCHAR(100) NOT NULL,
    ISBN                    CHAR(13)      NOT NULL,
    Titulo                  NVARCHAR(150) NOT NULL,
    Autor                   NVARCHAR(100) NOT NULL,
    Genero                  NVARCHAR(20)  NOT NULL,
    FechaPrestamo           DATE          NOT NULL,
    FechaDevolucionPrevista DATE          NOT NULL,
    FechaDevolucionReal     DATE          NULL,
    CONSTRAINT CK_Prestamos_Fechas CHECK (FechaDevolucionPrevista > FechaPrestamo)
);
GO

-- P1. ¿Por qué FechaDevolucionReal es la única columna que admite nulos?
--     Porque es el único dato que no se conoce al registrar el préstamo: mientras
--     el libro está fuera no hay fecha de devolución. Un NULL significa
--     "el libro todavía no se ha devuelto" (el préstamo está pendiente).

-- P2. ¿Por qué NVARCHAR para el título y VARCHAR para el código del socio?
--     NVARCHAR guarda Unicode (tildes, eñes y cualquier alfabeto) a costa de ocupar
--     el doble. El título puede llevar "Cómic", "Núñez"... El código del socio
--     siempre tiene el formato S0001 (letras y números ASCII), así que VARCHAR
--     basta y ocupa la mitad.

-- 2.2 Importar los datos
--   KEEPIDENTITY  -> conserva los IdPrestamo del fichero
--   CODEPAGE 65001 -> el fichero está en UTF-8
--   FIRSTROW = 2  -> saltamos la línea de cabecera
--   El último campo vacío (libro sin devolver) se importa como NULL.
IF NOT EXISTS (SELECT 1 FROM Prestamos)
BULK INSERT Prestamos
FROM 'C:\Datos\prestamos_biblioteca.txt'
WITH (
    CODEPAGE        = '65001',
    FIELDTERMINATOR = ';',
    ROWTERMINATOR   = '\n',
    FIRSTROW        = 2,
    KEEPIDENTITY,
    TABLOCK
);
GO

-- 2.3 Comprobar la importación
SELECT COUNT(*) AS TotalPrestamos FROM Prestamos;                         -- 60000
SELECT IDENT_CURRENT('Prestamos') AS IdentCurrent;                        -- 60000
SELECT COUNT(*) AS Pendientes FROM Prestamos
WHERE FechaDevolucionReal IS NULL;                                        -- 698
SELECT COUNT(DISTINCT CodigoSocio) AS SociosDistintos,                    -- 3954
       COUNT(DISTINCT ISBN)        AS LibrosDistintos                     -- 650
FROM Prestamos;
SELECT Genero, COUNT(*) AS Prestamos
FROM Prestamos
GROUP BY Genero
ORDER BY Prestamos DESC;
GO

-- P3. ¿Para qué sirve comprobar IDENT_CURRENT?
--     Para saber cuál fue el último valor de identidad. Como hemos importado con
--     KEEPIDENTITY, debe ser 60000: así el próximo préstamo que se registre
--     recibirá el 60001 y no chocará con ningún Id importado.


/* =====================================================================
   PARTE 3 — El mostrador va lento: un índice
   ===================================================================== */

SET STATISTICS IO ON;   -- en la pestaña Mensajes aparecen las lecturas lógicas
-- Activar también "Incluir plan de ejecución real" (Ctrl+M)

-- Antes del índice: Clustered Index Scan, unas 1.400-1.500 lecturas lógicas
SELECT IdPrestamo, Titulo, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal
FROM Prestamos
WHERE CodigoSocio = 'S2966'
ORDER BY FechaPrestamo DESC;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Prestamos_CodigoSocio')
    CREATE NONCLUSTERED INDEX IX_Prestamos_CodigoSocio ON Prestamos (CodigoSocio);
GO

-- Después del índice: Index Seek + Key Lookup, menos de 100 lecturas lógicas
SELECT IdPrestamo, Titulo, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal
FROM Prestamos
WHERE CodigoSocio = 'S2966'
ORDER BY FechaPrestamo DESC;
GO

SET STATISTICS IO OFF;
GO

-- P4. Sin índice, ¿por qué hay que leer la tabla entera?
--     Porque la tabla está ordenada (índice clúster) por IdPrestamo, no por
--     CodigoSocio. SQL Server no sabe dónde están las filas de S2966, así que
--     tiene que revisar las 60.000 filas una a una para no dejarse ninguna.

-- P5. ¿Por qué no hace falta un índice sobre IdPrestamo?
--     Porque es la PRIMARY KEY y SQL Server crea automáticamente un índice
--     clúster sobre ella: las búsquedas por IdPrestamo ya son rápidas.

-- P6. ¿Ayudaría un índice sobre Genero?
--     Mucho menos. Genero solo tiene 10 valores distintos y cada uno devuelve
--     miles de filas (Novela, 18.253), así que a SQL Server le sale más barato
--     recorrer la tabla que hacer miles de Key Lookup. CodigoSocio es muy
--     selectivo (~15 filas por socio). Además cada índice ocupa espacio y hace
--     más lentos los INSERT/UPDATE/DELETE, porque también hay que actualizarlo.


/* =====================================================================
   PARTE 4 — Informes para la dirección: vistas
   ===================================================================== */

-- 4.1 Préstamos pendientes (sin devolver)
CREATE OR ALTER VIEW vw_PrestamosPendientes
AS
SELECT IdPrestamo,
       CodigoSocio,
       NombreSocio,
       Titulo,
       FechaPrestamo,
       FechaDevolucionPrevista,
       DATEDIFF(DAY, FechaDevolucionPrevista, GETDATE()) AS DiasVencido
FROM Prestamos
WHERE FechaDevolucionReal IS NULL;
GO

-- 1. ¿Cuántos libros hay fuera?  -> 698
SELECT COUNT(*) AS LibrosFuera FROM vw_PrestamosPendientes;
-- 2. ¿Cuántos están vencidos?
SELECT COUNT(*) AS Vencidos FROM vw_PrestamosPendientes WHERE DiasVencido > 0;
-- 3. Los 10 con más retraso (los primeros: 109 y 125; el décimo: 1714)
SELECT TOP 10 *
FROM vw_PrestamosPendientes
ORDER BY DiasVencido DESC, IdPrestamo;
GO

-- 4.2 Estadísticas por género
CREATE OR ALTER VIEW vw_EstadisticasGenero
AS
SELECT Genero,
       COUNT(*)                     AS TotalPrestamos,
       COUNT(FechaDevolucionReal)   AS Devueltos,           -- COUNT(col) no cuenta NULL
       SUM(CASE WHEN FechaDevolucionReal > FechaDevolucionPrevista
                THEN 1 ELSE 0 END)  AS DevueltosConRetraso,
       CAST(100.0 * SUM(CASE WHEN FechaDevolucionReal > FechaDevolucionPrevista
                             THEN 1 ELSE 0 END)
                  / COUNT(FechaDevolucionReal) AS DECIMAL(5,1)) AS PorcentajeRetraso
FROM Prestamos
GROUP BY Genero;
GO

SELECT *
FROM vw_EstadisticasGenero
ORDER BY PorcentajeRetraso DESC;
GO

-- P7. ¿Qué ocurre con ORDER BY dentro de la vista?
--     Da error: una vista no puede llevar ORDER BY (salvo con TOP/OFFSET), porque
--     una vista es como una tabla y las tablas no tienen un orden garantizado.
--     El ORDER BY se pone en el SELECT que consulta la vista.

-- P8. ¿A qué géneros ampliarías el plazo?
--     A Cómic (38,3 % de retrasos) e Infantil (35,3 %), y probablemente Juvenil
--     (31,1 %): son los que más se devuelven tarde, muy por encima del resto
--     (Ensayo o Ciencia están en torno al 12 %).

-- P9. ¿Qué ventaja tiene usar la vista?
--     La directora no necesita saber SQL ni entender el GROUP BY: hace
--     SELECT * FROM vw_EstadisticasGenero. La lógica está escrita una vez, en la
--     base de datos; si hay que corregirla se cambia la vista y todos ven el
--     resultado nuevo, sin riesgo de que cada uno copie una versión distinta.


/* =====================================================================
   PARTE 5 — Las normas de préstamo: procedimientos almacenados
   ===================================================================== */

-- 5.1 Préstamos de un socio, con su estado
CREATE OR ALTER PROCEDURE usp_PrestamosSocio
    @CodigoSocio VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdPrestamo,
           Titulo,
           FechaPrestamo,
           FechaDevolucionPrevista,
           FechaDevolucionReal,
           CASE
               WHEN FechaDevolucionReal IS NULL                    THEN 'Pendiente'
               WHEN FechaDevolucionReal > FechaDevolucionPrevista  THEN 'Devuelto con retraso'
               ELSE 'Devuelto'
           END AS Estado
    FROM Prestamos
    WHERE CodigoSocio = @CodigoSocio
    ORDER BY FechaPrestamo DESC;
END;
GO

-- 5.2 Registrar un préstamo aplicando las normas de la biblioteca
CREATE OR ALTER PROCEDURE usp_RegistrarPrestamo
    @CodigoSocio VARCHAR(10),
    @ISBN        CHAR(13)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NombreSocio NVARCHAR(100),
            @Titulo      NVARCHAR(150),
            @Autor       NVARCHAR(100),
            @Genero      NVARCHAR(20);

    -- 1. El socio existe (de momento lo buscamos en Prestamos)
    SELECT TOP 1 @NombreSocio = NombreSocio
    FROM Prestamos
    WHERE CodigoSocio = @CodigoSocio;

    IF @NombreSocio IS NULL
    BEGIN
        SELECT 'ERROR: el socio ' + @CodigoSocio + ' no existe' AS Mensaje;
        RETURN;
    END;

    -- 2. El libro existe
    SELECT TOP 1 @Titulo = Titulo, @Autor = Autor, @Genero = Genero
    FROM Prestamos
    WHERE ISBN = @ISBN;

    IF @Titulo IS NULL
    BEGIN
        SELECT 'ERROR: el libro ' + @ISBN + ' no existe' AS Mensaje;
        RETURN;
    END;

    -- 3. No tiene libros vencidos
    IF EXISTS (SELECT 1 FROM vw_PrestamosPendientes
               WHERE CodigoSocio = @CodigoSocio AND DiasVencido > 0)
    BEGIN
        SELECT 'DENEGADO: el socio tiene libros vencidos sin devolver' AS Mensaje;
        RETURN;
    END;

    -- 4. No tiene ya 3 libros
    IF (SELECT COUNT(*) FROM vw_PrestamosPendientes WHERE CodigoSocio = @CodigoSocio) >= 3
    BEGIN
        SELECT 'DENEGADO: el socio ya tiene 3 libros en préstamo' AS Mensaje;
        RETURN;
    END;

    -- Todo correcto: registramos el préstamo (15 días de plazo)
    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);
    DECLARE @Prevista DATE = DATEADD(DAY, 15, @Hoy);

    INSERT INTO Prestamos (CodigoSocio, NombreSocio, ISBN, Titulo, Autor, Genero,
                           FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal)
    VALUES (@CodigoSocio, @NombreSocio, @ISBN, @Titulo, @Autor, @Genero,
            @Hoy, @Prevista, NULL);

    DECLARE @NuevoId INT = CAST(SCOPE_IDENTITY() AS INT);

    SELECT 'OK: préstamo ' + CAST(@NuevoId AS NVARCHAR(10)) +
           ' registrado. Devolver antes del ' + CONVERT(NVARCHAR(10), @Prevista, 103) AS Mensaje;
END;
GO

-- 5.3 Registrar una devolución
CREATE OR ALTER PROCEDURE usp_RegistrarDevolucion
    @IdPrestamo INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Prevista DATE, @Real DATE;

    SELECT @Prevista = FechaDevolucionPrevista, @Real = FechaDevolucionReal
    FROM Prestamos
    WHERE IdPrestamo = @IdPrestamo;

    -- 1. El préstamo existe
    IF @Prevista IS NULL
    BEGIN
        SELECT 'ERROR: el préstamo ' + CAST(@IdPrestamo AS NVARCHAR(10)) + ' no existe' AS Mensaje;
        RETURN;
    END;

    -- 2. No está ya devuelto
    IF @Real IS NOT NULL
    BEGIN
        SELECT 'ERROR: el préstamo ya se devolvió el ' + CONVERT(NVARCHAR(10), @Real, 103) AS Mensaje;
        RETURN;
    END;

    -- 3. Registrar la devolución con la fecha de hoy
    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

    UPDATE Prestamos
    SET FechaDevolucionReal = @Hoy
    WHERE IdPrestamo = @IdPrestamo;

    DECLARE @Retraso INT = DATEDIFF(DAY, @Prevista, @Hoy);

    IF @Retraso > 0
        SELECT 'Devuelto con ' + CAST(@Retraso AS NVARCHAR(10)) + ' días de retraso' AS Mensaje;
    ELSE
        SELECT 'Devuelto en plazo. ¡Gracias!' AS Mensaje;
END;
GO

-- 5.4 Batería de pruebas (en este orden)
-- Para repetirlas desde cero, descomentar estas tres líneas:
-- DELETE FROM Prestamos WHERE IdPrestamo > 60000;
-- UPDATE Prestamos SET FechaDevolucionReal = NULL WHERE IdPrestamo = 40065;
-- DBCC CHECKIDENT ('Prestamos', RESEED, 60000);

EXEC usp_PrestamosSocio @CodigoSocio = 'S2966';                 -- 21 filas, la primera la 58941 Pendiente

EXEC usp_RegistrarPrestamo 'S0003', '9788457144799';             -- 1. OK (60001)
EXEC usp_RegistrarPrestamo 'S0003', '9788427660052';             -- 2. OK (60002)
EXEC usp_RegistrarPrestamo 'S0003', '9788497402040';             -- 3. OK (60003)
EXEC usp_RegistrarPrestamo 'S0003', '9788467179682';             -- 4. DENEGADO: ya tiene 3 libros
EXEC usp_RegistrarPrestamo 'S2157', '9788457144799';             -- 5. DENEGADO: libros vencidos
EXEC usp_RegistrarDevolucion 40065;                              -- 6. Devuelto con N días de retraso
EXEC usp_RegistrarDevolucion 40065;                              -- 7. ERROR: ya se devolvió
EXEC usp_RegistrarPrestamo 'S2157', '9788457144799';             -- 8. OK (60004)
EXEC usp_RegistrarPrestamo 'S0003', '9780000000000';             -- 9. ERROR: el libro no existe
EXEC usp_RegistrarPrestamo 'S0249', '9788457144799';             -- 10. ERROR: el socio no existe

EXEC usp_PrestamosSocio 'S0003';                                 -- sus 3 préstamos nuevos como Pendiente
GO

-- P10. ¿Desde cuándo debería haber devuelto Pablo Gil Velasco (S2157) el préstamo 40065?
--      Desde su FechaDevolucionPrevista; se puede consultar con:
SELECT IdPrestamo, Titulo, FechaPrestamo, FechaDevolucionPrevista
FROM Prestamos WHERE IdPrestamo = 40065;
GO

-- P11. ¿Qué ganamos programando las normas en un procedimiento almacenado?
--      Las normas se escriben una sola vez, en la base de datos, y se cumplen
--      siempre. Si mañana se hace una aplicación móvil, llamará al mismo
--      procedimiento y no podrá saltarse las normas ni programarlas distinto.
--      Si cambia una norma (p. ej. 4 libros en vez de 3) solo se cambia aquí.

-- P12. ¿Por qué dice que S0249 no existe?
--      Porque solo tenemos la tabla Prestamos: un socio "existe" únicamente si
--      tiene algún préstamo. S0249 nunca ha sacado un libro, así que no aparece.
--      Es un problema de diseño: faltan tablas Socios y Libros propias; además
--      el nombre del socio y los datos del libro se repiten en cada préstamo
--      (redundancia, falta de normalización).

-- P13. ¿Añadirías un índice para la búsqueda por ISBN?
--      Sí. Con SET STATISTICS IO ON se ve que buscar por ISBN recorre toda la
--      tabla (Clustered Index Scan). Como se busca un libro en cada préstamo y
--      ISBN es bastante selectivo (650 valores), conviene un índice no clúster:
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Prestamos_ISBN')
    CREATE NONCLUSTERED INDEX IX_Prestamos_ISBN ON Prestamos (ISBN);
GO
