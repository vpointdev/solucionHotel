USE Hotel
GO

-- Create User
CREATE OR ALTER PROCEDURE PA_Usuario_Crear
    @NombreUsuario VARCHAR(50),
    @Clave VARCHAR(50),
    @CorreoRegistro VARCHAR(100)
AS
BEGIN
    INSERT INTO Usuario (NombreUsuario, Clave, CorreoRegistro)
    VALUES (@NombreUsuario, @Clave, @CorreoRegistro)
    
    DECLARE @UserId INT = SCOPE_IDENTITY()
    
    INSERT INTO UsuarioPorPerfil (UsuarioId, PerfilId)
    VALUES (@UserId, 2)
    
    RETURN @UserId
END
GO

-- Update User
CREATE OR ALTER PROCEDURE PA_Usuario_Actualizar
    @UsuarioId INT,
    @NombreUsuario VARCHAR(50),
    @Clave VARCHAR(50),
    @CorreoRegistro VARCHAR(100),
    @Estado BIT
AS
BEGIN
    UPDATE Usuario
    SET NombreUsuario = @NombreUsuario,
        Clave = @Clave,
        CorreoRegistro = @CorreoRegistro,
        Estado = @Estado
    WHERE UsuarioId = @UsuarioId
END
GO

-- Delete User
CREATE OR ALTER PROCEDURE PA_Usuario_Eliminar
    @UsuarioId INT
AS
BEGIN
    UPDATE Usuario
    SET Estado = 0
    WHERE UsuarioId = @UsuarioId
END
GO

-- Get User by ID
CREATE OR ALTER PROCEDURE PA_Usuario_ObtenerPorId
    @UsuarioId INT
AS
BEGIN
    SELECT 
        UsuarioId,
        NombreUsuario,
        Clave,
        FechaRegistro,
        CorreoRegistro,
        Estado
    FROM Usuario
    WHERE UsuarioId = @UsuarioId
        AND Estado = 1
END
GO

-- Get All Users
CREATE OR ALTER PROCEDURE PA_Usuario_ObtenerTodos
AS
BEGIN
    SELECT 
        UsuarioId,
        NombreUsuario,
        Clave,
        FechaRegistro,
        CorreoRegistro,
        Estado
    FROM Usuario
    WHERE Estado = 1
END
GO

-- Authenticate User
CREATE OR ALTER PROCEDURE PA_Usuario_Autenticar
    @NombreUsuario VARCHAR(50),
    @Clave VARCHAR(50)
AS
BEGIN
    SELECT 
        UsuarioId,
        NombreUsuario,
        Clave,
        FechaRegistro,
        CorreoRegistro,
        Estado
    FROM Usuario
    WHERE NombreUsuario = @NombreUsuario
        AND Clave = @Clave
        AND Estado = 1
END
GO

-- Get User Profiles
CREATE OR ALTER PROCEDURE PA_Usuario_ObtenerPerfiles
    @UsuarioId INT
AS
BEGIN
    SELECT 
        p.PerfilId,
        p.Nombre,
        p.Estado
    FROM Perfil p
    INNER JOIN UsuarioPorPerfil up ON p.PerfilId = up.PerfilId
    INNER JOIN Usuario u ON up.UsuarioId = u.UsuarioId
    WHERE u.UsuarioId = @UsuarioId
        AND u.Estado = 1
        AND p.Estado = 1
END
GO