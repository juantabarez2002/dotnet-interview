# REST API Documentation

This document provides detailed documentation for the RESTful API endpoints available for managing Todo Lists and Todo Items, developed using **C# + ASP.NET**.

---

## Endpoints

| Resource      | Method | Path                                                             | Description                                  | Success | Error(s) |
|---------------|--------|------------------------------------------------------------------|----------------------------------------------|---------|----------|
| **Todo Lists**| GET    | `/api/todolists`                                                 | Get every list with its items.               | 200     | 500 |
|               | GET    | `/api/todolists/{{id}}`                                          | Get a single list.                           | 200     | 404, 500 |
|               | POST   | `/api/todolists`                                                | Create a new list.                           | 201 + **Location** header | 400, 500 |
|               | PUT    | `/api/todolists/{{id}}`                                          | Update list name.                            | 204     | 400, 404 |
|               | DELETE | `/api/todolists/{{id}}`                                          | Remove list (and its items).                 | 204     | 404 |
| **Todo Items**| POST   | `/api/todolists/{{listId}}/items`                                | Create an item inside a list.                | 201 + **Location** header | 400, 404 |
|               | GET    | `/api/todolists/{{listId}}/items/{{itemId}}`                     | Get one item.                                | 200     | 404 |
|               | PUT    | `/api/todolists/{{listId}}/items/{{itemId}}`                     | Replace description.                         | 204     | 400, 404 |
|               | PATCH  | `/api/todolists/{{listId}}/items/{{itemId}}/complete`            | Mark item as completed.                      | 204     | 404 |
|               | DELETE | `/api/todolists/{{listId}}/items/{{itemId}}`                     | Delete item.                                 | 204     | 404 |




## Schemas

### TodoList

```json
{{
  "id": 1,
  "name": "Work",
  "items": [
    {{ "...": "See TodoItem schema below" }}
  ]
}}
```

| Field        | Type    | Description                 |
|--------------|---------|-----------------------------|
| `id`         | number  | Unique identifier.          |
| `name`       | string  | Name.        |
| `items`      | array   | Collection of TodoItem.     |

### TodoItem

```json
{{
  "id": 12,
  "description": "Finish report",
  "isCompleted": false,
  "todoListId": 1
}}
```

| Field          | Type     | Description                              |
|----------------|----------|------------------------------------------|
| `id`           | number   | Unique identifier.                       |
| `description`  | string   | Task description.                        |
| `isCompleted`  | boolean  | **true** if task is done.               |
| `todoListId`   | number   | Foreign key to parent list.              |

### CreateTodoList (request)

```json
{{
  "name": "New List"
}}
```

| Field | Type   | Required | Notes |
|-------|--------|----------|-------|
| `name`| string | ✔        | 1‑40 characters. |

### UpdateTodoList (request)

Same as **CreateTodoList**.

### CreateTodoItem (request)

```json
{{
  "description": "Item Description"
}}
```

| Field         | Type   | Required | Notes |
|---------------|--------|----------|-------|
| `description` | string | ✔        | 1‑140 characters. |

### UpdateTodoItem (request)

Same as **CreateTodoItem**.

### Error

```json
{{
  "message": "List not found"
}}
```

| Field     | Type   | Description                  |
|-----------|--------|------------------------------|
| `message` | string | Error detail. |



## Error Catalogue

| Code | Meaning           | Typical Reason                              |
|------|-------------------|---------------------------------------------|
| 400  | Bad Request       | Validation failed (missing name, etc.).   |
| 404  | Not Found         | List or item with provided id does not exist|
| 500  | Internal Error    | Unhandled server error.                     |


