CREATE OR ALTER PROCEDURE PA_Usuario_Crear
(
    @NombreUsuario VARCHAR(50),
    @Contrasena VARCHAR(50),
    @Nombre VARCHAR(100),
    @Email VARCHAR(100),
    @PerfilId INT
)
AS
BEGIN
    INSERT INTO Usuario
    (NombreUsuario, Contrasena, Nombre, Email, Activo)
    VALUES
    (@NombreUsuario, @Contrasena, @Nombre, @Email, 1)

    DECLARE @UsuarioId INT = SCOPE_IDENTITY()

    INSERT INTO UsuarioPorPerfil
    (UsuarioId, PerfilId)
    VALUES
    (@UsuarioId, @PerfilId)

    SELECT UsuarioId, NombreUsuario, Nombre, Email
    FROM Usuario
    WHERE UsuarioId = @UsuarioId
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_ObtenerPorId
(
    @UsuarioId INT
)
AS
BEGIN
    SELECT  u.UsuarioId,
            u.NombreUsuario,
            u.Nombre,
            u.Email,
            p.PerfilId,
            p.Nombre AS NombrePerfil
    FROM Usuario u
    INNER JOIN UsuarioPorPerfil up ON u.UsuarioId = up.UsuarioId
    INNER JOIN Perfil p ON p.PerfilId = up.PerfilId
    WHERE u.UsuarioId = @UsuarioId
    AND u.Activo = 1
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_Actualizar
(
    @UsuarioId INT,
    @Nombre VARCHAR(100),
    @Email VARCHAR(100)
)
AS
BEGIN
    UPDATE Usuario
    SET Nombre = @Nombre,
        Email = @Email
    WHERE UsuarioId = @UsuarioId
    AND Activo = 1

    SELECT UsuarioId, NombreUsuario, Nombre, Email
    FROM Usuario
    WHERE UsuarioId = @UsuarioId
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_CambiarContrasena
(
    @UsuarioId INT,
    @ContrasenaActual VARCHAR(50),
    @ContrasenaNueva VARCHAR(50)
)
AS
BEGIN
    UPDATE Usuario
    SET Contrasena = @ContrasenaNueva
    WHERE UsuarioId = @UsuarioId
    AND Contrasena = @ContrasenaActual
    AND Activo = 1

    SELECT UsuarioId
    FROM Usuario
    WHERE UsuarioId = @UsuarioId
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_Desactivar
(
    @UsuarioId INT
)
AS
BEGIN
    UPDATE Usuario
    SET Activo = 0
    WHERE UsuarioId = @UsuarioId
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_ListarActivos
AS
BEGIN
    SELECT  u.UsuarioId,
            u.NombreUsuario,
            u.Nombre,
            u.Email,
            p.Nombre AS NombrePerfil
    FROM Usuario u
    INNER JOIN UsuarioPorPerfil up ON u.UsuarioId = up.UsuarioId
    INNER JOIN Perfil p ON p.PerfilId = up.PerfilId
    WHERE u.Activo = 1
    ORDER BY u.NombreUsuario
END
GO