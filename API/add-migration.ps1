param ($name)

if ($name -eq $null) {
    $name = read-host -Prompt "Please enter a migration name" 
} 

dotnet ef migrations add $name --project ASSISTENTE.Persistence.MSSQL

dotnet ef migrations script --idempotent --output ASSISTENTE.DB.Upgrade/MSSQL-Migrations.sql --project ASSISTENTE.Persistence.MSSQL