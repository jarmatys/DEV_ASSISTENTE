param ($name)

if ($name -eq $null) {
    $name = read-host -Prompt "Please enter a migration name" 
} 

dotnet ef migrations add $name --project ASSISTENTE.Persistence.MSSQL