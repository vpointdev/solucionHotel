USE Hotel
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_Crear
    @NumeroHabitacion VARCHAR(10),
    @TipoHabitacionId INT,
    @Piso INT,
    @Estado VARCHAR(20) = 'Disponible',
    @Observaciones VARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Habitacion WHERE NumeroHabitacion = @NumeroHabitacion AND Activo = 1)
    BEGIN
        RETURN 0;
    END

    INSERT INTO Habitacion (
        NumeroHabitacion,
        TipoHabitacionId,
        Piso,
        Estado,
        Observaciones,
        Activo
    )
    VALUES (
        @NumeroHabitacion,
        @TipoHabitacionId,
        @Piso,
        @Estado,
        @Observaciones,
        1
    );

    RETURN 1;
END;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_Actualizar
    @HabitacionId INT,
    @NumeroHabitacion VARCHAR(10),
    @TipoHabitacionId INT,
    @Piso INT,
    @Estado VARCHAR(20),
    @Observaciones VARCHAR(500),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Habitacion
    SET NumeroHabitacion = @NumeroHabitacion,
        TipoHabitacionId = @TipoHabitacionId,
        Piso = @Piso,
        Estado = @Estado,
        Observaciones = @Observaciones,
        Activo = @Activo
    WHERE HabitacionId = @HabitacionId;

    RETURN 1;
END;
GO
USE Hotel
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_Eliminar
    @HabitacionId INT
AS
BEGIN
    SET NOCOUNT ON;
        UPDATE Habitacion
        SET Activo = 0
        WHERE HabitacionId = @HabitacionId;
        
        RETURN 0;
END
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ObtenerPorId
    @HabitacionId INT = NULL,
    @NumeroHabitacion VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        h.HabitacionId,
        h.NumeroHabitacion,
        h.TipoHabitacionId,
        h.Piso,
        h.Estado,
        h.Observaciones,
        h.Activo,
        t.Nombre as TipoHabitacionNombre,
        t.PrecioBase,
        t.Capacidad
    FROM Habitacion h
    INNER JOIN TipoHabitacion t ON h.TipoHabitacionId = t.TipoHabitacionId
    WHERE (@HabitacionId IS NULL OR h.HabitacionId = @HabitacionId)
    AND (@NumeroHabitacion IS NULL OR h.NumeroHabitacion = @NumeroHabitacion)
    AND h.Activo = 1
    ORDER BY h.NumeroHabitacion;
END;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ObtenerDisponibles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        h.HabitacionId,
        h.NumeroHabitacion,
        h.TipoHabitacionId,
        h.Piso,
        h.Estado,
        h.Observaciones,
        h.Activo,
        t.Nombre as TipoHabitacionNombre,
        t.PrecioBase,
        t.Capacidad
    FROM Habitacion h
    INNER JOIN TipoHabitacion t ON h.TipoHabitacionId = t.TipoHabitacionId
    WHERE h.Activo = 1
    AND h.Estado = 'Disponible'
    ORDER BY h.NumeroHabitacion;
END;
GO

CREATE OR ALTER PROCEDURE PA_TipoHabitacion_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        TipoHabitacionId,
        Nombre,
        Descripcion,
        PrecioBase,
        Capacidad,
        Activo
    FROM TipoHabitacion
    WHERE Activo = 1
    ORDER BY PrecioBase;
END;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ObtenerTodos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        h.HabitacionId,
        h.NumeroHabitacion,
        h.TipoHabitacionId,
        h.Piso,
        h.Estado,
        h.Observaciones,
        h.Activo,
        -- TipoHabitacion properties
        t.Nombre as TipoHabitacionNombre,
        t.PrecioBase,
        t.Capacidad
    FROM Habitacion h
    INNER JOIN TipoHabitacion t ON h.TipoHabitacionId = t.TipoHabitacionId
    WHERE h.Activo = 1
    ORDER BY h.NumeroHabitacion;
END;
GO

