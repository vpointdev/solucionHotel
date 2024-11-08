CREATE OR ALTER PROCEDURE PA_Habitacion_ObtenerDisponibles
(
    @FechaEntrada DATETIME,
    @FechaSalida DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @FechaEntrada >= @FechaSalida
            THROW 50000, 'La fecha de entrada debe ser anterior a la fecha de salida.', 1;

        SELECT  h.HabitacionId,
                h.NumeroHabitacion,
                h.Piso,
                th.TipoHabitacionId,
                th.Nombre AS TipoHabitacion,
                th.PrecioBase,
                th.Capacidad,
                DATEDIFF(DAY, @FechaEntrada, @FechaSalida) * th.PrecioBase AS PrecioEstadia
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        WHERE h.Estado = 'Disponible'
        AND h.Activo = 1
        AND NOT EXISTS (
            SELECT 1
            FROM Reservacion r
            WHERE r.HabitacionId = h.HabitacionId
            AND r.EstadoReservacionId IN (
                SELECT EstadoReservacionId 
                FROM EstadoReservacion 
                WHERE Nombre IN ('Confirmada')
            )
            AND (
                @FechaEntrada BETWEEN r.FechaEntrada AND r.FechaSalida
                OR @FechaSalida BETWEEN r.FechaEntrada AND r.FechaSalida
                OR (r.FechaEntrada BETWEEN @FechaEntrada AND @FechaSalida)
            )
        )
        ORDER BY th.PrecioBase, h.NumeroHabitacion
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ObtenerPorId
(
    @HabitacionId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT  h.HabitacionId,
                h.NumeroHabitacion,
                h.Piso,
                h.Estado,
                th.TipoHabitacionId,
                th.Nombre AS TipoHabitacion,
                th.PrecioBase,
                th.Capacidad,
                CASE 
                    WHEN r.ReservacionId IS NOT NULL THEN 'Reservada'
                    ELSE 'Disponible'
                END AS EstadoActual
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        LEFT JOIN Reservacion r ON h.HabitacionId = r.HabitacionId
            AND r.EstadoReservacionId IN (
                SELECT EstadoReservacionId 
                FROM EstadoReservacion 
                WHERE Nombre = 'Confirmada'
            )
            AND GETDATE() BETWEEN r.FechaEntrada AND r.FechaSalida
        WHERE h.HabitacionId = @HabitacionId
        AND h.Activo = 1
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ListarTodas
(
    @TipoHabitacionId INT = NULL,
    @Estado VARCHAR(20) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT  h.HabitacionId,
                h.NumeroHabitacion,
                h.Piso,
                h.Estado,
                th.TipoHabitacionId,
                th.Nombre AS TipoHabitacion,
                th.PrecioBase,
                th.Capacidad,
                CASE 
                    WHEN r.ReservacionId IS NOT NULL THEN 'Reservada'
                    ELSE 'Disponible'
                END AS EstadoActual
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        LEFT JOIN Reservacion r ON h.HabitacionId = r.HabitacionId
            AND r.EstadoReservacionId IN (
                SELECT EstadoReservacionId 
                FROM EstadoReservacion 
                WHERE Nombre = 'Confirmada'
            )
            AND GETDATE() BETWEEN r.FechaEntrada AND r.FechaSalida
        WHERE h.Activo = 1
        AND (@TipoHabitacionId IS NULL OR h.TipoHabitacionId = @TipoHabitacionId)
        AND (@Estado IS NULL OR h.Estado = @Estado)
        ORDER BY h.Piso, h.NumeroHabitacion
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ActualizarEstado
(
    @HabitacionId INT,
    @Estado VARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Habitacion WHERE HabitacionId = @HabitacionId AND Activo = 1)
            THROW 50000, 'La habitación no existe o está inactiva.', 1;

        IF @Estado NOT IN ('Disponible', 'Mantenimiento')
            THROW 50001, 'Estado de habitación no válido.', 1;

        IF EXISTS (
            SELECT 1
            FROM Reservacion r
            WHERE r.HabitacionId = @HabitacionId
            AND r.EstadoReservacionId IN (
                SELECT EstadoReservacionId 
                FROM EstadoReservacion 
                WHERE Nombre = 'Confirmada'
            )
            AND GETDATE() BETWEEN r.FechaEntrada AND r.FechaSalida
        )
            THROW 50002, 'No se puede cambiar el estado de una habitación con reservación activa.', 1;

        UPDATE Habitacion
        SET Estado = @Estado
        WHERE HabitacionId = @HabitacionId
        AND Activo = 1

        SELECT  h.HabitacionId,
                h.NumeroHabitacion,
                h.Estado,
                th.Nombre AS TipoHabitacion
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        WHERE h.HabitacionId = @HabitacionId
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ObtenerPorTipo
(
    @TipoHabitacionId INT,
    @FechaConsulta DATETIME = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SET @FechaConsulta = ISNULL(@FechaConsulta, GETDATE())

        SELECT  h.HabitacionId,
                h.NumeroHabitacion,
                h.Piso,
                h.Estado,
                th.PrecioBase,
                CASE 
                    WHEN r.ReservacionId IS NOT NULL THEN 'Reservada'
                    ELSE 'Disponible'
                END AS EstadoActual
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        LEFT JOIN Reservacion r ON h.HabitacionId = r.HabitacionId
            AND r.EstadoReservacionId IN (
                SELECT EstadoReservacionId 
                FROM EstadoReservacion 
                WHERE Nombre = 'Confirmada'
            )
            AND @FechaConsulta BETWEEN r.FechaEntrada AND r.FechaSalida
        WHERE h.TipoHabitacionId = @TipoHabitacionId
        AND h.Activo = 1
        ORDER BY h.NumeroHabitacion
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO