USE Hotel;
GO

-- PA_Usuario_Autenticar
CREATE PROCEDURE [dbo].[PA_Usuario_Autenticar]
    @NombreUsuario VARCHAR(50),
    @Contrasena VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT U.*, P.PerfilId, P.Nombre AS NombrePerfil 
    FROM Usuario U
    INNER JOIN UsuarioPorPerfil UP ON U.UsuarioId = UP.UsuarioId
    INNER JOIN Perfil P ON UP.PerfilId = P.PerfilId
    WHERE U.NombreUsuario = @NombreUsuario 
    AND U.Contrasena = @Contrasena 
    AND U.Activo = 1;
END
GO

-- PA_Usuario_Crear
CREATE PROCEDURE [dbo].[PA_Usuario_Crear]
    @NombreUsuario VARCHAR(50),
    @Contrasena VARCHAR(50),
    @Email VARCHAR(100),
    @Nombre VARCHAR(100),
    @PerfilId INT = 2  -- Por defecto es Cliente (PerfilId = 2)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
            
        -- Insertar Usuario
        INSERT INTO Usuario (NombreUsuario, Contrasena, Email, Nombre)
        VALUES (@NombreUsuario, @Contrasena, @Email, @Nombre);
        
        DECLARE @UsuarioId INT = SCOPE_IDENTITY();
            
        -- Asignar Perfil
        INSERT INTO UsuarioPorPerfil (UsuarioId, PerfilId)
        VALUES (@UsuarioId, @PerfilId);
            
        COMMIT TRANSACTION;
        SELECT @UsuarioId AS UsuarioId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        THROW;
    END CATCH
END
GO

-- PA_Usuario_Consultar
CREATE PROCEDURE [dbo].[PA_Usuario_Consultar]
    @NombreUsuario VARCHAR(50) = ''
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        U.*,
        P.PerfilId,
        P.Nombre AS NombrePerfil
    FROM Usuario U
    INNER JOIN UsuarioPorPerfil UP ON U.UsuarioId = UP.UsuarioId
    INNER JOIN Perfil P ON UP.PerfilId = P.PerfilId
    WHERE (@NombreUsuario = '' OR U.NombreUsuario = @NombreUsuario)
    AND U.Activo = 1;
END
GO

-- PA_Usuario_Actualizar
CREATE PROCEDURE [dbo].[PA_Usuario_Actualizar]
    @UsuarioId INT,
    @NombreUsuario VARCHAR(50),
    @Contrasena VARCHAR(50),
    @Email VARCHAR(100),
    @Nombre VARCHAR(100),
    @PerfilId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE Usuario
        SET NombreUsuario = @NombreUsuario,
            Contrasena = @Contrasena,
            Email = @Email,
            Nombre = @Nombre
        WHERE UsuarioId = @UsuarioId;
        
        UPDATE UsuarioPorPerfil
        SET PerfilId = @PerfilId
        WHERE UsuarioId = @UsuarioId;
        
        COMMIT TRANSACTION;
        
        SELECT @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        THROW;
    END CATCH
END
GO

-- PA_Usuario_Eliminar
CREATE PROCEDURE [dbo].[PA_Usuario_Eliminar]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Usuario
    SET Activo = 0
    WHERE UsuarioId = @UsuarioId;
    
    SELECT @@ROWCOUNT;
END
GO

-- PA_Usuario_ObtenerPerfiles
CREATE PROCEDURE [dbo].[PA_Usuario_ObtenerPerfiles]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT DISTINCT P.*
    FROM Perfil P
    INNER JOIN UsuarioPorPerfil UP ON P.PerfilId = UP.PerfilId
    WHERE UP.UsuarioId = @UsuarioId
    AND P.Activo = 1;
END
GO

-- PA_Usuario_CambiarPerfil
CREATE PROCEDURE [dbo].[PA_Usuario_CambiarPerfil]
    @UsuarioId INT,
    @PerfilId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE UsuarioPorPerfil
        SET PerfilId = @PerfilId
        WHERE UsuarioId = @UsuarioId;
        
        COMMIT TRANSACTION;
        
        SELECT @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        THROW;
    END CATCH
END
GO