using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

Console.OutputEncoding = Encoding.UTF8;

var foundryUrl = "http://localhost:5273/v1";
var modelName = "mistralai-Mistral-7B-Instruct-v0-2-generic-cpu";

IChatClient chatClient = CreateChatClient(modelName, foundryUrl);

if (!await CanChatWithModel(chatClient))
{
    await StartAndWaitForModel(modelName, chatClient);
}

await RunChatLoopAsync(chatClient);

static IChatClient CreateChatClient(string modelName, string endpoint)
{
    return new ChatClient(
        model: modelName,
        credential: new("local"),
        new OpenAI.OpenAIClientOptions { Endpoint = new(endpoint) }
    ).AsIChatClient();
}

static async Task<bool> CanChatWithModel(IChatClient chatClient)
{
    try
    {
        await foreach (
            var update in chatClient.GetStreamingResponseAsync([new(ChatRole.User, "ping")])
        )
        {
            return true;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Dry-run error: " + ex.Message);
    }

    return false;
}
static async Task StartAndWaitForModel(string modelName, IChatClient chatClient)
{
    Console.WriteLine("🚀 Starting Foundry model...");

    var psi = new ProcessStartInfo
    {
        FileName = "foundry",
        Arguments = $"model run \"{modelName}\"",
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
    };

    Process.Start(psi);

    Console.Write("⏳ Waiting for model to respond");

    while (true)
    {
        if (await CanChatWithModel(chatClient))
        {
            Console.WriteLine("\n✅ Model is ready.");
            return;
        }

        Console.Write(".");
        await Task.Delay(1000);
    }
}

static async Task RunChatLoopAsync(IChatClient chatClient)
{
    var messages = new List<Microsoft.Extensions.AI.ChatMessage>();

    while (true)
    {
        Console.Write("Q: ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            break;

        messages.Add(new(ChatRole.User, input));

        var responseText = new TextContent(string.Empty);
        var responseMessage = new Microsoft.Extensions.AI.ChatMessage(
            ChatRole.Assistant,
            [responseText]
        );

        Console.Write("A: ");
        await foreach (var update in chatClient.GetStreamingResponseAsync(messages))
        {
            Console.Write(update.Text);
            responseText.Text += update.Text;
        }

        Console.WriteLine();
        messages.Add(responseMessage);
        Console.WriteLine();
    }
}