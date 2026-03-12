using SQLite;
using MauiApp1.Models;

namespace MauiApp1.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _db;

    private async Task InitAsync()
    {
        if (_db is not null) return;
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "todoapp.db3");
        _db = new SQLiteAsyncConnection(dbPath);
        await _db.CreateTableAsync<UserClass>();
        await _db.CreateTableAsync<ToDoClass>();
    }

    // ── Users ─────────────────────────────────────────────────────────────────

    public async Task<UserClass?> GetUserAsync(string email)
    {
        await InitAsync();
        return await _db!.Table<UserClass>()
                         .Where(u => u.email == email)
                         .FirstOrDefaultAsync();
    }

    public async Task<int> CreateUserAsync(UserClass user)
    {
        await InitAsync();
        return await _db!.InsertAsync(user);
    }

    // ── ToDo Items ─────────────────────────────────────────────────────────────

    public async Task<List<ToDoClass>> GetToDoItemsAsync(int userId, string status)
    {
        await InitAsync();
        return await _db!.Table<ToDoClass>()
                         .Where(t => t.user_id == userId && t.status == status)
                         .ToListAsync();
    }

    public async Task<int> InsertToDoAsync(ToDoClass item)
    {
        await InitAsync();
        return await _db!.InsertAsync(item);
    }

    public async Task<int> UpdateToDoAsync(ToDoClass item)
    {
        await InitAsync();
        return await _db!.UpdateAsync(item);
    }

    public async Task<int> DeleteToDoAsync(ToDoClass item)
    {
        await InitAsync();
        return await _db!.DeleteAsync(item);
    }
}
