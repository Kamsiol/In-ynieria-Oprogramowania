Connect-AzAccount

try {
    Write-Output "Attempting to retrieve the password from Key Vault..."
    $securePassword = (Get-AzKeyVaultSecret -VaultName "IOkey" -Name "DBPassword").SecretValue
    $plainPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
        [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
    )
    
    # Формування рядка підключення
    $connectionString = "mssql+pyodbc://mh308876@student.polsl.pl:$plainPassword@mhserverstud.database.windows.net/DB_IOPROJECT?driver=ODBC+Driver+17+for+SQL+Server&authentication=ActiveDirectoryPassword"


    # Динамічно створюємо унікальний файл для збереження
    $timestamp = Get-Date -Format "yyyyMMddHHmmss"
    $outputFile = Join-Path -Path (Get-Location) -ChildPath "connection_string_$timestamp.txt"
    Write-Output "Writing connection string to file: $outputFile"
    Set-Content -Path $outputFile -Value $connectionString

} catch {
    Write-Error "Error retrieving the password from Key Vault: $($_.Exception.Message)"
    exit
}
