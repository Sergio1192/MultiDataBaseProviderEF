param(
    [Parameter(Mandatory=$true)]
    $message
)

function migration
{
    param(
        [Parameter(Mandatory=$true)]
        $message,
        [Parameter(Mandatory=$true)]
        $provider,
        [Parameter(Mandatory=$false)]
        $connectionString
    )

    Write-Output "Migration for $provider"

    dotnet ef migrations add $message.Replace(" ", "") --startup-project ..\MultiDataBaseProvider --project ..\MultiDataBaseProvider --context "$($provider)DbContext" --output-dir Migrations/$provider --configuration Release -- --Provider $provider --connectionStrings:Default $connectionString
}

migration $message "SqlServer"
migration $message "MySql" "server=localhost;port=3306;database=Insure;user=root;password=P@ssw0rd"
migration $message "Postgres"
migration $message "Oracle"