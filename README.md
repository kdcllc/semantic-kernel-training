# Semantic Kernel Training

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://raw.githubusercontent.com/kdcllc/semantic-kernel-training/master/LICENSE)

Welcome to the Semantic Kernel Training repository! In this repository, we explore the fascinating world of Semantic Kernel using C#.

![I Stand With Israel](./images/IStandWithIsrael.png)

## Hire me

Please send [email](mailto:kingdavidconsulting@gmail.com) if you consider to **hire me**.

[![buymeacoffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/vyve0og)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## What is Semantic Kernel?

Semantic Kernel is an innovative technology that allows you to supercharge your problem-solving creativity. It’s like having your own “Easy Bake Oven” for artificial intelligence! With Semantic Kernel, you can run AI prompts right on your local machine, harnessing the power of LLM (Large Language Models) to enhance your projects.

## Usage

This code utilizes Azure Key Vault to store secure values. It is possible to replace it with secrets.

```bash

    dotnet run -- -t ChatSimpleDemo

```

### Using local LLM models

The fastest and easiest way to get local LLM running is to use [llamafile](https://github.com/Mozilla-Ocho/llamafile/). Follow the instructions based on operating system

```json
  "OpenAIOptions": {
    "Azure": false,
    "Key": "no-secret-key-needed",
    "CompletionModelId": "LLaMA_CPP",
    "Endpoint": "http://127.0.0.1:8080/v1/chat/completions"
  }
```

and registration can be done like so

```csharp
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                    kernel.AddOpenAIChatCompletion(
                        modelId: options.CompletionModelId,
                        endpoint: new Uri(options.Endpoint),
                        apiKey: options.Key,
                        serviceId: keyed);
#pragma warning restore SKEXP0010 /
```


## References

- [Using Semantic Kernel with Dependency Injection](https://devblogs.microsoft.com/semantic-kernel/using-semantic-kernel-with-dependency-injection/)