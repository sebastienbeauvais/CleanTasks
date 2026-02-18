# CleanTasks — Implementation Requirements

This project is a **Task Management Web API** built using Clean Architecture. The solution structure, interfaces, DTOs, dependency injection, and unit tests are already in place. Your job is to implement the logic that makes the tests pass and the API functional.

Run `dotnet test` at any time to check your progress. All tests should fail initially (TDD).

---

## Architecture Overview

```
API (Presentation) → Application → Domain ← Infrastructure
```

- **Domain**: Entities, enums, and repository interfaces. This layer has zero dependencies. It defines *what* data looks like and *what* operations are available, but not *how* they work.
- **Application**: Service interfaces, DTOs, and service implementations. Depends only on Domain. Contains business logic, validation, and mapping between entities and DTOs.
- **Infrastructure**: Repository implementations. Depends on Domain (for interfaces/entities) and Application. Contains the actual data storage mechanism.
- **API**: Controllers and program startup. Depends on Application and Infrastructure. Translates HTTP requests into service calls and service results into HTTP responses.

> **Key principle**: Dependencies point inward. Domain knows nothing about the other layers. Application knows about Domain but not Infrastructure. The API wires everything together.

---

## Implementation Order

Work bottom-up so each layer is testable before you move on:

1. **Infrastructure** — Repositories
2. **Application** — Services
3. **API** — Controllers

---

## Phase 1: Infrastructure — Repository Implementations

### `InMemoryCategoryRepository`
**File**: `src/CleanTasks.Infrastructure/Repositories/InMemoryCategoryRepository.cs`
**Implements**: `ICategoryRepository`

Store categories in a thread-safe, in-memory collection keyed by their `Id`.

| Method | Behavior |
|---|---|
| `AddAsync` | Store the category and return it |
| `GetByIdAsync` | Return the category with the matching id, or `null` |
| `GetAllAsync` | Return all stored categories |
| `GetByNameAsync` | Return the category whose name matches (case-insensitive), or `null` |
| `UpdateAsync` | If a category with the given id exists, replace it and return the updated version. Otherwise return `null` |
| `DeleteAsync` | Remove the category by id. Return `true` if it was found and removed, `false` otherwise |

### `InMemoryTaskRepository`
**File**: `src/CleanTasks.Infrastructure/Repositories/InMemoryTaskRepository.cs`
**Implements**: `ITaskRepository`

Store tasks in a thread-safe, in-memory collection keyed by their `Id`.

| Method | Behavior |
|---|---|
| `AddAsync` | Store the task and return it |
| `GetByIdAsync` | Return the task with the matching id, or `null` |
| `GetAllAsync` | Return all stored tasks |
| `GetByStatusAsync` | Return all tasks matching the given status |
| `GetByPriorityAsync` | Return all tasks matching the given priority |
| `GetByCategoryIdAsync` | Return all tasks whose `CategoryId` matches the given id |
| `GetOverdueAsync` | Return tasks where `DueDate` is before the `referenceDate` **and** `Status` is not `Completed`. Tasks without a `DueDate` are never overdue |
| `UpdateAsync` | If a task with the given id exists, replace it and return the updated version. Otherwise return `null` |
| `DeleteAsync` | Remove the task by id. Return `true` if found and removed, `false` otherwise |

**Tests**: `tests/CleanTasks.Tests/Infrastructure/`

---

## Phase 2: Application — Service Implementations

### `CategoryService`
**File**: `src/CleanTasks.Application/Services/CategoryService.cs`
**Implements**: `ICategoryService`

This service handles business logic for categories. It receives DTOs from the API layer, validates them, converts between DTOs and domain entities, and delegates persistence to the repository.

**Business Rules**:
- Category name must not be empty or whitespace — throw `ArgumentException`
- Category name must be unique (case-insensitive) — throw `InvalidOperationException`
- When updating, if the name hasn't changed (same entity), it should not be treated as a duplicate

| Method | Behavior |
|---|---|
| `GetByIdAsync` | Retrieve entity from repo, map to `CategoryDto`, return it (or `null` if not found) |
| `GetAllAsync` | Retrieve all entities, map each to `CategoryDto` |
| `CreateAsync` | Validate name is not empty/whitespace. Validate name is unique. Generate a new `Guid` for the id. Create entity, persist via repo, return as `CategoryDto` |
| `UpdateAsync` | Return `null` if the category doesn't exist. Validate name is not empty/whitespace. Validate name uniqueness (allow keeping the same name). Update the entity fields and persist. Return as `CategoryDto` |
| `DeleteAsync` | Delegate to repository, return the boolean result |

