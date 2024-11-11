USE Hotel
GO

CREATE PROCEDURE PA_Usuario_Crear
    @Usuario VARCHAR(50),
    @Password VARCHAR(50),
    @Fecha DATETIME,
    @Correo VARCHAR(100),
    @Estado BIT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        IF EXISTS (SELECT 1 FROM Usuario WHERE NombreUsuario = @Usuario)
        BEGIN
            ROLLBACK TRANSACTION
            RETURN 0
        END
        INSERT INTO Usuario (NombreUsuario, Clave, FechaRegistro, CorreoRegistro, Estado)
        VALUES (@Usuario, @Password, @Fecha, @Correo, @Estado)
        
        INSERT INTO UsuarioPorPerfil (UsuarioId, PerfilId)
        VALUES (SCOPE_IDENTITY(), 2) -- 2 = Cliente (Usando el Codigo de Perfil)
        
        COMMIT TRANSACTION
        RETURN 1
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        RETURN 0
    END CATCH
END
GO

CREATE PROCEDURE PA_Usuario_Actualizar
    @Usuario VARCHAR(50),
    @Pass VARCHAR(50),
    @Fecha DATETIME,
    @Correo VARCHAR(100),
    @Estado BIT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE NombreUsuario = @Usuario)
        BEGIN
            ROLLBACK TRANSACTION
            RETURN 0
        END
        
        UPDATE Usuario 
        SET Clave = @Pass,
            FechaRegistro = @Fecha,
            CorreoRegistro = @Correo,
            Estado = @Estado
        WHERE NombreUsuario = @Usuario
        
        COMMIT TRANSACTION
        RETURN 1
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        RETURN 0
    END CATCH
END
GO

CREATE PROCEDURE PA_Usuario_Eliminar
    @Usuario VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE NombreUsuario = @Usuario)
        BEGIN
            ROLLBACK TRANSACTION
            RETURN 0
        END
        
        DELETE FROM UsuarioPorPerfil
        WHERE UsuarioId = (SELECT UsuarioId FROM Usuario WHERE NombreUsuario = @Usuario)
        
        UPDATE Usuario 
        SET Estado = 0
        WHERE NombreUsuario = @Usuario
        
        COMMIT TRANSACTION
        RETURN 1
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        RETURN 0
    END CATCH
END
GO

CREATE PROCEDURE PA_Usuario_ObtenerPorId
    @Usuario VARCHAR(50)
AS
BEGIN
    SELECT 
        u.UsuarioId,
        u.NombreUsuario,
        u.Clave,
        u.FechaRegistro,
        u.CorreoRegistro,
        u.Estado
    FROM Usuario u
    WHERE u.NombreUsuario = CASE 
        WHEN @Usuario = '''''' THEN u.NombreUsuario 
        ELSE @Usuario 
    END
    AND u.Estado = 1
END
GO

CREATE PROCEDURE PA_Usuario_Autenticar
    @Usuario VARCHAR(50),
    @Clave VARCHAR(50)
AS
BEGIN
    SELECT 
        u.UsuarioId,
        u.NombreUsuario,
        u.Clave,
        u.FechaRegistro,
        u.CorreoRegistro,
        u.Estado
    FROM Usuario u
    WHERE u.NombreUsuario = @Usuario
    AND u.Clave = @Clave
    AND u.Estado = 1
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_ObtenerPerfiles
    @Usuario VARCHAR(50)
AS
BEGIN
    SELECT 
        p.Codigo,        
        p.Descripcion,   
        p.Estado        
    FROM Perfil p
    INNER JOIN UsuarioPorPerfil up ON p.Codigo = up.PerfilId
    INNER JOIN Usuario u ON up.UsuarioId = u.UsuarioId
    WHERE u.NombreUsuario = @Usuario
    AND u.Estado = 1
    AND p.Estado = 1;
END
GO