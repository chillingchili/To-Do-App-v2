using SQLite;

namespace MauiApp1.Models;

[Table("users")]
public class UserClass
{
    [PrimaryKey, AutoIncrement]
    public int id { get; set; }

    public string name { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string password_hash { get; set; } = string.Empty;
}
