param(
    [ValidateSet("SqlServer", "MySql", "Postgres", "Oracle", "")]
    $provider = ""
)

docker-compose -p insure-infra -f ./docker-compose.infra.yml up $provider -d