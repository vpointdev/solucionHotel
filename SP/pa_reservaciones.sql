USE Hotel;
GO

CREATE OR ALTER FUNCTION FN_GenerarCodigoReservacion()
RETURNS VARCHAR(20)
AS
BEGIN
    RETURN 'RES-' + FORMAT(GETDATE(), 'yyyyMMdd') + 
           RIGHT('000' + CAST((SELECT COUNT(*) + 1 FROM Reservacion) AS VARCHAR(3)), 3)
END
GO

CREATE OR ALTER FUNCTION FN_CalcularCargoCancelacion(
    @ReservacionId INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @FechaEntrada DATETIME
    DECLARE @PrecioTotal DECIMAL(10,2)
    DECLARE @CargoCancelacion DECIMAL(10,2) = 0

    SELECT 
        @FechaEntrada = FechaEntrada,
        @PrecioTotal = PrecioTotal
    FROM Reservacion
    WHERE ReservacionId = @ReservacionId

    IF DATEDIFF(HOUR, GETDATE(), @FechaEntrada) <= 24
        SET @CargoCancelacion = @PrecioTotal * 0.25
    
    RETURN @CargoCancelacion
END
GO

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
            AND r.Estado IN ('Confirmada', 'Pendiente')
            AND (
                @FechaEntrada BETWEEN r.FechaEntrada AND r.FechaSalida
                OR @FechaSalida BETWEEN r.FechaEntrada AND r.FechaSalida
            )
        )
            THROW 50001, 'La habitación no está disponible para las fechas seleccionadas.', 1;

        DECLARE @PrecioTotal DECIMAL(10,2) = 0;
        
        SELECT @PrecioTotal = ISNULL(th.PrecioBase, 0) * DATEDIFF(DAY, @FechaEntrada, @FechaSalida)
        FROM Habitacion h
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        WHERE h.HabitacionId = @HabitacionId;

        DECLARE @CodigoReservacion VARCHAR(20) = dbo.FN_GenerarCodigoReservacion();

        -- Create reservation
        INSERT INTO Reservacion (
            CodigoReservacion, 
            UsuarioId, 
            HabitacionId, 
            FechaEntrada, 
            FechaSalida, 
            Estado, 
            PrecioTotal,
            PagoProcesado
        )
        VALUES (
            @CodigoReservacion,
            @UsuarioId,
            @HabitacionId,
            @FechaEntrada,
            @FechaSalida,
            'Pendiente',
            @PrecioTotal,
            0
        );

        SELECT 
            r.ReservacionId,
            r.CodigoReservacion,
            r.FechaEntrada,
            r.FechaSalida,
            r.PrecioTotal,
            r.Estado,
            h.NumeroHabitacion,
            th.Nombre AS TipoHabitacion
        FROM Reservacion r
        INNER JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
        INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        WHERE r.CodigoReservacion = @CodigoReservacion;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_ProcesarPago
