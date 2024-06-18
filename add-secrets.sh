#!/bin/bash

# enable script
chmod +x add-secrets.sh

# Prompt for resource group and vault name
echo "Please enter the vault name:"
read vaultName

# Get the project directory
# src/Moedim.GenAI.Demos.App/Moedim.GenAI.Demos.App.csproj
echo "Please enter the project directory:"
read projectDir

# Define the secrets
declare -a secrets=(
    "OpenAIOptions--Key"
    "OpenAIOptions--Endpoint"
)

# If the .env file does not exist, create it and populate it with the secrets from the Key Vault
for name in "${secrets[@]}"; do
    key_vault_name_with_dash=$(echo $name | sed 's/--/:/g')
    value=$(az keyvault secret show --name $name --vault-name $vaultName --query value -o tsv)
    if [ -z "$value" ]; then
        echo "No value returned for $name from Key Vault $vaultName"
        continue
    else
        echo "Successfully retrieved $key_vault_name_with_dash from Key Vault $vaultName"
        echo "Successfully created $key_vault_name_with_dash"

        dotnet user-secrets set "$key_vault_name_with_dash" "$value" --project "$projectDir"
    fi

done