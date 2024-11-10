USE Hotel;
GO

-- Crear pago
CREATE OR ALTER PROCEDURE PA_Pago_Crear
(
    @ReservacionId INT,
    @TipoPagoId INT,
    @Monto DECIMAL(10,2),
    @NumeroTransaccion VARCHAR(50),
    @UsuarioCreacionId INT,
    @Observaciones VARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Validar que la reservación existe y está confirmada
        IF NOT EXISTS (
            SELECT 1 
            FROM Reservacion 
            WHERE ReservacionId = @ReservacionId 
            AND Estado IN ('Confirmada', 'CheckOut')
        )
            THROW 50000, 'La reservación no existe o no está en estado válido para pagos.', 1;

        -- Validar que el monto no exceda el precio total de la reservación
        DECLARE @PrecioTotal DECIMAL(10,2)
        DECLARE @TotalPagado DECIMAL(10,2)

        SELECT @PrecioTotal = PrecioTotal
        FROM Reservacion
        WHERE ReservacionId = @ReservacionId;

        SELECT @TotalPagado = ISNULL(SUM(Monto), 0)
        FROM Pago
        WHERE ReservacionId = @ReservacionId
        AND Estado = 'Completado';

        IF (@TotalPagado + @Monto) > @PrecioTotal
            THROW 50001, 'El monto del pago excede el saldo pendiente.', 1;

        -- Insertar el pago
        INSERT INTO Pago (
            ReservacionId,
            TipoPagoId,
            Monto,
            FechaPago,
            NumeroTransaccion,
            Estado,
            Observaciones,
            UsuarioCreacionId
        )
        VALUES (
            @ReservacionId,
            @TipoPagoId,
            @Monto,
            GETDATE(),
            @NumeroTransaccion,
            'Completado',
            @Observaciones,
            @UsuarioCreacionId
        );

        -- Retornar el pago creado
        SELECT 
            p.*,
            tp.Nombre AS TipoPago,
            r.CodigoReservacion,
            u.Nombre AS NombreUsuario
        FROM Pago p
        INNER JOIN TipoPago tp ON p.TipoPagoId = tp.TipoPagoId
        INNER JOIN Reservacion r ON p.ReservacionId = r.ReservacionId
        INNER JOIN Usuario u ON p.UsuarioCreacionId = u.UsuarioId
        WHERE p.PagoId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO