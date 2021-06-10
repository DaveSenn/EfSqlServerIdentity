<#
    .SYNOPSIS
        Drops an existing Argos DB
    
    .DESCRIPTION
        - Drops the DB

    .PARAMETER ServerInstance
        The server instance to run the SQL file.

    .PARAMETER DatabaseName
        The database name to run the SQL file.

    .NOTES
        Author:         Dave Senn
        Creation Date:  2020-08-11
        (c) Logisoft AG
#>
[cmdletbinding()]
Param(
    [string] $ServerInstance = ".",
    [string] $DatabaseName = "Argos"
)

# Drops the database
Function DropDatabase() {
    Write-Host "Drop database '$DatabaseName'..."
    $query = "IF ( DB_ID( '$DatabaseName' ) IS NOT NULL ) 
BEGIN
    EXEC(' ALTER DATABASE $DatabaseName SET SINGLE_USER WITH ROLLBACK IMMEDIATE' )
    -- EXEC(' ALTER DATABASE $DatabaseName SET OFFLINE' )
    -- EXEC(' ALTER DATABASE $DatabaseName SET ONLINE' )
    EXEC(' DROP DATABASE $DatabaseName' )
END"
    Invoke-Sqlcmd -Query $query -ServerInstance $ServerInstance -QueryTimeout 65535
}

# Drop the database
DropDatabase