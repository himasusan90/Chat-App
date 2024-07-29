using GroqSharp;
using GroqSharp.Models;

class Program
{
    static async Task Main(string[] args)
    {
        // Retrieve API key from environment variable
        var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("API key not found. Please set the GROQ_API_KEY environment variable.");
            return;
        }

        // Set the model to use
        var apiModel = "llama3-70b-8192"; // Replace with the correct model as needed

        // Initialize the Groq client
        IGroqClient groqClient = new GroqClient(apiKey, apiModel)
            .SetTemperature(0.5)
            .SetMaxTokens(256)
            .SetTopP(1)
            .SetStop("NONE");

        // Initialize chat history
        //  GroqSharp.Models.Message
        var chatHistory = new System.Collections.Generic.List<GroqSharp.Models.Message>();
        chatHistory.Add(new GroqSharp.Models.Message { Role = GroqSharp.Models.MessageRoleType.System, Content = "You are a helpful, smart, kind, and efficient AI assistant. You always fulfill the user's requests to the best of your ability." });

        // Start the chat loop
        while (true)
        {
            Console.Write("You: ");
            var userInput = Console.ReadLine();
            if (string.IsNullOrEmpty(userInput))
            {
                break;
            }

            // Add user message to chat history
            chatHistory.Add(new Message { Role = MessageRoleType.User, Content = userInput });

            string? response = await groqClient.CreateChatCompletionAsync(chatHistory.ToArray());

            // Assuming response.Choices is a collection of messages
            var assistantReply = response;

            if (!string.IsNullOrEmpty(assistantReply))
            {
                // Add assistant's reply to chat history
                chatHistory.Add(new Message { Role = MessageRoleType.Assistant, Content = assistantReply });

                // Print the assistant's reply
                Console.WriteLine($"Assistant: {assistantReply}");
            }
            else
            {
                Console.WriteLine("No response from assistant.");
            }
        }
    }
}
