USE Hotel
GO

CREATE OR ALTER PROCEDURE PA_Usuario_Crear
    @NombreUsuario VARCHAR(50),
    @Clave VARCHAR(50),
    @CorreoRegistro VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ExistingUserId INT;
    SELECT @ExistingUserId = UsuarioId 
    FROM Usuario 
    WHERE NombreUsuario = @NombreUsuario;
    
    IF @ExistingUserId IS NOT NULL
    BEGIN
        UPDATE Usuario 
        SET Estado = 1,
            Clave = @Clave,
            CorreoRegistro = @CorreoRegistro  
        WHERE UsuarioId = @ExistingUserId;
        
        IF NOT EXISTS (SELECT 1 FROM UsuarioPorPerfil WHERE UsuarioId = @ExistingUserId AND PerfilId = 2)
        BEGIN
            INSERT INTO UsuarioPorPerfil (UsuarioId, PerfilId)
            VALUES (@ExistingUserId, 2);
        END

        SELECT @ExistingUserId AS UsuarioId; 
        RETURN 0; -- Success
    END
    
    BEGIN TRY
        BEGIN TRANSACTION;
            
        INSERT INTO Usuario (NombreUsuario, Clave, CorreoRegistro, Estado)
        VALUES (@NombreUsuario, @Clave, @CorreoRegistro, 1);
        
        DECLARE @NewUserId INT = SCOPE_IDENTITY();
        
        INSERT INTO UsuarioPorPerfil (UsuarioId, PerfilId)
        VALUES (@NewUserId, 2);
        
        SELECT @NewUserId AS UsuarioId; 
        
        COMMIT TRANSACTION;
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        -- Return error
        RETURN -1;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_Actualizar
    @UsuarioId INT,
    @NombreUsuario VARCHAR(50) = NULL,
    @Clave VARCHAR(50) = NULL,
    @CorreoRegistro VARCHAR(100) = NULL,
    @Estado BIT = NULL
AS
BEGIN
    UPDATE Usuario
    SET NombreUsuario = CASE 
            WHEN @NombreUsuario IS NOT NULL AND @NombreUsuario != '' THEN @NombreUsuario 
            ELSE NombreUsuario 
        END,
        Clave = CASE 
            WHEN @Clave IS NOT NULL AND @Clave != '' THEN @Clave 
            ELSE Clave 
        END,
        CorreoRegistro = CASE 
            WHEN @CorreoRegistro IS NOT NULL AND @CorreoRegistro != '' THEN @CorreoRegistro 
            ELSE CorreoRegistro 
        END,
        Estado = CASE 
            WHEN @Estado IS NOT NULL THEN @Estado 
            ELSE Estado 
        END
    WHERE UsuarioId = @UsuarioId
END
GO

CREATE OR ALTER PROCEDURE PA_Usuario_Eliminar
    @UsuarioId INT
AS
BEGIN
    UPDATE Usuario
    SET Estado = 0
    WHERE UsuarioId = @UsuarioId
END
GO

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
END
GO

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
END
GO

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

CREATE OR ALTER PROCEDURE PA_Usuario_ObtenerPerfiles 
    @NombreUsuario VARCHAR(50)
AS
BEGIN
    SELECT DISTINCT
        p.PerfilId,
        p.Nombre,
        p.Estado
    FROM Perfil p
    INNER JOIN UsuarioPorPerfil up ON p.PerfilId = up.PerfilId
    INNER JOIN Usuario u ON up.UsuarioId = u.UsuarioId
    WHERE u.NombreUsuario = @NombreUsuario
        AND u.Estado = 1          
        AND p.Estado = 1         
    ORDER BY p.PerfilId;     
END
GO