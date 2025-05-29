import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { z } from "zod";
import axios from "axios";

const API_BASE = process.env.TODO_API_BASE || "http://api:80/api";
const client = axios.create({ baseURL: API_BASE });

const server = new McpServer({ name: "Todo", version: "1.0.0" });

server.tool(
  "create_todo_item",
  "Crea un ítem en una lista. Inputs: todoListId (number), description (string).",
  { todoListId: z.number(), description: z.string() },
  async ({ todoListId, description }) => {
    const r = await client.post(`/todolists/${todoListId}/items`, { description });
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "update_todo_item",
  "Actualiza descripción de un ítem. Inputs: todoListId, itemId, description.",
  { todoListId: z.number(), itemId: z.number(), description: z.string() },
  async ({ todoListId, itemId, description }) => {
    const r = await client.put(
      `/todolists/${todoListId}/items/${itemId}`,
      { description }
    );
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "complete_todo_item",
  "Marca ítem como completado. Inputs: todoListId, itemId.",
  { todoListId: z.number(), itemId: z.number() },
  async ({ todoListId, itemId }) => {
    await client.patch(`/todolists/${todoListId}/items/${itemId}/complete`);
    return {
      content: [{ type: "text", text: `Ítem ${itemId} de lista ${todoListId} completado` }]
    };
  }
);

server.tool(
  "delete_todo_item",
  "Elimina un ítem. Inputs: todoListId, itemId.",
  { todoListId: z.number(), itemId: z.number() },
  async ({ todoListId, itemId }) => {
    await client.delete(`/todolists/${todoListId}/items/${itemId}`);
    return {
      content: [{ type: "text", text: `Ítem ${itemId} de lista ${todoListId} eliminado` }]
    };
  }
);

server.tool(
  "list_todo_lists",
  "Lista todas las listas de tareas (sin inputs).",
  {},
  async () => {
    const r = await client.get(`/todolists`);
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "get_todo_list",
  "Obtiene una lista por ID. Inputs: todoListId (number).",
  { todoListId: z.number() },
  async ({ todoListId }) => {
    const r = await client.get(`/todolists/${todoListId}`);
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "create_todo_list",
  "Crea una nueva lista. Inputs: name (string).",
  { name: z.string() },
  async ({ name }) => {
    const r = await client.post(`/todolists`, { name });
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "update_todo_list",
  "Actualiza el nombre de una lista. Inputs: todoListId (number), name (string).",
  { todoListId: z.number(), name: z.string() },
  async ({ todoListId, name }) => {
    const r = await client.put(`/todolists/${todoListId}`, { name });
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "delete_todo_list",
  "Elimina una lista. Inputs: todoListId (number).",
  { todoListId: z.number() },
  async ({ todoListId }) => {
    await client.delete(`/todolists/${todoListId}`);
    return {
      content: [{ type: "text", text: `TodoList ${todoListId} eliminada` }]
    };
  }
);

export { server };
