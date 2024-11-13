USE Hotel
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_Obtener
AS
BEGIN
    SELECT 
        r.ReservacionId, 
        r.CodigoReservacion, 
        r.UsuarioId, 
        r.HabitacionId, 
        r.FechaEntrada, 
        r.FechaSalida, 
        r.EstadoReservacion, 
        r.PrecioTotal, 
        r.Observaciones,
        h.NumeroHabitacion,
        t.Nombre AS TipoHabitacionNombre
    FROM Reservacion r
    JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    JOIN TipoHabitacion t ON h.TipoHabitacionId = t.TipoHabitacionId
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_ObtenerPorUsuario
    @UsuarioId INT
AS
BEGIN
    SELECT 
        r.ReservacionId, 
        r.CodigoReservacion, 
        r.UsuarioId, 
        r.HabitacionId, 
        r.FechaEntrada, 
        r.FechaSalida, 
        r.EstadoReservacion, 
        r.PrecioTotal, 
        r.Observaciones,
        h.NumeroHabitacion,
        t.Nombre AS TipoHabitacionNombre
    FROM Reservacion r
    JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    JOIN TipoHabitacion t ON h.TipoHabitacionId = t.TipoHabitacionId
    WHERE r.UsuarioId = @UsuarioId
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_ObtenerPorCodigo
    @CodigoReservacion VARCHAR(50)
AS
BEGIN
    SELECT 
        r.ReservacionId, 
        r.CodigoReservacion, 
        r.UsuarioId, 
        r.HabitacionId, 
        r.FechaEntrada, 
        r.FechaSalida, 
        r.EstadoReservacion, 
        r.PrecioTotal, 
        r.Observaciones,
        h.NumeroHabitacion,
        t.Nombre AS TipoHabitacionNombre
    FROM Reservacion r
    JOIN Habitacion h ON r.HabitacionId = h.HabitacionId
    JOIN TipoHabitacion t ON h.TipoHabitacionId = t.TipoHabitacionId
    WHERE r.CodigoReservacion = @CodigoReservacion
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_Agregar
    @CodigoReservacion VARCHAR(50),
    @UsuarioId INT,
    @HabitacionId INT,
    @FechaEntrada DATETIME,
    @FechaSalida DATETIME,
    @EstadoReservacion VARCHAR(50),
    @PrecioTotal DECIMAL(18,2),
    @Observaciones VARCHAR(500)
AS
BEGIN
    INSERT INTO Reservacion (
        CodigoReservacion, 
        UsuarioId, 
        HabitacionId, 
        FechaEntrada, 
        FechaSalida, 
        EstadoReservacion, 
        PrecioTotal, 
        Observaciones
    ) VALUES (
        @CodigoReservacion,
        @UsuarioId,
        @HabitacionId,
        @FechaEntrada,
        @FechaSalida,
        @EstadoReservacion,
        @PrecioTotal,
        @Observaciones
    )
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_Actualizar
    @ReservacionId INT,
    @CodigoReservacion VARCHAR(50),
    @UsuarioId INT,
    @HabitacionId INT,
    @FechaEntrada DATETIME,
    @FechaSalida DATETIME,
    @EstadoReservacion VARCHAR(50),
    @PrecioTotal DECIMAL(18,2),
    @Observaciones VARCHAR(500)
AS
BEGIN
    UPDATE Reservacion
    SET 
        CodigoReservacion = @CodigoReservacion,
        UsuarioId = @UsuarioId,
        HabitacionId = @HabitacionId,
        FechaEntrada = @FechaEntrada,
        FechaSalida = @FechaSalida,
        EstadoReservacion = @EstadoReservacion,
        PrecioTotal = @PrecioTotal,
        Observaciones = @Observaciones
    WHERE ReservacionId = @ReservacionId
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_Eliminar
    @ReservacionId INT
AS
BEGIN
    DELETE FROM Reservacion
    WHERE ReservacionId = @ReservacionId
END
GO

CREATE OR ALTER PROCEDURE PA_Reservacion_Cancelar
    @ReservacionId INT
AS
BEGIN
    UPDATE Reservacion
    SET EstadoReservacion = 'Cancelado'
    WHERE ReservacionId = @ReservacionId
END
GO