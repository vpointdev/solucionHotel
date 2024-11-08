# SQL Project Setup Instructions

## Project Files Overview
The solution consists of two main files:
1. Solution File (`.ssmssln`)
2. Project File (`.ssmssqlproj`)

## Solution File Setup
1. Rename `SqlSolution.ssmssln.template` to `YourSolutionName.ssmssln`
2. Replace the following placeholders:
   - `[PROJECT_NAME]`: Your project name (e.g., "HotelDB")
   - `[PROJECT_GUID]`: Generate a new GUID (e.g., using Visual Studio or online GUID generator)
   - `[SOLUTION_GUID]`: Generate another new GUID

### Example:
```text
Project("{4F2E2C19-372F-40D8-9FA7-9D2138C6997A}") = "HotelDB", "HotelDB.ssmssqlproj", "{A1B2C3D4-E5F6-4747-8899-1234567890AB}"
```

## Project File Setup
1. Rename `SqlProject.ssmssqlproj.template` to match your project name (e.g., `HotelDB.ssmssqlproj`)
2. Replace the following placeholders:
   - `[SERVER_NAME]`: Your SQL Server instance name (e.g., "LOCALHOST", "DESKTOP-ABC123")
   - `[USER_NAME]`: Your Windows username or SQL Server authentication username

### Required Changes in Project File
#### In ConnectionNode:
```xml
<ConnectionNode Name="[SERVER_NAME]:[USER_NAME]">
  <Server>[SERVER_NAME]</Server>
</ConnectionNode>
```

#### In FileNode:
```xml
<AssociatedConnectionMoniker>8c91a03d-f9b4-46c0-a305-b5dcc79ff907:[SERVER_NAME]:True</AssociatedConnectionMoniker>
<AssociatedConnSrvName>[SERVER_NAME]</AssociatedConnSrvName>
```

## Project Structure
- `.ssmssln` file: Solution container that manages one or more projects
- `.ssmssqlproj` file: Individual project containing:
  1. `Connections`: SQL Server connection settings
  2. `Queries`: SQL script files
  3. `Miscellaneous`: Additional resources

## Generating GUIDs
You can generate GUIDs using:
- Visual Studio: Tools > Create GUID
- PowerShell: `[guid]::NewGuid()`
- Online GUID generators
- VS Code extensions

## Recommended Setup Process
1. Copy both template files
2. Generate new GUIDs for project and solution
3. Rename files according to your project
4. Update placeholders in both files
5. Update server and authentication information
6. Add your SQL files to the Queries folder
7. Open the solution in SQL Server Management Studio

## File Naming Conventions
- Solution: `YourSolutionName.ssmssln`
- Project: `YourProjectName.ssmssqlproj`
- Scripts: Use prefixes for ordering (e.g., "01_", "02_")

## Common Issues
- If you get connection errors, verify your server name is correct
- Ensure you have appropriate permissions on the SQL Server instance
- Check that Windows Authentication is enabled if you're using it
- Make sure GUIDs are unique and properly formatted
- Verify all file paths in the solution file match your project structure

## Version Control Notes
- The template files should be committed to the repository
- Developer-specific files should be listed in `.gitignore`
- Consider using environment-specific connection settings

## Notes
- The GUID in `AssociatedConnectionMoniker` is standard and should remain unchanged
- Authentication is set to Windows Authentication by default
- TrustServerCertificate is set to True by default
- LoginTimeout is set to 30 seconds by default