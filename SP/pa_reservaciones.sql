USE Hotel;
GO

-- 1. Primero la función de generar código
CREATE OR ALTER FUNCTION FN_GenerarCodigoReservacion()
RETURNS VARCHAR(20)
AS
BEGIN
    RETURN FORMAT(GETDATE(), 'yyyyMMdd-') + 
           RIGHT('000' + CAST((SELECT COUNT(*) + 1 FROM Reservacion) AS VARCHAR(3)), 3)
END
GO

-- 2. El SP para obtener por código (ya que es dependencia)
CREATE OR ALTER PROCEDURE PA_Reservacion_ObtenerPorCodigo
(
    @CodigoReservacion VARCHAR(20)
)
AS
BEGIN
    SELECT  r.ReservacionId,
            r.CodigoReservacion,
            r.FechaEntrada,
            r.FechaSalida,
            r.PrecioTotal,
            h.NumeroHabitacion,
            th.Nombre AS TipoHabitacion,
            r.Estado,
            u.Nombre AS NombreUsuario
    FROM Reservacion r
    INNER JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
    INNER JOIN Usuario u ON r.UsuarioId = u.UsuarioId
    WHERE r.CodigoReservacion = @CodigoReservacion
END
GO

-- 3. Ahora sí el SP de crear
CREATE OR ALTER PROCEDURE PA_Reservacion_Crear
(
    @UsuarioId INT,
    @HabitacionId INT,
    @FechaEntrada DATETIME,
    @FechaSalida DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @FechaEntrada >= @FechaSalida
            THROW 50000, 'La fecha de entrada debe ser anterior a la fecha de salida.', 1;

        IF EXISTS (
            SELECT 1
            FROM Reservacion r
            WHERE r.HabitacionId = @HabitacionId
            AND r.Estado = 'Confirmada'
            AND (
                @FechaEntrada BETWEEN r.FechaEntrada AND r.FechaSalida
                OR @FechaSalida BETWEEN r.FechaEntrada AND r.FechaSalida
            )
        )
            THROW 50001, 'La habitación no está disponible para las fechas seleccionadas.', 1;

        DECLARE @PrecioTotal DECIMAL(10,2)
        SELECT @PrecioTotal = th.PrecioBase * DATEDIFF(DAY, @FechaEntrada, @FechaSalida)
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        WHERE h.HabitacionId = @HabitacionId;

        DECLARE @CodigoReservacion VARCHAR(20) = dbo.FN_GenerarCodigoReservacion();

        INSERT INTO Reservacion (
            CodigoReservacion,
            UsuarioId,
            HabitacionId,
            FechaEntrada,
            FechaSalida,
            Estado,
            PrecioTotal
        )
        VALUES (
            @CodigoReservacion,
            @UsuarioId,
            @HabitacionId,
            @FechaEntrada,
            @FechaSalida,
            'Confirmada',
            @PrecioTotal
        );
        
        EXEC PA_Reservacion_ObtenerPorCodigo @CodigoReservacion;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_Cancelar
(
    @ReservacionId INT,
    @UsuarioId INT
)
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM Reservacion 
        WHERE ReservacionId = @ReservacionId 
        AND UsuarioId = @UsuarioId
    )
    BEGIN
        RAISERROR('Reservación no encontrada o no pertenece al usuario.', 16, 1)
        RETURN
    END

    DECLARE @FechaEntrada DATETIME
    DECLARE @PrecioTotal DECIMAL(10,2)
    DECLARE @CargoCancelacion DECIMAL(10,2) = 0

    SELECT @FechaEntrada = FechaEntrada,
           @PrecioTotal = PrecioTotal
    FROM Reservacion
    WHERE ReservacionId = @ReservacionId

    DECLARE @HorasParaEntrada INT
    SET @HorasParaEntrada = DATEDIFF(HOUR, GETDATE(), @FechaEntrada)

    IF @HorasParaEntrada <= 24
    BEGIN
        SET @CargoCancelacion = @PrecioTotal * 0.25 -- 25% fee
    END

    UPDATE Reservacion
    SET Estado = 'Cancelada'
    WHERE ReservacionId = @ReservacionId

    SELECT  ReservacionId,
            CodigoReservacion,
            'Cancelada' AS Estado,
            @CargoCancelacion AS CargoCancelacion,
            CASE 
                WHEN @HorasParaEntrada <= 24 THEN 'Aplica cargo del 25%'
                ELSE 'Sin cargo'
            END AS Observaciones
    FROM Reservacion
    WHERE ReservacionId = @ReservacionId
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_ObtenerPorUsuario
(
    @UsuarioId INT
)
AS
BEGIN
    SELECT  r.ReservacionId,
            r.CodigoReservacion,
            r.FechaEntrada,
            r.FechaSalida,
            r.PrecioTotal,
            h.NumeroHabitacion,
            th.Nombre AS TipoHabitacion,
            r.Estado
    FROM Reservacion r
    INNER JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
    WHERE r.UsuarioId = @UsuarioId
    ORDER BY r.FechaEntrada DESC
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_ObtenerPorCodigo
(
    @CodigoReservacion VARCHAR(20)
)
AS
BEGIN
    SELECT  r.ReservacionId,
            r.CodigoReservacion,
            r.FechaEntrada,
            r.FechaSalida,
            r.PrecioTotal,
            h.NumeroHabitacion,
            th.Nombre AS TipoHabitacion,
            r.Estado,
            u.Nombre AS NombreUsuario
    FROM Reservacion r
    INNER JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
    INNER JOIN Usuario u ON r.UsuarioId = u.UsuarioId
    WHERE r.CodigoReservacion = @CodigoReservacion
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_ActualizarCheckOut
AS
BEGIN
    UPDATE r
    SET r.Estado = 'CheckOut'
    FROM Reservacion r
    WHERE r.FechaSalida < GETDATE()
    AND r.Estado = 'Confirmada'

    SELECT 
        COUNT(*) AS ReservacionesActualizadas,
        'Reservaciones actualizadas a CheckOut' AS Mensaje
    FROM Reservacion
    WHERE Estado = 'CheckOut'
    AND FechaSalida < GETDATE()
