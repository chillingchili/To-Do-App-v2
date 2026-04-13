# Phase 1: API Integration - Research

**Research Date:** 2026-04-13

## Domain Analysis

### Current Implementation Status

All 7 required APIs are already implemented in the codebase:

| API | Location | Method |
|-----|----------|--------|
| Sign in | LoginViewModel.cs | LoginAsync |
| Sign Up | RegisterViewModel.cs | RegisterAsync |
| Add To Do Task | AddToDoViewModel.cs | SaveAsync |
| Get To Do Task | ToDoListViewModel.cs | LoadItemsAsync |
| Update To Do Task | EditToDoViewModel.cs | SaveAsync |
| Change To Do Status | EditToDoViewModel.cs | MarkCompleteAsync |
| Delete To Do Task | EditToDoViewModel.cs | DeleteAsync |

### Data Layer

**DatabaseService** provides all CRUD operations:
- `GetUserAsync(email)` - Retrieves user by email
- `CreateUserAsync(user)` - Creates new user
- `GetToDoItemsAsync(userId, status)` - Gets todos by status
- `InsertToDoAsync(item)` - Creates new todo
- `UpdateToDoAsync(item)` - Updates existing todo
- `DeleteToDoAsync(item)` - Deletes todo

**ToDoClass** model:
- item_id (Primary Key, Auto-increment)
- item_name, item_description (strings)
- status ("pending" or "completed")
- user_id (foreign key)

**UserClass** model:
- id, email, name, password_hash

## Validation

### Error Handling Strategy

Current implementation uses:
- Try-catch blocks in ViewModels
- Alert dialogs for user feedback
- IsBusy flag for loading states

### Input Validation

- Login: Email and password required
- Register: All fields required, password match, min 6 chars
- Add/Edit ToDo: Name required

---

*Research complete: 2026-04-13*