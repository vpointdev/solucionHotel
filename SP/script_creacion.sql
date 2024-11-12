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
    PerfilId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
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
    FOREIGN KEY (PerfilId) REFERENCES Perfil(PerfilId)
);
GO

-- Create room-related tables
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
    Estado VARCHAR(20) DEFAULT 'Disponible', -- Disponible, Ocupada, Mantenimiento
    Observaciones VARCHAR(500),
    Activo BIT DEFAULT 1,
    FOREIGN KEY (TipoHabitacionId) REFERENCES TipoHabitacion(TipoHabitacionId)
);
GO

INSERT INTO Perfil (Nombre, Estado) VALUES
('Administrador', 1),
('Cliente', 1);
GO

INSERT INTO Usuario (NombreUsuario, Clave, CorreoRegistro) VALUES 
('admin', 'admin123', 'admin@hotel.com'),
('cliente', 'cliente123', 'cliente@mail.com');
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

-- Insert room types
INSERT INTO TipoHabitacion (Nombre, Descripcion, PrecioBase, Capacidad) VALUES
('Individual', 'Habitación individual con una cama', 100.00, 1),
('Doble', 'Habitación con dos camas individuales', 175.00, 2),
('Matrimonial', 'Habitación con una cama matrimonial', 200.00, 2),
('Suite', 'Suite de lujo con sala de estar', 350.00, 4);
GO

-- Insert sample rooms
INSERT INTO Habitacion (NumeroHabitacion, TipoHabitacionId, Piso) VALUES
('101', 1, 1), -- Individual rooms
('102', 1, 1),
('201', 2, 2), -- Double rooms
('202', 2, 2),
('301', 3, 3), -- Matrimonial rooms
('302', 3, 3),
('401', 4, 4), -- Suites
('402', 4, 4);
GO