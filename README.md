# Microsoft Foundry Local Console App (C#)

This project is a minimal streaming chat console app using `Microsoft.Extensions.AI` and a local Foundry-hosted model.

## ✅ Prerequisites

1. Install the latest [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

2. Install and configure [Microsoft Foundry Local](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/get-started).

3. Start the local model:

```bash
foundry model run mistralai-Mistral-7B-Instruct-v0-2-generic-cpu
```

4. Clone and run the console app:

```bash
git clone https://github.com/aherrick/MicrosoftFoundryLocalSharp
cd MicrosoftFoundryLocalSharp
dotnet run
```

## 💬 Example Q&A

```
Q: What is the capital of France?
A: The capital of France is Paris.
```

## 🔗 Resources

- [Foundry Local Documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-local/get-started)
- [.NET 9 SDK Downloads](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
