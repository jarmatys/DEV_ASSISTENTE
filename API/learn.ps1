param (
    [switch] $b
)

$currentPath = Get-Location

if($b) {
    Write-Host "Current Path: $currentPath" -ForegroundColor Green
    
    Write-Host "Building Playground" -ForegroundColor Green
    
    dotnet build ASSISTENTE.Playground --configuration Release -property:SolutionDir=$currentPath
    
    Write-Host "Coping appsettings.json" -ForegroundColor Green
        
    cp appsettings.json ASSISTENTE.Playground/bin/Release/net8.0
}

Write-Host "Reseting knowledge base" -ForegroundColor Green
& 'ASSISTENTE.Playground/bin/Release/net8.0/ASSISTENTE.Playground.exe' -r

Write-Host "Starting learning from scratch" -ForegroundColor Green
& 'ASSISTENTE.Playground/bin/Release/net8.0/ASSISTENTE.Playground.exe' -l
