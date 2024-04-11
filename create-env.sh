#!/bin/bash

# Prompt for resource group and vault name
echo "Please enter the vault name:"
read vaultName

# Define the secrets
declare -a secrets=(
    "OpenAIOptions--Key"
    "OpenAIOptions--Endpoint"
)

# Define the .env file
env_file=".env.azure"

# Check if the .env file exists
if [ ! -f "$env_file" ]; then
    # If the .env file does not exist, create it and populate it with the secrets from the Key Vault
    for name in "${secrets[@]}"; do
        key_vault_name_with_dash=$(echo $name | tr '-' '_')
        value=$(az keyvault secret show --name $name --vault-name $vaultName --query value -o tsv)
        if [ -z "$value" ]; then
            echo "No value returned for $name from Key Vault $vaultName"
            continue
        fi
        echo "$key_vault_name_with_dash=$value" >> $env_file
        
        echo "Settting $key_vault_name_with_dash"
        export $key_vault_name_with_dash="$value"
        #printenv | grep $key_vault_name_with_dash

        # Check if the export was successful
        if [ -z "${!key_vault_name_with_dash}" ]; then
            echo "Failed to export $key_vault_name_with_dash"
        else
            echo "Successfully exported $key_vault_name_with_dash"
        fi
    done
fi