### `TaskService`
**File**: `src/CleanTasks.Application/Services/TaskService.cs`
**Implements**: `ITaskService`

This service handles business logic for tasks.

**Business Rules**:
- Task title must not be empty or whitespace — throw `ArgumentException`
- If a `CategoryId` is provided, that category must exist in the repository — throw `ArgumentException` if not found
- If `CategoryId` is `null`, skip category validation entirely
- New tasks always start with status `Pending` and `CreatedAt` set to `DateTime.UtcNow`
- `GetOverdueAsync` should pass `DateTime.UtcNow` as the reference date to the repository

| Method | Behavior |
|---|---|
| `GetByIdAsync` | Retrieve entity, map to `TaskItemDto`, return (or `null`) |
| `GetAllAsync` | Retrieve all entities, map to DTOs |
| `GetByStatusAsync` | Delegate filtering to repo, map results to DTOs |
| `GetByPriorityAsync` | Delegate filtering to repo, map results to DTOs |
| `GetByCategoryAsync` | Delegate filtering to repo, map results to DTOs |
| `GetOverdueAsync` | Call repo with `DateTime.UtcNow`, map results to DTOs |
| `CreateAsync` | Validate title. Validate category if provided. Generate id, set `CreatedAt` to `DateTime.UtcNow`, set status to `Pending`. Create entity, persist, return as DTO |
| `UpdateAsync` | Return `null` if task doesn't exist. Validate title. Validate category if provided. Update entity fields, persist, return as DTO |
| `CompleteAsync` | Return `null` if task doesn't exist. Set status to `Completed`, persist, return as DTO |
| `DeleteAsync` | Delegate to repository, return the boolean result |

**Mapping**: You need to manually map between entities and DTOs. Consider how you want to handle this — inline in each method, or via a private helper method. There is no automapper configured; this is intentional so you think about the mapping layer.

**Tests**: `tests/CleanTasks.Tests/Application/`

---

## Phase 3: API — Controller Implementations

### `CategoriesController`
**File**: `src/CleanTasks.Api/Controllers/CategoriesController.cs`

| Endpoint | HTTP Response |
|---|---|
| `GET /api/categories` | `200 OK` with list of categories |
| `GET /api/categories/{id}` | `200 OK` with category, or `404 Not Found` |
| `POST /api/categories` | `201 Created` with the new category and a `Location` header pointing to the get-by-id route |
| `PUT /api/categories/{id}` | `200 OK` with updated category, or `404 Not Found` |
| `DELETE /api/categories/{id}` | `204 No Content` if deleted, or `404 Not Found` |

### `TasksController`
**File**: `src/CleanTasks.Api/Controllers/TasksController.cs`

| Endpoint | HTTP Response |
|---|---|
| `GET /api/tasks` | `200 OK` with list of tasks |
| `GET /api/tasks/{id}` | `200 OK` with task, or `404 Not Found` |
| `GET /api/tasks/status/{status}` | `200 OK` with filtered list |
| `GET /api/tasks/priority/{priority}` | `200 OK` with filtered list |
| `GET /api/tasks/category/{categoryId}` | `200 OK` with filtered list |
| `GET /api/tasks/overdue` | `200 OK` with overdue tasks |
| `POST /api/tasks` | `201 Created` with new task and `Location` header |
| `PUT /api/tasks/{id}` | `200 OK` with updated task, or `404 Not Found` |
| `PATCH /api/tasks/{id}/complete` | `200 OK` with completed task, or `404 Not Found` |
| `DELETE /api/tasks/{id}` | `204 No Content` if deleted, or `404 Not Found` |

**Error handling**: When a service throws `ArgumentException` or `InvalidOperationException`, return `400 Bad Request` with the exception message. You can handle this in each action or consider a global approach — that design decision is yours.

**Tests**: Controller tests are not included — focus on getting the service and repository tests green first. You can verify the controllers manually using Swagger at `https://localhost:{port}/swagger` after running the API.

---

## Verification

1. **Run tests**: `dotnet test` from the solution root — all tests should pass
2. **Run API**: `dotnet run --project src/CleanTasks.Api` — Swagger UI available to explore and test endpoints
3. **Think about**: Why does the Application layer define service interfaces? Why are repositories registered as singletons but services as scoped? What would change if you swapped the in-memory store for a real database?
