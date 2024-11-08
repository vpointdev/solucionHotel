CREATE OR ALTER PROCEDURE PA_Reservacion_ActualizarCheckOut
AS
BEGIN
    DECLARE @EstadoCheckOut INT
    SELECT @EstadoCheckOut = EstadoReservacionId
    FROM EstadoReservacion
    WHERE Nombre = 'CheckOut'

    UPDATE r
    SET EstadoReservacionId = @EstadoCheckOut,
        FechaModificacion = GETDATE()
    FROM Reservacion r
    WHERE r.FechaSalida < GETDATE()
    AND r.EstadoReservacionId IN (
        SELECT EstadoReservacionId 
        FROM EstadoReservacion 
        WHERE Nombre = 'Confirmada'
    )

    SELECT 
        COUNT(*) AS ReservacionesActualizadas,
        'Reservaciones actualizadas a CheckOut' AS Mensaje
    FROM Reservacion
    WHERE EstadoReservacionId = @EstadoCheckOut
    AND FechaModificacion = GETDATE()
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

    DECLARE @EstadoCheckOut INT
    SELECT @EstadoCheckOut = EstadoReservacionId
    FROM EstadoReservacion
    WHERE Nombre = 'CheckOut'

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
    AND r.EstadoReservacionId = @EstadoCheckOut
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
    AND r.EstadoReservacionId IN (
        SELECT EstadoReservacionId 
        FROM EstadoReservacion 
        WHERE Nombre IN ('Confirmada', 'CheckOut')
    )
    WHERE h.Activo = 1
    GROUP BY th.Nombre
    ORDER BY th.Nombre
END
GO