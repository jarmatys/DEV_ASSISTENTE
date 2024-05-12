param ($name)

if ($name -eq $null) {
    $name = read-host -Prompt "Please enter a migration name" 
} 

dotnet ef migrations add $name --project 'ASSISTENTE.Persistence.Configuration'

dotnet ef migrations script --idempotent --output ASSISTENTE.DB.Upgrade/migrations.sql --project ASSISTENTE.Persistence.Configuration