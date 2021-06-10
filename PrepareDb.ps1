[cmdletbinding()]
Param(
    [string] $ServerInstance = ".",
    [string] $DatabaseName = "TestDb"
)

Function PrepareDb() {
    DropDatabase
    CreateDatabase
    SetupDatabase
    GenerateEfClasses
}

Function DropDatabase() {
    Write-Host "Drop database '$DatabaseName'..."
    $query = "IF ( DB_ID( '$DatabaseName' ) IS NOT NULL ) 
BEGIN
    EXEC(' ALTER DATABASE $DatabaseName SET SINGLE_USER WITH ROLLBACK IMMEDIATE' )
    EXEC(' DROP DATABASE $DatabaseName' )
END"
    Invoke-Sqlcmd -Query $query -ServerInstance $ServerInstance -QueryTimeout 65535
}

Function CreateDatabase() {
    Write-Host "Start creating database '$DatabaseName'..."
    $variables = "DbName=$DatabaseName", "DbSize=128MB", "DbFileGrowth=32MB", "LogSize=512MB", "LogFileGrowth=64MB", "Collation=Latin1_General_CI_AS"
    Invoke-Sqlcmd -InputFile "$PSScriptRoot\CreateDb.sql" -ServerInstance $ServerInstance -QueryTimeout 65535 -Variable $variables
    Write-Host "Database '$DatabaseName' created"
}

Function SetupDatabase() {
    $sqlText = [System.IO.File]::ReadAllText( "$PSScriptRoot\Schema.sql" )
    $sqlOut = $(Invoke-Sqlcmd -Query $sqlText -ServerInstance $ServerInstance -Database $DatabaseName -QueryTimeout 65535 -Verbose) 4>&1
    if ( $sqlOut ) {
        Write-Host $sqlOut        
    }
    Write-Host "Setup database '$DatabaseName' completed" -ForegroundColor Green
}

Function GenerateEfClasses() {
    $dbProject = "$PSScriptRoot\EfSqlServerIdentity\EfSqlServerIdentity\EfSqlServerIdentity.csproj"
    $contextDir = "$PSScriptRoot\EfSqlServerIdentity\EfSqlServerIdentity\Model\"

    dotnet ef dbcontext scaffold --schema "dbo" "Server=$($ServerInstance);Database=$($DatabaseName);Integrated Security=True;Connection Timeout=10" "Microsoft.EntityFrameworkCore.SqlServer" --data-annotations --project "$dbProject" --context MyContext --context-dir "$contextDir" --output-dir "$contextDir" -f
}

PrepareDb