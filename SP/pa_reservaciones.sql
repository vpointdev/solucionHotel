CREATE OR ALTER FUNCTION FN_GenerarCodigoReservacion()
RETURNS VARCHAR(20)
AS
BEGIN
    RETURN FORMAT(GETDATE(), 'yyyyMMdd-') + 
           RIGHT('000' + CAST((SELECT COUNT(*) + 1 FROM Reservacion) AS VARCHAR(3)), 3)
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_Crear
(
    @UsuarioId INT,
    @HabitacionId INT,
    @FechaEntrada DATETIME,
    @FechaSalida DATETIME,
    @UsuarioCreacionId INT
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
            AND r.EstadoReservacionId IN (
                SELECT EstadoReservacionId 
                FROM EstadoReservacion 
                WHERE Nombre IN ('Confirmada')
            )
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

        DECLARE @EstadoConfirmada INT
        SELECT @EstadoConfirmada = EstadoReservacionId
        FROM EstadoReservacion
        WHERE Nombre = 'Confirmada';

        INSERT INTO Reservacion (
            CodigoReservacion,
            UsuarioId,
            HabitacionId,
            FechaEntrada,
            FechaSalida,
            EstadoReservacionId,
            PrecioTotal,
            UsuarioCreacionId
        )
        VALUES (
            dbo.FN_GenerarCodigoReservacion(),
            @UsuarioId,
            @HabitacionId,
            @FechaEntrada,
            @FechaSalida,
            @EstadoConfirmada,
            @PrecioTotal,
            @UsuarioCreacionId
        );

        DECLARE @CodigoReservacion VARCHAR(20) = SCOPE_IDENTITY();
        
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

    DECLARE @EstadoCancelada INT
    SELECT @EstadoCancelada = EstadoReservacionId
    FROM EstadoReservacion
    WHERE Nombre = 'Cancelada'

    UPDATE Reservacion
    SET EstadoReservacionId = @EstadoCancelada,
        FechaModificacion = GETDATE(),
        UsuarioModificacionId = @UsuarioId
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
            er.Nombre AS Estado
    FROM Reservacion r
    INNER JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
    INNER JOIN EstadoReservacion er ON r.EstadoReservacionId = er.EstadoReservacionId
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
            er.Nombre AS Estado,
            u.Nombre AS NombreUsuario
    FROM Reservacion r
    INNER JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
    INNER JOIN EstadoReservacion er ON r.EstadoReservacionId = er.EstadoReservacionId
    INNER JOIN Usuario u ON r.UsuarioId = u.UsuarioId
    WHERE r.CodigoReservacion = @CodigoReservacion
END
GO