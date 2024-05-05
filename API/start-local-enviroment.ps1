param (
    [switch] $db,
    [switch] $b
)

if ($db)
{
    Write-Host "Starting local enviroment + database upgrade" -ForegroundColor Green
    docker compose -f .\docker-compose.database.yml -f .\docker-compose.yml up -d
}
elseif ($b)
{
    Write-Host "Rebuild and starting local enviroment" -ForegroundColor Green
    docker compose -f .\docker-compose.yml up -d --build
}
else
{
    Write-Host "Starting local enviroment" -ForegroundColor Green
    docker compose -f .\docker-compose.yml up -d
}