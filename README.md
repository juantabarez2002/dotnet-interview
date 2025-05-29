
# Jr AI Full Stack Developer Interview

This project extends a basic REST API developed using **C# + ASP.NET** for managing todo lists and tasks (TodoItems), integrating Model Context Protocol (MCP) to enable natural language interactions specifically designed for use with **Claude Desktop**.

## Project Demo

Watch a step-by-step video demonstrating how to deploy and run this project:

[Watch Demo Video on YouTube](https://youtu.be/5FwAyA4VOYU)

## Prerequisites

- Docker installed.
- Claude Desktop installed (recommended).

## How to Deploy the Project

Execute all commands from the project's root directory `(dotnet-interview)`.

1. **Run the API and Database** using Docker Compose:

```bash
docker-compose up -d
```

2. **Build the MCP Server Image**:

```bash
docker build -t todo-mcp ./MCPServer
```

3. **Configure Claude Desktop** to use the MCP server:

In Claude Desktop, add this configuration:

```json
{
  "mcpServers": {
    "todo": {
      "command": "docker",
      "args": [
        "run", "-i", "--rm",
        "-e", "TODO_API_BASE=http://host.docker.internal:5500/api",
        "todo-mcp"
      ],
      "env": {}
    }
  }
}
```

## Documentation

- [REST API Documentation](api-documentation.md)
- [MCP Tools Documentation](mcp-tools-documentation.md)

## Example MCP Prompts

- **Create item**: "Create an item in the 'Work' list with description 'Finish report'."
- **Update item**: "Update item 'Finish report' to 'Review final report' in 'Work' list."
- **Complete item**: "Mark item 'Review final report' as completed in 'Work' list."
- **Delete item**: "Delete item 'Review final report' from 'Work' list."


## Author

Juan  Tabarez   
[LinkedIn](https://www.linkedin.com/in/juan-tabarez/) · [Email](mailto:jats2002@hotmail.com)
