Note: This sample app assumes you're familiar with Docker and .NET.

## Setup instructions

1. Run the following docker commands to set up CPU and GPU oriented containers:

  ```bash
  docker pull ollama/ollama
  
  docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollamaCPU ollama/ollama
  
  docker run -d --gpus=all -v ollama:/root/.ollama -p 11500:11434 --name ollamaGPU ollama/ollama
  ```

2. Run the following ollama commands inside of one of the containers to pull down the models to the shared container volume:

  ```bash
  ollama pull phi3:mini
  ollama pull codellama
  ollama pull nomic-embed-text
  ```

3. Clone and open the .NET project in this repo.
4. Open the project in VS Code or Visual Studio or your editor of choice.
5. Uncomment the tests you want to run in the `Program.cs` file.
6. Run the project using the run button or the `dotnet run` command from the root of the project directory.
