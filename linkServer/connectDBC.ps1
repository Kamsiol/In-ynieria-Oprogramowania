Connect-AzAccount

try {
    Write-Output "Attempting to retrieve the password from Key Vault..."
    $securePassword = (Get-AzKeyVaultSecret -VaultName "IOkey" -Name "DBPassword").SecretValue
    $plainPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
        [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
    )
    
    # Формування рядка підключення
    $connectionString = "Server=tcp:mhserverstud.database.windows.net,1433;" +
    "Database=DB_IOPROJECT;" +
    "User ID=mh308876@student.polsl.pl;" +
    "Password=$plainPassword;" +
    "Trusted_Connection=False;" +
    "Encrypt=True;" +
    "Connection Timeout=30;"

    # Динамічно створюємо унікальний файл для збереження
    $timestamp = Get-Date -Format "yyyyMMddHHmmss"
    $outputFile = Join-Path -Path (Get-Location) -ChildPath "connection_string_$timestamp.txt"
    Write-Output "Writing connection string to file: $outputFile"
    Set-Content -Path $outputFile -Value $connectionString

} catch {
    Write-Error "Error retrieving the password from Key Vault: $($_.Exception.Message)"
    exit
}