
$currentPath = Get-Location

Write-Host "Current Path: $currentPath" -ForegroundColor Green

Write-Host "Building ASSISTENTE.DB.Upgrade" -ForegroundColor Green
    
dotnet build ASSISTENTE.DB.Upgrade --configuration Release -property:SolutionDir=$currentPath
    
Write-Host "Coping appsettings.json" -ForegroundColor Green
        
cp appsettings.json ASSISTENTE.DB.Upgrade/bin/Release/net8.0

Write-Host "Reseting knowledge base" -ForegroundColor Green
& 'ASSISTENTE.DB.Upgrade/bin/Release/net8.0/ASSISTENTE.DB.Upgrade.exe'