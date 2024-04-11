#!/bin/bash

echo "Before running this script make sure you are logged in with Azure CLI."

# Check if the Azure CLI is installed
if ! [ -x "$(command -v az)" ]; then
    echo "Error: Azure CLI is not installed. Please install it before running this script."
    exit 1
else
    echo "Azure CLI is installed"
fi

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
        else
            echo "Successfully retrieved $name from Key Vault $vaultName"
            echo "$key_vault_name_with_dash=$value" >> $env_file
            echo "Successfully exported $key_vault_name_with_dash"
        fi
    done
else
    echo "The $env_file file already exists"
fi