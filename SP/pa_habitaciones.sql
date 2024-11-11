USE Hotel;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_Crear
(
    @NumeroHabitacion VARCHAR(10),
    @TipoHabitacionId INT,
    @Piso INT,
    @Estado VARCHAR(20) = 'Disponible',
    @Observaciones VARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Habitacion WHERE NumeroHabitacion = @NumeroHabitacion AND Activo = 1)
            THROW 50000, 'Ya existe una habitación con este número.', 1;

        IF NOT EXISTS (SELECT 1 FROM TipoHabitacion WHERE TipoHabitacionId = @TipoHabitacionId AND Activo = 1)
            THROW 50001, 'El tipo de habitación no existe.', 1;

        IF @Estado NOT IN ('Disponible', 'Mantenimiento')
            THROW 50002, 'Estado de habitación no válido.', 1;

        INSERT INTO Habitacion (
            NumeroHabitacion,
            TipoHabitacionId,
            Piso,
            Estado,
            Observaciones
        )
        VALUES (
            @NumeroHabitacion,
            @TipoHabitacionId,
            @Piso,
            @Estado,
            @Observaciones
        );

        SELECT 
            h.HabitacionId,
            h.NumeroHabitacion,
            h.TipoHabitacionId,
            th.Nombre AS TipoHabitacion,
            h.Piso,
            h.Estado,
            h.Observaciones
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        WHERE h.HabitacionId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_Obtener
(
    @HabitacionId INT = NULL,
    @NumeroHabitacion VARCHAR(10) = NULL,
    @TipoHabitacionId INT = NULL,
    @Estado VARCHAR(20) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT 
            h.HabitacionId,
            h.NumeroHabitacion,
            h.TipoHabitacionId,
            th.Nombre AS TipoHabitacion,
            th.PrecioBase,
            th.Capacidad,
            h.Piso,
            h.Estado,
            CASE 
                WHEN EXISTS (
                    SELECT 1 
                    FROM Reservacion r 
                    WHERE r.HabitacionId = h.HabitacionId 
                    AND r.Estado = 'Confirmada'
                    AND GETDATE() BETWEEN r.FechaEntrada AND r.FechaSalida
                ) THEN 'Ocupada'
                ELSE h.Estado
            END AS EstadoActual,
            h.Observaciones
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        WHERE h.Activo = 1
        AND (@HabitacionId IS NULL OR h.HabitacionId = @HabitacionId)
        AND (@NumeroHabitacion IS NULL OR h.NumeroHabitacion = @NumeroHabitacion)
        AND (@TipoHabitacionId IS NULL OR h.TipoHabitacionId = @TipoHabitacionId)
        AND (@Estado IS NULL OR h.Estado = @Estado)
        ORDER BY h.Piso, h.NumeroHabitacion;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_Actualizar
(
    @HabitacionId INT,
    @TipoHabitacionId INT,
    @Piso INT,
    @Estado VARCHAR(20),
    @Observaciones VARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Habitacion WHERE HabitacionId = @HabitacionId AND Activo = 1)
            THROW 50000, 'La habitación no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM TipoHabitacion WHERE TipoHabitacionId = @TipoHabitacionId AND Activo = 1)
            THROW 50001, 'El tipo de habitación no existe.', 1;

        IF @Estado NOT IN ('Disponible', 'Mantenimiento')
            THROW 50002, 'Estado de habitación no válido.', 1;

        IF EXISTS (
            SELECT 1 
            FROM Reservacion 
            WHERE HabitacionId = @HabitacionId 
            AND Estado = 'Confirmada'
            AND GETDATE() BETWEEN FechaEntrada AND FechaSalida
        )
            THROW 50003, 'No se puede modificar una habitación ocupada.', 1;

        UPDATE Habitacion
        SET TipoHabitacionId = @TipoHabitacionId,
            Piso = @Piso,
            Estado = @Estado,
            Observaciones = @Observaciones
        WHERE HabitacionId = @HabitacionId;

        EXEC PA_Habitacion_Obtener @HabitacionId = @HabitacionId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_Eliminar
(
    @HabitacionId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Habitacion WHERE HabitacionId = @HabitacionId AND Activo = 1)
            THROW 50000, 'La habitación no existe.', 1;

        IF EXISTS (
            SELECT 1 
            FROM Reservacion 
            WHERE HabitacionId = @HabitacionId 
            AND Estado = 'Confirmada'
        )
            THROW 50001, 'No se puede eliminar una habitación con reservaciones.', 1;

        UPDATE Habitacion
        SET Activo = 0
        WHERE HabitacionId = @HabitacionId;

        SELECT 'Habitación eliminada correctamente.' AS Mensaje;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

USE Hotel;
GO

CREATE OR ALTER PROCEDURE PA_TipoHabitacion_Crear
(
    @Nombre VARCHAR(50),
    @Descripcion VARCHAR(200) = NULL,
    @PrecioBase DECIMAL(10,2),
    @Capacidad INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM TipoHabitacion WHERE Nombre = @Nombre AND Activo = 1)
            THROW 50000, 'Ya existe un tipo de habitación con este nombre.', 1;

        INSERT INTO TipoHabitacion (
            Nombre,
            Descripcion,
            PrecioBase,
            Capacidad
        )
        VALUES (
            @Nombre,
            @Descripcion,
            @PrecioBase,
            @Capacidad
        );

        SELECT TipoHabitacionId, Nombre, Descripcion, PrecioBase, Capacidad
        FROM TipoHabitacion
        WHERE TipoHabitacionId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_TipoHabitacion_Obtener
(
    @TipoHabitacionId INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT 
            th.TipoHabitacionId,
            th.Nombre,
            th.Descripcion,
            th.PrecioBase,
            th.Capacidad
        FROM TipoHabitacion th
        WHERE th.Activo = 1
        AND (@TipoHabitacionId IS NULL OR th.TipoHabitacionId = @TipoHabitacionId)
        ORDER BY th.PrecioBase;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_TipoHabitacion_Actualizar
(
    @TipoHabitacionId INT,
    @Nombre VARCHAR(50),
    @Descripcion VARCHAR(200) = NULL,
    @PrecioBase DECIMAL(10,2),
    @Capacidad INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM TipoHabitacion WHERE TipoHabitacionId = @TipoHabitacionId AND Activo = 1)
            THROW 50000, 'El tipo de habitación no existe.', 1;

        IF EXISTS (SELECT 1 FROM TipoHabitacion WHERE Nombre = @Nombre AND TipoHabitacionId != @TipoHabitacionId AND Activo = 1)
            THROW 50001, 'Ya existe otro tipo de habitación con este nombre.', 1;

        UPDATE TipoHabitacion
        SET Nombre = @Nombre,
            Descripcion = @Descripcion,
            PrecioBase = @PrecioBase,
            Capacidad = @Capacidad
        WHERE TipoHabitacionId = @TipoHabitacionId;

        SELECT TipoHabitacionId, Nombre, Descripcion, PrecioBase, Capacidad
        FROM TipoHabitacion
        WHERE TipoHabitacionId = @TipoHabitacionId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_TipoHabitacion_Eliminar
(
    @TipoHabitacionId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM TipoHabitacion WHERE TipoHabitacionId = @TipoHabitacionId AND Activo = 1)
            THROW 50000, 'El tipo de habitación no existe.', 1;

        IF EXISTS (SELECT 1 FROM Habitacion WHERE TipoHabitacionId = @TipoHabitacionId AND Activo = 1)
            THROW 50001, 'No se puede eliminar el tipo de habitación porque existen habitaciones asociadas.', 1;

        UPDATE TipoHabitacion
        SET Activo = 0
        WHERE TipoHabitacionId = @TipoHabitacionId;

        SELECT 'Tipo de habitación eliminado correctamente.' AS Mensaje;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO