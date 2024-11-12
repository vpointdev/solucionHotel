use Hotel
Go
-- Test Case 1: Create a new user (Should succeed)
EXEC PA_Usuario_Crear 
    @NombreUsuario = 'testuser1',
    @Clave = 'password123',
    @CorreoRegistro = 'testuser1@test.com'

-- Test Case 2: Try to create duplicate user (Should fail)
EXEC PA_Usuario_Crear 
    @NombreUsuario = 'testuser1',
    @Clave = 'password123',
    @CorreoRegistro = 'testuser1@test.com'

-- Test Case 3: Create another valid user
EXEC PA_Usuario_Crear 
    @NombreUsuario = 'testuser2',
    @Clave = 'password456',
    @CorreoRegistro = 'testuser2@test.com'

-- Verify the users were created
SELECT * FROM Usuario WHERE NombreUsuario IN ('testuser1', 'testuser2')
SELECT * FROM UsuarioPorPerfil WHERE UsuarioId IN (
    SELECT UsuarioId FROM Usuario WHERE NombreUsuario IN ('testuser1', 'testuser2')
)

-- Get the UsuarioId for testuser1
DECLARE @UserId INT
SELECT @UserId = UsuarioId FROM Usuario WHERE NombreUsuario = 'testuser1'

-- Test Case 1: Update existing user (Should succeed)
EXEC PA_Usuario_Actualizar
    @UsuarioId = @UserId,
    @NombreUsuario = 'testuser1_updated',
    @Clave = 'newpassword123',
    @CorreoRegistro = 'updated1@test.com',
    @Estado = 1

-- Test Case 2: Try to update non-existent user (Should fail)
EXEC PA_Usuario_Actualizar
    @UsuarioId = 99999,
    @NombreUsuario = 'nonexistent',
    @Clave = 'password',
    @CorreoRegistro = 'none@test.com',
    @Estado = 1

-- Verify the update
SELECT * FROM Usuario WHERE UsuarioId = @UserId

-- Get the UsuarioId for testuser2
DECLARE @UserIdToDelete INT
SELECT @UserIdToDelete = UsuarioId FROM Usuario WHERE NombreUsuario = 'testuser2'

-- Test Case 1: Delete (deactivate) existing user
EXEC PA_Usuario_Eliminar @UsuarioId = @UserIdToDelete

-- Test Case 2: Try to delete already deleted user
EXEC PA_Usuario_Eliminar @UsuarioId = @UserIdToDelete

-- Verify the deletion (should show Estado = 0)
SELECT * FROM Usuario WHERE UsuarioId = @UserIdToDelete

-- Get a valid UsuarioId
DECLARE @ValidUserId INT
SELECT @ValidUserId = UsuarioId FROM Usuario WHERE NombreUsuario = 'testuser1_updated'

-- Test Case 1: Get existing active user
EXEC PA_Usuario_ObtenerPorId @UsuarioId = @ValidUserId

-- Test Case 2: Try to get deleted user
DECLARE @DeletedUserId INT
SELECT @DeletedUserId = UsuarioId FROM Usuario WHERE NombreUsuario = 'testuser2'
EXEC PA_Usuario_ObtenerPorId @UsuarioId = @DeletedUserId

-- Test Case 3: Try to get non-existent user
EXEC PA_Usuario_ObtenerPorId @UsuarioId = 99999

-- Test getting all active users
EXEC PA_Usuario_ObtenerTodos

-- Test Case 1: Authenticate with correct credentials
EXEC PA_Usuario_Autenticar 
    @NombreUsuario = 'testuser1_updated',
    @Clave = 'newpassword123'

-- Test Case 2: Try with incorrect password
EXEC PA_Usuario_Autenticar 
    @NombreUsuario = 'testuser1_updated',
    @Clave = 'wrongpassword'

-- Test Case 3: Try with non-existent user
EXEC PA_Usuario_Autenticar 
    @NombreUsuario = 'nonexistentuser',
    @Clave = 'password123'

	-- Get a valid UsuarioId
DECLARE @UserIdForProfiles INT
SELECT @UserIdForProfiles = UsuarioId FROM Usuario WHERE NombreUsuario = 'testuser1_updated'

-- Test Case 1: Get profiles for existing user
EXEC PA_Usuario_ObtenerPerfiles @UsuarioId = @UserIdForProfiles

-- Test Case 2: Get profiles for non-existent user
EXEC PA_Usuario_ObtenerPerfiles @UsuarioId = 99999

-- Test Case 3: Get profiles for inactive user
DECLARE @InactiveUserId INT
SELECT @InactiveUserId = UsuarioId FROM Usuario WHERE NombreUsuario = 'testuser2'
EXEC PA_Usuario_ObtenerPerfiles @UsuarioId = @InactiveUserId