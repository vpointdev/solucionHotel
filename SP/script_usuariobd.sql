SELECT SERVERPROPERTY('IsIntegratedSecurityOnly');
-- Returns 1 if Windows Authentication only, 0 if Mixed Mode

-- If it returns 1, you need to enable Mixed Mode Authentication in SQL Server

-- Create login if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'usr_Acceso')
BEGIN
    CREATE LOGIN [usr_Acceso] WITH PASSWORD=N'12345678', 
    DEFAULT_DATABASE=[Hotel], 
    CHECK_EXPIRATION=OFF, 
    CHECK_POLICY=OFF
END
GO

-- Create database user and grant permissions
USE [Hotel]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'usr_Acceso')
BEGIN
    CREATE USER [usr_Acceso] FOR LOGIN [usr_Acceso]
    ALTER ROLE [db_owner] ADD MEMBER [usr_Acceso]  -- Or use more specific roles if needed
END
GO