(
    @ReservacionId INT,
    @UsuarioId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Validate that the reservation exists and is in a pending state
        IF NOT EXISTS (
            SELECT 1 
            FROM Reservacion 
            WHERE ReservacionId = @ReservacionId 
            AND UsuarioId = @UsuarioId
            AND Estado = 'Pendiente'
            AND PagoProcesado = 0
        )
            THROW 50002, 'Reservación no encontrada o no está pendiente de pago.', 1;

        -- Get the total amount for the reservation
        DECLARE @MontoTotal DECIMAL(10,2);
        SELECT @MontoTotal = PrecioTotal
        FROM Reservacion
        WHERE ReservacionId = @ReservacionId;

        -- Insert payment record
        INSERT INTO Pago (
            ReservacionId,
            Monto,
            FechaPago,
            NumeroTransaccion,
            Estado,
            EsCargoCancelacion,
            Observaciones,
            UsuarioCreacionId
        )
        VALUES (
            @ReservacionId,
            @MontoTotal,
            GETDATE(),
            'PAY-' + CAST(@ReservacionId AS VARCHAR),
            'Completado',
            0,
            'Pago inicial de reservación',
            @UsuarioId
        );

        -- Update reservation status to confirmed
        UPDATE Reservacion
        SET 
            Estado = 'Confirmada',
            PagoProcesado = 1
        WHERE ReservacionId = @ReservacionId;

        -- Retrieve all reservation and related data
        SELECT 
            r.ReservacionId,
            r.CodigoReservacion,
            r.UsuarioId,
            r.HabitacionId,
            r.FechaEntrada,
            r.FechaSalida,
            r.Estado,
            r.PrecioTotal,
            r.PagoProcesado,
            COALESCE(c.CargoCancelacion, 0.00) AS CargoCancelacion,
            COALESCE(r.MontoReembolsado, 0.00) AS MontoReembolsado,
            COALESCE(r.TotalPagado, 0.00) AS TotalPagado,
            COALESCE(h.NumeroHabitacion, '') AS NumeroHabitacion,
            COALESCE(th.Nombre, '') AS TipoHabitacion,
            COALESCE(r.Observaciones, '') AS Observaciones,
            p.NumeroTransaccion,
            p.Monto AS MontoPagado,
            p.FechaPago,
            'Pago procesado exitosamente' AS Mensaje
        FROM Reservacion r
        LEFT JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
        LEFT JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
        LEFT JOIN Pago p ON r.ReservacionId = p.ReservacionId AND p.EsCargoCancelacion = 0
        WHERE r.ReservacionId = @ReservacionId;
    END TRY
    BEGIN CATCH
        -- Re-throw the error for the caller to handle
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
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 FROM Reservacion 
            WHERE ReservacionId = @ReservacionId 
            AND UsuarioId = @UsuarioId
            AND Estado = 'Confirmada'
            AND PagoProcesado = 1
        )
            THROW 50002, 'Reservación no encontrada o no está en estado válido para cancelar.', 1;

        DECLARE @CargoCancelacion DECIMAL(10,2) = dbo.FN_CalcularCargoCancelacion(@ReservacionId);
        DECLARE @MontoReembolso DECIMAL(10,2);
        
        SELECT @MontoReembolso = PrecioTotal - @CargoCancelacion
        FROM Reservacion
        WHERE ReservacionId = @ReservacionId;

        BEGIN TRANSACTION;
            -- Process refund
            INSERT INTO Pago (
                ReservacionId,
                Monto,
                FechaPago,
                NumeroTransaccion,
                Estado,
                EsCargoCancelacion,
                Observaciones,
                UsuarioCreacionId
            )
            VALUES (
                @ReservacionId,
                -@MontoReembolso,
                GETDATE(),
                'REF-' + CAST(@ReservacionId AS VARCHAR(10)),
                'Completado',
                0,
                'Reembolso por cancelación',
                @UsuarioId
            );

            IF @CargoCancelacion > 0
            BEGIN
                INSERT INTO Pago (
                    ReservacionId,
                    Monto,
                    FechaPago,
                    NumeroTransaccion,
                    Estado,
                    EsCargoCancelacion,
                    Observaciones,
                    UsuarioCreacionId
                )
                VALUES (
                    @ReservacionId,
                    @CargoCancelacion,
                    GETDATE(),
                    'CAN-' + CAST(@ReservacionId AS VARCHAR(10)),
                    'Completado',
                    1,
                    'Cargo por cancelación dentro de 24 horas',
                    @UsuarioId
                );
            END

            UPDATE Reservacion
            SET Estado = 'Cancelada'
            WHERE ReservacionId = @ReservacionId;

        COMMIT;

        SELECT 
            r.ReservacionId,
            r.CodigoReservacion,
            r.Estado,
            @CargoCancelacion as CargoCancelacion,
            @MontoReembolso as MontoReembolsado,
            CASE 
                WHEN @CargoCancelacion > 0 THEN 'Cancelación con cargo del 25%'
                ELSE 'Cancelación sin cargo'
            END as Observaciones
        FROM Reservacion r
        WHERE r.ReservacionId = @ReservacionId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_ObtenerPorUsuario
(
    @UsuarioId INT
)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        r.ReservacionId,
        r.CodigoReservacion,
        r.FechaEntrada,
        r.FechaSalida,
        r.PrecioTotal,
        h.NumeroHabitacion,
        th.Nombre AS TipoHabitacion,
        r.Estado,
        r.PagoProcesado,
        ISNULL((
            SELECT SUM(Monto)
            FROM Pago p
            WHERE p.ReservacionId = r.ReservacionId
            AND p.Estado = 'Completado'
            AND p.EsCargoCancelacion = 0
        ), 0) AS TotalPagado
    FROM Reservacion r
    INNER JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
    WHERE r.UsuarioId = @UsuarioId
    ORDER BY r.FechaEntrada DESC;
END;
GO

CREATE OR ALTER PROCEDURE PA_Habitacion_ObtenerDisponibles
(
    @FechaEntrada DATETIME,
    @FechaSalida DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        h.HabitacionId,
        h.NumeroHabitacion,
        th.Nombre AS TipoHabitacion,
        th.PrecioBase,
        th.Capacidad,
        h.Piso
    FROM Habitacion h
    INNER JOIN TipoHabitacion th ON h.TipoHabitacionId = th.TipoHabitacionId
    WHERE h.Activo = 1
    AND h.Estado = 'Disponible'
    AND NOT EXISTS (
        SELECT 1
        FROM Reservacion r
        WHERE r.HabitacionId = h.HabitacionId
        AND r.Estado IN ('Confirmada', 'Pendiente')
        AND (
            @FechaEntrada BETWEEN r.FechaEntrada AND r.FechaSalida
            OR @FechaSalida BETWEEN r.FechaEntrada AND r.FechaSalida
        )
    )
    ORDER BY th.PrecioBase;
END;
GO