END
GO

CREATE OR ALTER PROCEDURE PA_Reporte_ActualizarYObtenerIngresos
(
    @FechaInicio DATETIME,
    @FechaFin DATETIME
)
AS
BEGIN
    EXEC PA_Reservacion_ActualizarCheckOut
    SELECT  
        th.Nombre AS TipoHabitacion,
        COUNT(r.ReservacionId) AS CantidadReservas,
        SUM(r.PrecioTotal) AS TotalIngresos,
        MIN(r.PrecioTotal) AS IngresoMinimo,
        MAX(r.PrecioTotal) AS IngresoMaximo,
        AVG(r.PrecioTotal) AS PromedioIngreso
    FROM TipoHabitacion th
    LEFT JOIN Habitacion h ON th.TipoHabitacionId = h.TipoHabitacionId
    LEFT JOIN Reservacion r ON h.HabitacionId = r.HabitacionId
    AND r.FechaEntrada BETWEEN @FechaInicio AND @FechaFin
    AND r.Estado = 'CheckOut'
    GROUP BY th.Nombre
    ORDER BY th.Nombre
END
GO

CREATE OR ALTER PROCEDURE PA_Reporte_OcupacionHabitaciones
(
    @FechaInicio DATETIME,
    @FechaFin DATETIME
)
AS
BEGIN
    EXEC PA_Reservacion_ActualizarCheckOut
    SELECT  
        th.Nombre AS TipoHabitacion,
        COUNT(DISTINCT h.HabitacionId) AS TotalHabitaciones,
        COUNT(DISTINCT r.ReservacionId) AS TotalReservas,
        CAST(COUNT(DISTINCT r.ReservacionId) AS FLOAT) / 
        CAST(COUNT(DISTINCT h.HabitacionId) * DATEDIFF(DAY, @FechaInicio, @FechaFin) AS FLOAT) * 100 AS PorcentajeOcupacion
    FROM TipoHabitacion th
    LEFT JOIN Habitacion h ON th.TipoHabitacionId = h.TipoHabitacionId
    LEFT JOIN Reservacion r ON h.HabitacionId = r.HabitacionId
    AND r.FechaEntrada BETWEEN @FechaInicio AND @FechaFin
    AND r.Estado IN ('Confirmada', 'CheckOut')
    WHERE h.Activo = 1
    GROUP BY th.Nombre
    ORDER BY th.Nombre
END
GO