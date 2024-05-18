param (
    [switch] $d,
    [switch] $s,
    [switch] $a,
    [switch] $p
)

if ($d)
{
    docker compose -f .\docker-compose.yml --profile database up
}
elseif ($s)
{
    docker compose -f .\docker-compose.yml --profile setup up -d
}
elseif ($a)
{
    docker compose -f .\docker-compose.yml --profile app up -d
}
elseif ($p)
{
    docker compose -f .\docker-compose.yml --profile playground up
}
else
{
    Write-Host "Please provide a profile to start the enviroment: -s, -d, -p, -a (setup, database, playground, app)"
}