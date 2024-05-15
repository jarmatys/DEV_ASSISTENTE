param (
    [switch] $d,
    [switch] $s,
    [switch] $a,
    [switch] $p
)

if ($d)
{
    docker compose -f .\docker-compose.yml --profile database up -d
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
    docker compose -f .\docker-compose.yml --profile playground up -d
}