USE [master]
GO

IF DB_ID('Hotel') IS NOT NULL
BEGIN
    ALTER DATABASE Hotel SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Hotel;
END
GO

CREATE DATABASE Hotel;
GO

USE Hotel;
GO

-- Ahora sí crear las tablas
CREATE TABLE Perfil (
    PerfilId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200),
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE Usuario (
    UsuarioId INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Contrasena VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE UsuarioPorPerfil (
    UsuarioPerfilId INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    PerfilId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (PerfilId) REFERENCES Perfil(PerfilId)
);
GO

CREATE TABLE TipoHabitacion (
    TipoHabitacionId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200),
    PrecioBase DECIMAL(10,2) NOT NULL,
    Capacidad INT NOT NULL,
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE Habitacion (
    HabitacionId INT PRIMARY KEY IDENTITY(1,1),
    NumeroHabitacion VARCHAR(10) NOT NULL UNIQUE,
    TipoHabitacionId INT NOT NULL,
    Piso INT NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Disponible',
    Observaciones VARCHAR(500),
    Activo BIT DEFAULT 1,
    FOREIGN KEY (TipoHabitacionId) REFERENCES TipoHabitacion(TipoHabitacionId)
);
GO

CREATE TABLE Reservacion (
    ReservacionId INT PRIMARY KEY IDENTITY(1,1),
    CodigoReservacion VARCHAR(20) NOT NULL UNIQUE,
    UsuarioId INT NOT NULL,
    HabitacionId INT NOT NULL,
    FechaEntrada DATETIME NOT NULL,
    FechaSalida DATETIME NOT NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Confirmada',
    PrecioTotal DECIMAL(10,2) NOT NULL,
    Observaciones VARCHAR(500),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (HabitacionId) REFERENCES Habitacion(HabitacionId)
);
GO

CREATE TABLE TipoPago (
    TipoPagoId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200),
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE Pago (
    PagoId INT PRIMARY KEY IDENTITY(1,1),
    ReservacionId INT NOT NULL,
    TipoPagoId INT NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    FechaPago DATETIME NOT NULL,
    NumeroTransaccion VARCHAR(50),
    Estado VARCHAR(20) NOT NULL,
    Observaciones VARCHAR(500),
    UsuarioCreacionId INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ReservacionId) REFERENCES Reservacion(ReservacionId),
    FOREIGN KEY (TipoPagoId) REFERENCES TipoPago(TipoPagoId),
    FOREIGN KEY (UsuarioCreacionId) REFERENCES Usuario(UsuarioId)
);
GO

-- Insertar datos iniciales
INSERT INTO Perfil (Nombre, Descripcion) VALUES
('Administrador', 'Acceso completo al sistema'),
('Cliente', 'Acceso a reservaciones propias');
GO

INSERT INTO Usuario (NombreUsuario, Contrasena, Email, Nombre)
VALUES 
('admin', 'admin123', 'admin@hotel.com', 'Admin System'),
('cliente', 'cliente123', 'cliente@mail.com', 'Cliente Demo');
GO

INSERT INTO UsuarioPorPerfil (UsuarioId, PerfilId)
SELECT 
    (SELECT UsuarioId FROM Usuario WHERE NombreUsuario = 'admin'),
    (SELECT PerfilId FROM Perfil WHERE Nombre = 'Administrador')
UNION ALL
SELECT 
    (SELECT UsuarioId FROM Usuario WHERE NombreUsuario = 'cliente'),
    (SELECT PerfilId FROM Perfil WHERE Nombre = 'Cliente');
GO

INSERT INTO TipoHabitacion (Nombre, Descripcion, PrecioBase, Capacidad) VALUES
('Individual', 'Habitación individual con una cama', 100.00, 1),
('Doble', 'Habitación con dos camas individuales', 175.00, 2),
('Matrimonial', 'Habitación con una cama matrimonial', 200.00, 2),
('Suite', 'Suite de lujo con sala de estar', 350.00, 4);
GO

INSERT INTO Habitacion (NumeroHabitacion, TipoHabitacionId, Piso) VALUES
('101', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Individual'), 1),
('102', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Individual'), 1),
('103', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Doble'), 1),
('104', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Doble'), 1),
('201', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Matrimonial'), 2),
('202', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Matrimonial'), 2),
('203', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Suite'), 2),
('204', (SELECT TipoHabitacionId FROM TipoHabitacion WHERE Nombre = 'Suite'), 2);
GO

INSERT INTO TipoPago (Nombre, Descripcion) VALUES
('Efectivo', 'Pago en efectivo'),
('Tarjeta de Crédito', 'Pago con tarjeta de crédito'),
('Tarjeta de Débito', 'Pago con tarjeta de débito'),
('Transferencia', 'Pago por transferencia bancaria');
GO