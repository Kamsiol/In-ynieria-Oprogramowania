# Log in to Azure (if not already done)
Connect-AzAccount

try {
    Write-Output "Attempting to retrieve the secret from Key Vault..."

    # Check if the secret exists
    $secret = Get-AzKeyVaultSecret -VaultName "IOkey" -Name "connectionDBCstring" -ErrorAction SilentlyContinue
    if ($null -eq $secret) {
        Write-Error "The secret 'connectionDBCstring' does not exist in the Key Vault."
        exit
    }

    # Retrieve the secret value
    $secureSecret = $secret.SecretValue

    # Convert SecureString to plain text string
    $connectionString = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
        [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureSecret)
    )

    Write-Output "Successfully retrieved the connection string from Key Vault."

    # Set the connection string as an environment variable for the C# application
    [System.Environment]::SetEnvironmentVariable('DB_CONNECTION_STRING', $connectionString, [System.EnvironmentVariableTarget]::Process)

    Write-Output "Connection string stored in environment variable 'DB_CONNECTION_STRING'. C# application can now access it."

} catch {
    Write-Error "Error retrieving the secret from Key Vault: $($_.Exception.Message)"
    exit
}
