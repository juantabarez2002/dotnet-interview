
# MCP Tools Documentation

This document details the tools exposed by the MCP server for interacting with the Todo API through natural language commands, specifically designed for use with Claude Desktop.



## Available MCP Tools

| Tool Name          | Description                                        | Parameters                                                                                      | Returns* |
|--------------------|----------------------------------------------------|-------------------------------------------------------------------------------------------------|----------|
| `create_todo_item` | Creates a new todo item inside a list.             | `todoListId` (number) – ID of the list<br>`description` (string) – text of the item             | `TodoItem` JSON of the newly‑created item |
| `update_todo_item` | Updates the description of an existing item.       | `todoListId` (number), `itemId` (number), `description` (string)                                | Updated `TodoItem` JSON |
| `complete_todo_item` | Marks an item as completed.                      | `todoListId` (number), `itemId` (number)                                                        | Updated `TodoItem` JSON with `isCompleted:true` |
| `delete_todo_item` | Deletes a specific item.                           | `todoListId` (number), `itemId` (number)                                                        | Plain‑text confirmation |
| `list_todo_lists`  | Lists all todo lists.                              | – (no parameters)                                                                               | Array `TodoList[]` |
| `get_todo_list`    | Retrieves a specific list and its items.           | `todoListId` (number)                                                                           | `TodoList` JSON |
| `create_todo_list` | Creates a new todo list.                           | `name` (string) – list name                                                                     | `TodoList` JSON of the newly‑created list |
| `update_todo_list` | Renames an existing list.                          | `todoListId` (number), `name` (string)                                                          | Updated `TodoList` JSON |
| `delete_todo_list` | Deletes a list **and all its items** *(destructive)*. | `todoListId` (number)                                                                           | Plain‑text confirmation |

\* The exact JSON shape is shown in **Schemas** below.



## Schemas

<details>
<summary>TodoItem</summary>

```jsonc
{
  "id": 42,
  "description": "Item Description",
  "isCompleted": false,
  "todoListId": 7
}
```
</details>

<details>
<summary>TodoList</summary>

```jsonc
{
  "id": 7,
  "name": "List Name",
  "items": [
    {
      "id": 42,
      "description": "Item Description",
      "isCompleted": false,
      "todoListId": 7
    }
  ]
}
```
</details>



## Error Responses

| Status | When it happens                             | Example body |
|--------|---------------------------------------------|--------------|
| **400 Bad Request** | Invalid or missing parameters (e.g. negative `todoListId`, empty `description`). | `{ "message": "Validation error" }` |
| **404 Not Found**  | List or item ID does not exist. | `{ "message": "TodoList 99 not found" }` |
| **500 Internal Server Error** | Unexpected failure in downstream API. | `{ "message": "Unexpected error, please try again later." }` |

---

## Example Prompts for Claude

> The AI only needs the natural‑language prompt; it will map arguments automatically.

### Lists

| Goal | Natural‑language prompt |
|------|------------------------|
| Create list | `Create a new todo list called "Work"` |
| List lists  | `Show me all my todo lists` |
| Rename list | `Rename the list with id 3 to "Personal"` |
| Delete list | `Delete the list with id 3` |

### Items

| Goal | Natural‑language prompt |
|------|------------------------|
| Add item | `Add "Finish quarterly report" to list 1` |
| Update item | `Change the description of item 10 in list 1 to "Review final report"` |
| Complete item | `Mark item 10 in list 1 as done` |
| Delete item | `Remove item 10 from list 1` |



### Tool‑chain Example

1. `list_todo_lists` → Find list with name “Marketing” (`id = 5`).
2. `create_todo_item` (`todoListId=5`, `description="Prepare Q2 campaign"`)
3. Return the created item JSON back to the user.


