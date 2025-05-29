import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";
import { z } from "zod";
import axios from "axios";

const API_BASE = process.env.TODO_API_BASE || "http://api:80/api";
const client = axios.create({ baseURL: API_BASE });

const server = new McpServer({ name: "Todo", version: "1.0.0" });

server.tool(
  "create_todo_item",
  "Crea un ítem en una lista. Inputs: listId (number), description (string).",
  { listId: z.number(), description: z.string() },
  async ({ listId, description }) => {
    const r = await client.post(`/todolists/${listId}/items`, { description });
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "update_todo_item",
  "Actualiza descripción de un ítem. Inputs: listId, itemId, description.",
  { listId: z.number(), itemId: z.number(), description: z.string() },
  async ({ listId, itemId, description }) => {
    const r = await client.put(
      `/todolists/${listId}/items/${itemId}`,
      { description }
    );
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "complete_todo_item",
  "Marca ítem como completado. Inputs: listId, itemId.",
  { listId: z.number(), itemId: z.number() },
  async ({ listId, itemId }) => {
    await client.patch(`/todolists/${listId}/items/${itemId}/complete`);
    return {
      content: [{ type: "text", text: `Ítem ${itemId} de lista ${listId} completado` }]
    };
  }
);

server.tool(
  "delete_todo_item",
  "Elimina un ítem. Inputs: listId, itemId.",
  { listId: z.number(), itemId: z.number() },
  async ({ listId, itemId }) => {
    await client.delete(`/todolists/${listId}/items/${itemId}`);
    return {
      content: [{ type: "text", text: `Ítem ${itemId} de lista ${listId} eliminado` }]
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
  "Obtiene una lista por ID. Inputs: listId (number).",
  { listId: z.number() },
  async ({ listId }) => {
    const r = await client.get(`/todolists/${listId}`);
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
  "Actualiza el nombre de una lista. Inputs: listId (number), name (string).",
  { listId: z.number(), name: z.string() },
  async ({ listId, name }) => {
    const r = await client.put(`/todolists/${listId}`, { name });
    return { content: [{ type: "text", text: JSON.stringify(r.data) }] };
  }
);

server.tool(
  "delete_todo_list",
  "Elimina una lista. Inputs: listId (number).",
  { listId: z.number() },
  async ({ listId }) => {
    await client.delete(`/todolists/${listId}`);
    return {
      content: [{ type: "text", text: `TodoList ${listId} eliminada` }]
    };
  }
);

export { server };
