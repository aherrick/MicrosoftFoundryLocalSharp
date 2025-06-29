using System.ClientModel;
using System.Text;
using Microsoft.AI.Foundry.Local;
using OpenAI;
using OpenAI.Chat;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Alias or model ID to run locally (e.g., mistral, phi-3, etc.)
var alias = "mistralai-Mistral-7B-Instruct-v0-2-generic-cpu";

// Start the model and get manager
var manager = await FoundryLocalManager.StartModelAsync(alias);
var model = await manager.GetModelInfoAsync(alias);
var credential = new ApiKeyCredential(manager.ApiKey);

// Setup OpenAI client pointing to local Foundry endpoint
var client = new OpenAIClient(credential, new OpenAIClientOptions { Endpoint = manager.Endpoint });

var chatClient = client.GetChatClient(model.ModelId);

// Interactive chat loop
var messages = new List<ChatMessage>();

while (true)
{
    Console.Write("Q: ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
        break;

    messages.Add(new UserChatMessage(input));

    Console.Write("A:");

    var assistantMessageStringBuilder = new StringBuilder();

    await foreach (var chatUpdate in chatClient.CompleteChatStreamingAsync(messages))
    {
        foreach (var chatMessageContentPart in chatUpdate.ContentUpdate)
        {
            assistantMessageStringBuilder.Append(chatMessageContentPart.Text);
            Console.Write(chatMessageContentPart.Text);
        }
    }

    messages.Add(new AssistantChatMessage(assistantMessageStringBuilder.ToString()));

    Console.WriteLine("\n");
}