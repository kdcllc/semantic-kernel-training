#!/bin/bash

# Path to the .env.azure file
ENV_FILE=".env.azure"

# Path to the launchSettings.json file
LAUNCH_SETTINGS="src/Moedim.GenAI.Demos.App/Properties/launchSettings.json"

# Temporary file for jq output
TEMP_FILE=$(mktemp)

# Read each line in the .env.azure file
while IFS= read -r line
do
  # Split the line into KEY and VALUE
  IFS='=' read -ra parts <<< "$line"
  KEY="${parts[0]}"
  VALUE="${parts[1]}"

  # Update the launchSettings.json file
  jq --arg key "$KEY" --arg value "$VALUE" \
    '.profiles."Demos.App".environmentVariables[$key] = $value' \
    "$LAUNCH_SETTINGS" > "$TEMP_FILE" && mv "$TEMP_FILE" "$LAUNCH_SETTINGS"
done < "$ENV_FILE"
