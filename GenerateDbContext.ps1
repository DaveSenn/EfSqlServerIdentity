Function GenerateEfClasses() {
    $dbProject = "$PSScriptRoot\EfSqlServerIdentity\EfSqlServerIdentity\EfSqlServerIdentity.csproj"
    $contextDir = "$PSScriptRoot\EfSqlServerIdentity\EfSqlServerIdentity\Model\"

    dotnet ef dbcontext scaffold --schema "dbo" "Server=.;Database=TestDb;Integrated Security=True;Connection Timeout=10" "Microsoft.EntityFrameworkCore.SqlServer" --data-annotations --project "$dbProject" --context MyContext --context-dir "$contextDir" --output-dir "$contextDir" -f
}

GenerateEfClasses