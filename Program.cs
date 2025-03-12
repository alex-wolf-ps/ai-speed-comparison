using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using OpenAI;
using Microsoft.Extensions.AI;

// Conversational clients
IChatClient chatClientCPU =
    new OllamaChatClient(new Uri("http://localhost:11434/"), "phi3:mini");
IChatClient chatClientGPU =
    new OllamaChatClient(new Uri("http://localhost:11500/"), "phi3:mini");

// Code gen clients
IChatClient codeClientCPU =
    new OllamaChatClient(new Uri("http://localhost:11434/"), "codellama");
IChatClient codeClientGPU =
    new OllamaChatClient(new Uri("http://localhost:11500/"), "codellama");

// Embedding clients
IEmbeddingGenerator<string,Embedding<float>> embeddingClientCPU = 
    new OllamaEmbeddingGenerator(new Uri("http://localhost:11434/"), "nomic-embed-text");
IEmbeddingGenerator<string,Embedding<float>> embeddingClientGPU = 
    new OllamaEmbeddingGenerator(new Uri("http://localhost:11500/"), "nomic-embed-text");

// Generate a story
// await GenerativePrompt(chatClientCPU, "CPU", 
//     "Write me a 3 paragraph short story about woodland creatures that follows the hero's cycle.");
// await GenerativePrompt(chatClientGPU, "GPU", 
//     "Write me a 3 paragraph short story about woodland creatures that follows the hero's cycle.");

// // Answer a real question
// await GenerativePrompt(chatClientCPU, "CPU", 
// "Write a detailed summary of the evolution of military technology over the last 2000 years.");
// await GenerativePrompt(chatClientGPU, "GPU", 
// "Write a detailed summary of the evolution of military technology over the last 2000 years.");

// Code Generation
// await GenerativePrompt(codeClientCPU, "CPU", 
// "Write the C# code for a minimal API that performs CRUD operations on product data.");
// await GenerativePrompt(codeClientGPU, "GPU", 
// "Write the C# code for a minimal API that performs CRUD operations on product data.");

// // Embedding generation
var text = @"The history of military technology spans back at least 2,000 years and reflects humanity' endless quest to gain an advantage in conflict situations through superior weaponry, tactics, and strategies. Initially, warfare was a matter of brute force with armies relying on infantry using spears, swords, axes, shields, and later the development of ranged weapons like bows and crossbows for combat at distances further than hand-to-hand encounters allowed.
The evolution can be traced through several key periods: 
1. The Ancient World (up to around 500 AD): Armies were composed mainly of infantry supported by chariots, with a focus on mass and shock tactics exemplified in the phalanx formations used by Greek city-states like Sparta or Macedon under Philip II and Alexander the Great. The Romans later perfected these into an even more disciplined force that dominated their empire.
2. Medieval Period (500 - 1400 AD): Knighthood rose to prominence with heavy cavalry as decisive on-the-ground forces, and infantry employed longbows or crossbows for ranged combat in significant conflicts such as the Hundred Years' War between England and France. Siege warfare became critical during this period due to fortified cities becoming commonplace defensive measures against invasions.
3. Renaissance (1400 - 18th century): The introduction of gunpowder led by Chinese innovation revolutionized European battlefields, leading to the development of cannons and muskets—firearms that replaced traditional melee weapons in most military forces. Notable advancements during this period included pike formations for infantry as well as early use of naval artillery on warships like galleys used by Mediterranean powers such as Venice or the Ottoman Empire's Janissaries, who were elite soldiers trained to fight both at sea and land.
4. Industrial Revolution (18th-20th century): The introduction of steam power significantly impacted naval warfare with ironclad ships replacing wooden sailing vessels; artillery continued to improve in range, accuracy, and destructive capability while the telegraph revolutionized command and control methods on battlefields across Europe.
5. World Wars (20th century): The 19th-century inventions like machine guns and trench warfare reached new heights of lethality during both world wars with corresponding developments in armored vehicles, aircraft, submarines, and naval vessels including battleships that were designed to carry large caliber guns.
6. Modern Military Technology (20th-21st century): The atomic age introduced nuclear weapons as a deterrent power rather than offensive weaponry due to the concept of mutually assured destruction; subsequent technological advancements saw ballistic missile submarines, stealth aircraft like the F-117 Nighthawk (later succeeded by more advanced jets such as F-22 Raptors and fifth generation fighters), precision guidance systems for munitions to minimize collateral damage. Today’s military technology is dominated by computerized warfare with artificial intelligence playing an increasing role in decision making, cyber capabilities are now significant threats (and potential tools) alongside traditional kinetic weapons like drones and hypersonic glide vehicles that continue the ever-evolving narrative of military technological advancement.
Throughout these periods, innovations were often driven by necessity as much as rivalry among powers seeking to protect their interests or expand them through conquests – a dynamic which continues in various forms today with countries like China and Russia investing heavily into modernizing their armed forces amid global geopolitical tensions.";

await EmbeddingPrompt(embeddingClientCPU, "CPU", text);
await EmbeddingPrompt(embeddingClientGPU, "GPU", text);


async Task GenerativePrompt(IChatClient chatClient, string hardware, string userPrompt)
{
    List<ChatMessage> chatHistory = new();

    // Get user prompt and add to chat history
    Console.WriteLine(userPrompt);
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    // Start the stopwatch
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    // Stream the AI response and add to chat history
    Console.WriteLine("AI Response:");
    var response = "";
    await foreach (var item in
        chatClient.GetStreamingResponseAsync(chatHistory))
    {
        Console.Write(item.Text);
        response += item.Text;
    }
    stopWatch.Stop();
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine();
    
    // Print the elapsed time
    TimeSpan ts = stopWatch.Elapsed;
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        Console.WriteLine($"{hardware} time: {elapsedTime}");
}

async Task EmbeddingPrompt(IEmbeddingGenerator<string,Embedding<float>> generator, string hardware, string userPrompt)
{
    // Start the stopwatch
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    // Stream the AI response and add to chat history
    Console.WriteLine("AI Response:");
    var embedding = await generator.GenerateAsync([userPrompt]);
    Console.WriteLine(embedding[0].Vector);
    Console.WriteLine();
    
    // Print the elapsed time
    TimeSpan ts = stopWatch.Elapsed;
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        Console.WriteLine($"{hardware} time: {elapsedTime}");
}