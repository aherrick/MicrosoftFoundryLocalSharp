using Microsoft.Extensions.AI;
using OpenAI.Chat;

Console.OutputEncoding = System.Text.Encoding.UTF8;

IChatClient chatClient = new ChatClient(
    model: "mistralai-Mistral-7B-Instruct-v0-2-generic-cpu",
    credential: new("local"),
    new OpenAI.OpenAIClientOptions() { Endpoint = new("http://localhost:5273/v1") }
).AsIChatClient();

List<Microsoft.Extensions.AI.ChatMessage> messages = [];
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