param(
    $revert = $true
)

function migration
{
    param(
        [Parameter(Mandatory=$true)]
        $provider,
        [Parameter(Mandatory=$false)]
        $connectionString
    )

    Write-Output "Removing migration for $provider"
    dotnet ef migrations remove --startup-project ..\src\MultiDataBaseProvider --project ..\src\MultiDataBaseProvider --context "$($provider)DbContext" --no-build --force -- --Provider $provider --connectionStrings:Default $connectionString
}

if ($revert)
{
    $migration = Read-Host -Prompt "Revert to migration"

    Write-Output "Reverting database for SqlServer provider"
    dotnet ef database update $migration --startup-project ..\src\MultiDataBaseProvider --project ..\src\MultiDataBaseProvider --context "SqlServerDbContext" --configuration Release
}

migration "SqlServer"
migration "MySql" "server=localhost;port=3306;database=Insure;user=root;password=P@ssw0rd"
migration "Postgres"
migration "Oracle" "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=Insure)));User Id=system;Password=P@ssw0rd"
