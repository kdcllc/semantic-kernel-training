#!/bin/bash

# permissions to execute the script
# chmod +x create-kv.sh

# Prompt for resource group and vault name
echo "Please enter the vault name:"
read vaultName

keyName="OpenAIOptions--Key"
endpointName="OpenAIOptions--Endpoint"

# Prompt for secrets
echo "Please enter the value for $keyName:"
read keyValue

echo "Please enter the value for $endpointName:"
read endpointValue

# Set secrets
az keyvault secret set --vault-name $vaultName --name $keyName --value "$keyValue"
az keyvault secret set --vault-name $vaultName --name $endpointName --value "$endpointValue"