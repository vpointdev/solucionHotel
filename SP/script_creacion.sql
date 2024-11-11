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

CREATE TABLE Perfil (
    Codigo INT PRIMARY KEY IDENTITY(1,1),           
    Descripcion VARCHAR(200),                       
    Estado BIT DEFAULT 1                            
);
GO

CREATE TABLE Usuario (
    UsuarioId INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Clave VARCHAR(50) NOT NULL,
    CorreoRegistro VARCHAR(100) NOT NULL UNIQUE,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Estado BIT DEFAULT 1
);
GO

CREATE TABLE UsuarioPorPerfil (
    UsuarioPerfilId INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    PerfilId INT NOT NULL,                       
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (PerfilId) REFERENCES Perfil(Codigo)  
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
    Estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    PrecioTotal DECIMAL(10,2) NOT NULL,
    PagoProcesado BIT DEFAULT 0,
    Observaciones VARCHAR(500),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId),
    FOREIGN KEY (HabitacionId) REFERENCES Habitacion(HabitacionId)
);
GO

CREATE TABLE Pago (
    PagoId INT PRIMARY KEY IDENTITY(1,1),
    ReservacionId INT NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    FechaPago DATETIME NOT NULL,
    NumeroTransaccion VARCHAR(50),
    Estado VARCHAR(20) NOT NULL DEFAULT 'Completado',
    EsCargoCancelacion BIT NOT NULL DEFAULT(0),
    Observaciones VARCHAR(500),
    UsuarioCreacionId INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ReservacionId) REFERENCES Reservacion(ReservacionId),
    FOREIGN KEY (UsuarioCreacionId) REFERENCES Usuario(UsuarioId)
);
GO

-- Datos de prueba
INSERT INTO TipoHabitacion (Nombre, Descripcion, PrecioBase, Capacidad) VALUES
('Individual', 'Habitación individual con una cama', 100.00, 1),
('Doble', 'Habitación con dos camas individuales', 175.00, 2),
('Matrimonial', 'Habitación con una cama matrimonial', 200.00, 2),
('Suite', 'Suite de lujo con sala de estar', 350.00, 4),
('Suite Ejecutiva', 'Suite con sala de estar y oficina', 450.00, 4),
('Suite Presidencial', 'Suite de lujo con todas las comodidades', 800.00, 6);
GO

INSERT INTO Habitacion (NumeroHabitacion, TipoHabitacionId, Piso) VALUES
('101', 1, 1), ('102', 1, 1), ('103', 1, 1),
('104', 2, 1), ('105', 2, 1), ('106', 2, 1),
('201', 3, 2), ('202', 3, 2), ('203', 3, 2), ('204', 3, 2),
('301', 4, 3), ('302', 4, 3), ('303', 5, 3), ('304', 5, 3),
('401', 6, 4), ('402', 6, 4);
GO

INSERT INTO Perfil (Descripcion, Estado) VALUES
('Administrador', 1),    -- Código 1 = Admin
('Cliente', 1);          -- Código 2 = Cliente
GO

INSERT INTO Usuario (NombreUsuario, Clave, CorreoRegistro, Estado) VALUES 
('admin', 'admin123', 'admin@hotel.com', 1),
('cliente1', 'cliente123', 'cliente1@mail.com', 1),
('cliente2', 'cliente123', 'cliente2@mail.com', 1),
('cliente3', 'cliente123', 'cliente3@mail.com', 1);
GO

INSERT INTO UsuarioPorPerfil (UsuarioId, PerfilId)
SELECT 
    (SELECT UsuarioId FROM Usuario WHERE NombreUsuario = 'admin'),
    (SELECT Codigo FROM Perfil WHERE Descripcion = 'Administrador')
UNION ALL
SELECT 
    (SELECT UsuarioId FROM Usuario WHERE NombreUsuario = 'cliente1'),
    (SELECT Codigo FROM Perfil WHERE Descripcion = 'Cliente')
UNION ALL
SELECT 
    (SELECT UsuarioId FROM Usuario WHERE NombreUsuario = 'cliente2'),
    (SELECT Codigo FROM Perfil WHERE Descripcion = 'Cliente')
UNION ALL
SELECT 
    (SELECT UsuarioId FROM Usuario WHERE NombreUsuario = 'cliente3'),
    (SELECT Codigo FROM Perfil WHERE Descripcion = 'Cliente');
GO